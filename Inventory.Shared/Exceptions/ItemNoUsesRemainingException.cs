using System;
using IgiCore.Inventory.Shared.Models;

namespace IgiCore.Inventory.Shared.Exceptions
{
	public class ItemNoUsesRemainingException : Exception
	{
		public IItem Item { get; protected set; }

		public ItemNoUsesRemainingException(IItem item)
		{
			this.Item = item;
		}
	}
}
