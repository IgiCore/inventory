using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using IgiCore.Inventory.Shared.Models;
using Newtonsoft.Json;
using NFive.SDK.Core.Models;

namespace IgiCore.Inventory.Server.Models
{
	public class Container : IdentityModel, IContainer
	{
		[JsonIgnore]
		public virtual Container ParentContainer { get; set; }

		[ForeignKey("ParentContainer")]
		public Guid? ParentContainerId { get; set; }

		[JsonIgnore]
		public virtual List<Item> Items { get; set; }

		public int Width { get; set; }

		public int Height { get; set; }

		public float MaxWeight { get; set; }

		public Container()
		{
			this.Items = new List<Item>();
		}
	}
}
