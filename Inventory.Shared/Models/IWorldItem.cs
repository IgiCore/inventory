using System;
using NFive.SDK.Core.Models;

namespace IgiCore.Inventory.Shared.Models
{
	public interface IWorldItem : IIdentityModel
	{
		Item Item { get; set; }

		Guid ItemId { get; set; }

		Position Position { get; set; }
	}
}
