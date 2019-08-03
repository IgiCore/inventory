using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IgiCore.Inventory.Shared.Models;
using Newtonsoft.Json;
using NFive.SDK.Core.Models;

namespace IgiCore.Inventory.Server.Models
{
	public class Item : IdentityModel, IItem
	{
		[Required]
		[ForeignKey("ItemDefinition")]
		public Guid ItemDefinitionId { get; set; }

		[JsonIgnore]
		public virtual ItemDefinition ItemDefinition { get; set; }

		[JsonIgnore]
		public virtual Container Container { get; set; }

		public Guid? ContainerId { get; set; }

		public int? X { get; set; }

		public int? Y { get; set; }

		public bool Rotated { get; set; }

		public int UsesRemaining { get; set; }
	}
}
