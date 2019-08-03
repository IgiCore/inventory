namespace IgiCore.Inventory.Shared
{
	public static class InventoryEvents
	{
		public const string Configuration = "igicore:inventory:configuration";

		public const string CreateContainer = "igicore:inventory:container:create";
		public const string DeleteContainer = "igicore:inventory:container:delete";
		public const string UpdateContainer = "igicore:inventory:container:update";

		public const string CreateItem = "igicore:inventory:item:create";
		public const string DeleteItem = "igicore:inventory:item:delete";
		public const string UpdateItem = "igicore:inventory:item:update";
		public const string UseItem = "igicore:inventory:item:use";
		public const string TransferItemToWorld = "igicore:inventory:item:movetoworld";

		public const string CreateWorldItem = "igicore:inventory:worlditem:create";
		public const string DeleteWorldItem = "igicore:inventory:worlditem:delete";
		public const string UpdateWorldItem = "igicore:inventory:worlditem:update";

		public const string ContainerAddItem = "igicore:inventory:container:add";
		public const string ContainerRemoveItem = "igicore:inventory:container:remove";
		public const string ContainerMoveItem = "igicore:inventory:container:move";
		public const string ContainerTransferItem = "igicore:inventory:container:transfer";
	}
}
