using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using IgiCore.Inventory.Server.Models;
using IgiCore.Inventory.Server.Storage;
using IgiCore.Inventory.Shared.Exceptions;
using IgiCore.Inventory.Shared.Models;
using JetBrains.Annotations;
using NFive.SDK.Core.Helpers;
using NFive.SDK.Core.IoC;
using NFive.SDK.Core.Models;

// ReSharper disable AccessToDisposedClosure
namespace IgiCore.Inventory.Server
{
	[Component(Lifetime = Lifetime.Singleton)]
	[PublicAPI]
	// ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
	public class InventoryManager
	{
		private readonly Dictionary<Guid, object> containerLocks = new Dictionary<Guid, object>();
		private readonly Dictionary<Guid, object> itemLocks = new Dictionary<Guid, object>();

		public virtual StorageContext GetContext() => new StorageContext();

		public virtual DbContextTransaction BeginTransaction(StorageContext context) =>
			context.Database.BeginTransaction();

		public void WithContainerLock(Guid containerId, Action action)
		{
			if (!this.containerLocks.ContainsKey(containerId))
			{
				this.containerLocks[containerId] = new object();
			}
			lock (this.containerLocks[containerId])
			{
				action();
			}
		}

		public void WithItemLock(Guid itemId, Action action)
		{
			if (!this.itemLocks.ContainsKey(itemId))
			{
				this.itemLocks[itemId] = new object();
			}
			lock (this.itemLocks[itemId])
			{
				action();
			}
		}

		public Item CreateItem(IItem itemToCreate)
		{
			var item = (Item)itemToCreate;
			item.Id = GuidGenerator.GenerateTimeBasedGuid();

			using (var context = GetContext())
			{
				context.Items.Add(item);
				context.SaveChanges();
			}

			return item;
		}

		public WorldItem CreateWorldItem(IWorldItem worldItemToCreate)
		{
			var worldItem = (WorldItem)worldItemToCreate;
			worldItem.Id = GuidGenerator.GenerateTimeBasedGuid();

			using (var context = GetContext())
			{
				WithItemLock(worldItem.ItemId, () =>
				{
					context.WorldItems.Add(worldItem);
					context.SaveChanges();
				});
			}

			return worldItem;
		}

		public WorldItem RemoveWorldItem(Guid worldItemId)
		{
			using (var context = GetContext())
			{
				var worldItem = context.WorldItems.First(w => w.Id == worldItemId);
				context.WorldItems.Remove(worldItem);
				context.SaveChanges();
				return worldItem;
			}
		}

		public Container CreateContainer(IContainer containerToCreate)
		{
			var container = (Container)containerToCreate;
			container.Id = GuidGenerator.GenerateTimeBasedGuid();

			using (var context = GetContext())
			{
				context.Containers.Add(container);
				context.SaveChanges();
			}

			return container;
		}

		protected Container GetContainerById(Guid id)
		{
			using (var context = GetContext())
			{
				return context.Containers.First(c => c.Id == id);
			}
		}

		protected Item GetItemById(Guid id)
		{
			using (var context = GetContext())
			{
				return context.Items.Find(id);
			}
		}

		public void AddItemToContainer(Guid itemToAddId, Guid containerToAddId, int x, int y, bool rotated = false)
		{
			Item item;
			Container container;

			using (var context = GetContext())
			{
				item = context.Items.First(i => i.Id == itemToAddId);
				container = context.Containers.First(i => i.Id == containerToAddId);
			}

			AddItemToContainer(item, container, x, y, rotated);
		}

		public void AddItemToContainer(IItem itemToAdd, IContainer containerToAdd, int x, int y, bool rotated = false)
		{
			using (var context = GetContext())
			using (var transaction = context.Database.BeginTransaction())
			{
				try
				{
					var item = (Item)itemToAdd;
					var container = (Container)containerToAdd;

					CanItemFitInContainerAt(x, y, item, container);

					item.ContainerId = container.Id;
					item.X = x;
					item.Y = y;
					item.Rotated = rotated;
					WithContainerLock(container.Id, () =>
					{
						context.Items.AddOrUpdate(item);
						context.SaveChanges();
						transaction.Commit();
					});
				}
				finally
				{
					transaction.Rollback();
				}
			}
		}

		public void RemoveItemFromContainer(Guid itemId, Guid containerId)
		{
			Item item;
			Container container;

			using (var context = GetContext())
			{
				item = context.Items.First(i => i.Id == itemId);
				container = context.Containers.First(i => i.Id == containerId);
			}

			RemoveItemFromContainer(item, container);
		}

