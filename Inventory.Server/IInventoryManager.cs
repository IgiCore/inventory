using System;
using System.Collections.Generic;
using System.Data.Entity;
using IgiCore.Inventory.Server.Models;
using IgiCore.Inventory.Server.Storage;
using IgiCore.Inventory.Shared.Models;

namespace IgiCore.Inventory.Server
{
	public interface IInventoryManager
	{
		StorageContext GetContext();
		DbContextTransaction BeginTransaction(StorageContext context);
		void WithContainerLock(Guid containerId, Action action);
		void WithItemLock(Guid itemId, Action action);
		List<ItemDefinition> GetItemDefinitions();
		List<Item> GetItems();
		List<Container> GetContainers();
		List<WorldItem> GetWorldItems();
		Item CreateItem(IItem itemToCreate);
		WorldItem CreateWorldItem(IWorldItem worldItemToCreate);
		WorldItem RemoveWorldItem(Guid worldItemId);
		Container CreateContainer(IContainer containerToCreate);
		void AddItemToContainer(Guid itemToAddId, Guid containerToAddId, int x, int y, bool rotated = false);
		void AddItemToContainer(IItem itemToAdd, IContainer containerToAdd, int x, int y, bool rotated = false);
		void RemoveItemFromContainer(Guid itemId, Guid containerId);
		void CanItemFitInContainerAt(int x, int y, IItem itemToCheck, IContainer containerToCheck);
		void CanItemWeightFitInContainer(IItem item, Container container);
		void CanItemSizeFitInContainerAt(int x, int y, IItem item, Container container);
		void CanItemFitInContainerBoundsAt(int x, int y, IItem item, IContainer container);
		void DoesItemCollideInContainerAt(int x, int y, IItem item, Container container);
		void MoveItemWithinContainer(Guid itemId, Guid containerId, int moveToX, int moveToY);

		void MoveItemBetweenContainers(Guid itemId, Guid sourceContainerId, Guid targetContainerId, int moveToX,
			int moveToY);

		void DropItem(Guid itemId, Guid sourceContainerId, float xPos, float yPos, float zPos);
		void PickupItem(Guid worldItemId, Guid targetContainerId, int moveToX, int moveToY);
		void UseItem(Guid itemId);
	}
}
