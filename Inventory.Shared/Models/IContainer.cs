using System;
using System.Collections.Generic;
using NFive.SDK.Core.Models;

namespace IgiCore.Inventory.Shared.Models
{
	public interface IContainer : IIdentityModel
	{
		Container ParentContainer { get; set; }

		Guid? ParentContainerId { get; set; }

		List<Item> Items { get; set; }

		int MaxWeight { get; set; }

		int Width { get; set; }

		int Height { get; set; }
	}
}
