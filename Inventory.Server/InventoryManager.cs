using IgiCore.Inventory.Server.Models;
using IgiCore.Inventory.Server.Storage;
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
		public WorldItem CreateWorldItem(IWorldItem worldItemToCreate)
		{
			var worldItem = (WorldItem) worldItemToCreate;
			worldItem.Id = GuidGenerator.GenerateTimeBasedGuid();

			using (var context = new StorageContext())
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

		public Item CreateItem(IItem itemToCreate)
		{
			var item = (Item) itemToCreate;
			item.Id = GuidGenerator.GenerateTimeBasedGuid();

			using (var context = new StorageContext())
			using (var transaction = context.Database.BeginTransaction())
			{
				try { 
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

		public Container CreateContainer(IContainer containerToCreate)
		{
			var container = (Container) containerToCreate;
			container.Id = GuidGenerator.GenerateTimeBasedGuid();

			using (var context = new StorageContext())
			using (var transaction = context.Database.BeginTransaction())
			{
				try { 
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
	}
}
