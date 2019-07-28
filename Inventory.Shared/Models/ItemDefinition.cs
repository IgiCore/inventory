using NFive.SDK.Core.Models;

namespace IgiCore.Inventory.Shared.Models
{
	public class ItemDefinition : IdentityModel, IItemDefinition
	{
		public string Name { get; set; }

		public string Description { get; set; }

		public byte[] Image { get; set; }

		public string Model { get; set; }

		public uint Weight { get; set; }

		public uint Width { get; set; }

		public uint Height { get; set; }

		public uint TotalUses { get; set; }
	}
}
