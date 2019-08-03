using System;
using IgiCore.Inventory.Shared.Models;
using Newtonsoft.Json;
using NFive.SDK.Core.Models;

namespace IgiCore.Inventory.Client.Models
{
	public class WorldItem : IdentityModel, IWorldItem
	{
		[JsonIgnore]
		public Item Item { get; set; }

		public Guid ItemId { get; set; }

		public Position Position { get; set; }

	}
}
