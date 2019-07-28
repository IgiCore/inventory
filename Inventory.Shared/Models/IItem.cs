using System;
using NFive.SDK.Core.Models;

namespace IgiCore.Inventory.Shared.Models
{
	public interface IItem : IIdentityModel
	{
		Guid ItemDefinitionId { get; set; }

		ItemDefinition ItemDefinition { get; set; }

		uint UsesRemaining { get; set; }
	}
}
