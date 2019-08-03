using System;
using IgiCore.Inventory.Shared.Models;

namespace IgiCore.Inventory.Shared.Exceptions
{
	public class ItemOverlapException : Exception
	{
		public IItem Item { get; }

		public IContainer Container { get; }

		public Guid OverlappingItemId { get; }

		public ItemOverlapException(IItem item, IContainer container, Guid overlappingItemId)
		{
			this.Item = item;
			this.Container = container;
			this.OverlappingItemId = overlappingItemId;
		}
	}
}