		private Item RemoveItemFromContainer(IItem itemToRemove, IContainer containerToRemoveFrom)
		{
			using (var context = GetContext())
			{
				var item = (Item)itemToRemove;
				var container = (Container)containerToRemoveFrom;

				item.ContainerId = null;
				item.X = null;
				item.Y = null;
				item.Rotated = false;

				WithContainerLock(container.Id, () =>
				{
					context.Items.Remove(item);
					context.SaveChanges();
				});

				return item;
			}
		}

		public void CanItemFitInContainerAt(int x, int y, IItem itemToCheck, IContainer containerToCheck)
		{
			Container container;
			using (var context = GetContext())
			{
				container = context.Containers.Include(c => c.Items).First(c => c.Id == containerToCheck.Id);
			}

			CanItemWeightFitInContainer(itemToCheck, container);
			CanItemSizeFitInContainerAt(x, y, itemToCheck, container);
		}

		public void CanItemWeightFitInContainer(IItem item, Container container)
		{
			WithContainerLock(container.Id, () =>
			{
				var containerTotalWeight = container.Items.Sum(i => i.Weight);
				if (containerTotalWeight + item.Weight > container.MaxWeight)
				{
					throw new MaxWeightExceededException(item, container, containerTotalWeight);
				}
			});
		}

		public void CanItemSizeFitInContainerAt(int x, int y, IItem item, Container container)
		{
			CanItemFitInContainerBoundsAt(x, y, item, container);
			DoesItemCollideInContainerAt(x, y, item, container);
		}

		public void CanItemFitInContainerBoundsAt(int x, int y, IItem item, IContainer container)
		{
			WithContainerLock(container.Id, () =>
			{
				if (x < 0 || y < 0 || x + item.Width > container.Width || y + item.Height > container.Height)
				{
					throw new ItemOutOfContainerBoundsException(container, item);
				}
			});
		}

		public void DoesItemCollideInContainerAt(int x, int y, IItem item, Container container)
		{
			WithContainerLock(container.Id, () =>
			{
				foreach (IItem i in container.Items)
				{
					if (i.Id == item.Id) continue;
					if (!(x > i.X + i.Width - 1) // Item's left edge is not to the right of i's right edge
					    && !(x + item.Width < i.X) // Item's right edge is not to the left of i's left edge
					    && !(y + item.Height - 1 < i.Y) // Item's bottom edge is not above i's top edge
					    && !(y > i.Y + i.Width - 1)) // Item's top edge is not below i's bottom edge
					{
						throw new ItemOverlapException(item, container, i.ContainerId ?? Guid.Empty);
					}
				}
			});
		}

		public void MoveItemWithinContainer(Guid itemId, Guid containerId, int moveToX, int moveToY)
		{
			WithContainerLock(containerId, () =>
			{
				var container = GetContainerById(containerId);
				var item = container.Items.First(i => i.Id == itemId);

				CanItemSizeFitInContainerAt(moveToX, moveToY, item, container);
				item.X = moveToX;
				item.Y = moveToY;

				using (var context = GetContext())
				{
					context.Items.AddOrUpdate(item);
					context.SaveChanges();
				}
			});
		}

		public void MoveItemBetweenContainers(Guid itemId, Guid sourceContainerId, Guid targetContainerId, int moveToX,
			int moveToY)
		{
			// TODO: Add shared locking/transaction.
			RemoveItemFromContainer(itemId, sourceContainerId);
			AddItemToContainer(itemId, targetContainerId, moveToX, moveToY);
		}

		public void DropItem(Guid itemId, Guid sourceContainerId, float xPos, float yPos, float zPos)
		{
			// TODO: Add shared locking/transaction.
			RemoveItemFromContainer(itemId, sourceContainerId);
			CreateWorldItem(new WorldItem()
			{
				ItemId = itemId,
				Position = new Position(xPos, yPos, zPos)
			});
		}

		public void PickupItem(Guid worldItemId, Guid targetContainerId, int moveToX, int moveToY)
		{
			// TODO: Add shared locking/transaction.
			var worldItem = RemoveWorldItem(worldItemId);
			AddItemToContainer(worldItem.ItemId, targetContainerId, moveToX, moveToY);
		}

		public void UseItem(Guid itemId)
		{
			using (var context = GetContext())
			{
				var item = context.Items.First(i => i.Id == itemId);
				WithItemLock(item.Id, () =>
				{
					if (item.UsesRemaining <= 0)
					{
						throw new ItemNoUsesRemainingException(item);
					}
					item.UsesRemaining--;
					context.Items.AddOrUpdate(item);
					context.SaveChanges();
				});
			}
		}
	}
}
