using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using IgiCore.Inventory.Server.Extensions;
using IgiCore.Inventory.Server.Models;
using IgiCore.Inventory.Server.Storage;
using IgiCore.Inventory.Shared.Exceptions;
using IgiCore.Inventory.Shared.Models;
using JetBrains.Annotations;
using NFive.SDK.Core.Helpers;
using NFive.SDK.Core.IoC;

namespace IgiCore.Inventory.Server
{
	[Component(Lifetime = Lifetime.Singleton)]
	[PublicAPI]
	public class InventoryManager
	{
		public virtual StorageContext GetContext() => new StorageContext();

		public Item CreateItem(IItem itemToCreate)
		{
			var item = (Item)itemToCreate;
			item.Id = GuidGenerator.GenerateTimeBasedGuid();

			using (var context = GetContext())
			using (var transaction = context.Database.BeginTransaction())
			{
				try
				{
					context.Items.Add(item);
					context.SaveChanges();
					transaction.Commit();
				}
				catch
				{
					transaction.Rollback();
				}
			}

			return item;
		}

		public WorldItem CreateWorldItem(IWorldItem worldItemToCreate)
		{
			var worldItem = (WorldItem)worldItemToCreate;
			worldItem.Id = GuidGenerator.GenerateTimeBasedGuid();

			using (var context = GetContext())
			using (var transaction = context.Database.BeginTransaction())
			{
				try
				{
					context.WorldItems.Add(worldItem);
					context.SaveChanges();
					transaction.Commit();
				}
				catch
				{
					transaction.Rollback();
				}

			}

			return worldItem;
		}

		public Container CreateContainer(IContainer containerToCreate)
		{
			var container = (Container)containerToCreate;
			container.Id = GuidGenerator.GenerateTimeBasedGuid();

			using (var context = GetContext())
			using (var transaction = context.Database.BeginTransaction())
			{
				try
				{
					context.Containers.Add(container);
					context.SaveChanges();
					transaction.Commit();
				}
				catch
				{
					transaction.Rollback();
				}
			}

			return container;
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
					var item = (Item) itemToAdd;
					var container = (Container) containerToAdd;

					CanItemFitInContainerAt(x, y, item, container);

					item.ContainerId = container.Id;
					item.X = x;
					item.Y = y;
					item.Rotated = rotated;
					context.Items.AddOrUpdate(item);

					context.SaveChanges();
					transaction.Commit();
				}
				finally
				{
					transaction.Rollback();
				}
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
			var containerTotalWeight = container.Items.Sum(i => i.Weight);
			if (containerTotalWeight + item.Weight > container.MaxWeight)
			{
				throw new MaxWeightExceededException(item, container, containerTotalWeight);
			}
		}

		public void CanItemSizeFitInContainerAt(int x, int y, IItem item, Container container)
		{
			CanItemFitInContainerBoundsAt(x, y, item, container);
			DoesItemCollideInContainerAt(x, y, item, container);
		}

		public void CanItemFitInContainerBoundsAt(int x, int y, IItem item, IContainer container)
		{
			if (x < 0 || y < 0 || x + item.Width > container.Width || y + item.Height > container.Height)
			{
				throw new ItemOutOfContainerBoundsException(container, item);
			}
		}

		public void DoesItemCollideInContainerAt(int x, int y, IItem item, Container container)
		{
			// TODO: Benchmark and compare matrix math to looping.

			var containerItemMatrix = container.GetItemMatrix();
			var xEnd = x + item.Width;
			var yEnd = y + item.Height;
			for (var r = x; r < xEnd; r++)
			{
				for (var c = y; c < yEnd; c++)
				{
					if (containerItemMatrix[r, c] != Guid.Empty)
					{
						throw new ItemOverlapException(item, container, containerItemMatrix[r, c]);
					}
				}
			}
		}
	}
}
