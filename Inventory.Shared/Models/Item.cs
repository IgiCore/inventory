using System;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using NFive.SDK.Core.Models;

namespace IgiCore.Inventory.Shared.Models
{
	public class Item : IdentityModel, IItem
	{
		[ForeignKey("ItemDefinition")]
		public Guid ItemDefinitionId { get; set; }

		[JsonIgnore]
		public virtual ItemDefinition ItemDefinition { get; set; }

		[ForeignKey("ContainerItem")]
		public Guid? ContainerItemId { get; set; }

		[JsonIgnore]
		public virtual ContainerItem ContainerItem { get; set; }

		public uint UsesRemaining { get; set; }
	}
}
