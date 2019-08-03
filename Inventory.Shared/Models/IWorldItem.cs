using System;
using NFive.SDK.Core.Models;

namespace IgiCore.Inventory.Shared.Models
{
	public interface IWorldItem : IIdentityModel
	{

		Guid ItemId { get; set; }

		Position Position { get; set; }
	}
}
