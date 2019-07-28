using NFive.SDK.Core.Models;

namespace IgiCore.Inventory.Shared.Models
{
	public interface IItemDefinition : IIdentityModel
	{
		string Name { get; set; }

		string Description { get; set; }

		byte[] Image { get; set; }

		string Model { get; set; }

		uint Weight { get; set; }

		uint Width { get; set; }

		uint Height { get; set; }

		uint TotalUses { get; set; }
	}
}
