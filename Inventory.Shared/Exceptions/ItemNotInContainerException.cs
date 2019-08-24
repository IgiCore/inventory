using System;

namespace IgiCore.Inventory.Shared.Exceptions
{
	public class ItemNotInContainerException : Exception
	{
		public Guid ItemId { get; protected set; }

		public Guid ContainerId { get; protected set; }

		public ItemNotInContainerException(Guid itemId, Guid containerId)
		{
			this.ItemId = itemId;
			this.ContainerId = containerId;
		}
	}
}
