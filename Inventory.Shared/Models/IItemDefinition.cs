using NFive.SDK.Core.Models;

namespace IgiCore.Inventory.Shared.Models
{
	public interface IItemDefinition : IIdentityModel
	{
		string Name { get; set; }

		string Description { get; set; }

		string Image { get; set; }

		string Model { get; set; }

		int Weight { get; set; }

		int Width { get; set; }

		int Height { get; set; }

		int TotalUses { get; set; }
	}
}
