using System;
using System.Collections.Generic;
using NFive.SDK.Core.Models;

namespace IgiCore.Inventory.Shared.Models
{
	public interface IContainer : IIdentityModel
	{
		Container ParentContainer { get; set; }

		Guid? ParentContainerId { get; set; }

		List<ContainerItem> Items { get; set; }

		uint MaxWeight { get; set; }

		uint Width { get; set; }

		uint Height{ get; set; }
	}
}
