using System;
using NFive.SDK.Core.Models;

namespace IgiCore.Inventory.Shared.Models
{
	public interface IItem : IIdentityModel
	{
		Guid ItemDefinitionId { get; set; }

		ItemDefinition ItemDefinition { get; set; }

		Guid? ContainerId { get; set; }

		int? X { get; set; }

		int? Y { get; set; }

		int UsesRemaining { get; set; }
	}
}
