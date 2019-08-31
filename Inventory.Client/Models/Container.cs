using System;
using System.Collections.Generic;
using IgiCore.Inventory.Shared.Models;
using Newtonsoft.Json;
using NFive.SDK.Core.Models;

namespace IgiCore.Inventory.Client.Models
{
	public class Container : IdentityModel, IContainer
	{
		[JsonIgnore]
		public virtual Container ParentContainer { get; set; }

		public Guid? ParentContainerId { get; set; }

		public string Name { get; set; }

		[JsonIgnore]
		public virtual List<Item> Items { get; set; }

		public int Width { get; set; }

		public int Height { get; set; }

		public float MaxWeight { get; set; }
	}
}
