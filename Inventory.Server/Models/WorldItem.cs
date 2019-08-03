using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IgiCore.Inventory.Shared.Models;
using Newtonsoft.Json;
using NFive.SDK.Core.Models;

namespace IgiCore.Inventory.Server.Models
{
	public class WorldItem : IdentityModel, IWorldItem
	{
		[JsonIgnore]
		public Item Item { get; set; }

		[ForeignKey("Item")]
		public Guid ItemId { get; set; }

		[Required]
		public Position Position { get; set; }
	}
}
