using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using NFive.SDK.Core.Helpers;
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
		public virtual List<Item> Items { get; set; }

		public int Width { get; set; }

		public int Height { get; set; }

		public int MaxWeight { get; set; }

		public Container()
		{
			Id = GuidGenerator.GenerateTimeBasedGuid();
			Created = DateTime.UtcNow;
			Items = new List<Item>();
		}
	}
}
