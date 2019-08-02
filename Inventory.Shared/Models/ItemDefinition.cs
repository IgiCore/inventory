using System;
using NFive.SDK.Core.Helpers;
using NFive.SDK.Core.Models;

namespace IgiCore.Inventory.Shared.Models
{
	public class ItemDefinition : IdentityModel, IItemDefinition
	{
		public string Name { get; set; }

		public string Description { get; set; }

		public string Image { get; set; }

		public string Model { get; set; }

		public int Weight { get; set; }

		public int Width { get; set; }

		public int Height { get; set; }

		public int TotalUses { get; set; }

		public ItemDefinition()
		{
			Id = GuidGenerator.GenerateTimeBasedGuid();
			Created = DateTime.UtcNow;
		}
	}
}
