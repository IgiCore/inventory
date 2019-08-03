using System;
using IgiCore.Inventory.Shared.Models;

namespace IgiCore.Inventory.Shared.Exceptions
{
	public class MaxWeightExceededException : Exception
	{
		public IItem Item { get; }
		public IContainer Container { get; }

		public float CurrentTotalWeight { get; }
		public MaxWeightExceededException(IItem item, IContainer container, float currentTotalWeight)
		{
			this.Item = item;
			this.Container = container;
			this.CurrentTotalWeight = currentTotalWeight;
		}
	}
}
