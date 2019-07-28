using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using NFive.SDK.Core.Models;

namespace IgiCore.Inventory.Shared.Models
{
	public class ContainerItem : IContainerItem
	{
		[JsonIgnore]
		public virtual Item Item { get; set; }

		[Required]
		[Key, ForeignKey("Item")]
		public Guid ItemId { get; set; }

		[JsonIgnore]
		public virtual Container Container { get; set; }

		[Required]
		[ForeignKey("Container")]
		public Guid ContainerId { get; set; }

		public uint X { get; set; }

		public uint Y { get; set; }

		public DateTime Created { get; set; }

		public DateTime? Deleted { get; set; }
	}
}
