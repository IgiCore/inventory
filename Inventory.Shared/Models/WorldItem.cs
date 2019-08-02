using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using NFive.SDK.Core.Helpers;
using NFive.SDK.Core.Models;

namespace IgiCore.Inventory.Shared.Models
{
	public class WorldItem : IdentityModel, IWorldItem
	{
		[JsonIgnore]
		public Item Item { get; set; }

		[ForeignKey("Item")]
		public Guid ItemId { get; set; }

		[Required]
		public Position Position { get; set; }

		public WorldItem()
		{
			Id = GuidGenerator.GenerateTimeBasedGuid();
		}
	}
}
