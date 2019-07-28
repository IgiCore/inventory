using System;
using NFive.SDK.Core.Models;

namespace IgiCore.Inventory.Shared.Models
{
	public interface IContainerItem
	{
		Item Item { get; set; }

		Container Container { get; set; }

		uint X { get; set; }

		uint Y { get; set; }
	}
}
