using System;
using NFive.SDK.Core.Models;

namespace IgiCore.Inventory.Shared.Models
{
	public interface IItem : IIdentityModel
	{
		Guid ItemDefinitionId { get; set; }

		Guid? ContainerId { get; set; }

		float Weight { get; set; }

		int Width { get; set; }
		
		int Height { get; set; }

		int? X { get; set; }

		int? Y { get; set; }

		bool Rotated { get; set; }

		int UsesRemaining { get; set; }
	}
}
