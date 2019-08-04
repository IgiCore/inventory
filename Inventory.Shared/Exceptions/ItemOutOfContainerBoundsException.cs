using System;
using IgiCore.Inventory.Shared.Models;

namespace IgiCore.Inventory.Shared.Exceptions
{
	public class ItemOutOfContainerBoundsException : Exception
	{
		public IContainer Container { get; }

		public IItem Item { get; set; }

		public ItemOutOfContainerBoundsException(IContainer container, IItem item)
		{
			this.Container = container;
			this.Item = item;
		}

		
	}
}
