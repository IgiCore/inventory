using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using NFive.SDK.Core.Models;

namespace IgiCore.Inventory.Shared.Models
{
	public class Container : IdentityModel, IContainer
	{

		[JsonIgnore]
		public virtual Container ParentContainer { get; set; }

		[ForeignKey("ParentContainer")]
		public Guid? ParentContainerId { get; set; }

		[JsonIgnore]
		public virtual List<ContainerItem> Items { get; set; }

		public uint Width { get; set; }

		public uint Height { get; set; }

		public uint MaxWeight { get; set; }
	}
}
