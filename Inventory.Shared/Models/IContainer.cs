using System;
using NFive.SDK.Core.Models;

namespace IgiCore.Inventory.Shared.Models
{
	public interface IContainer : IIdentityModel
	{
		Guid? ParentContainerId { get; set; }

		int MaxWeight { get; set; }

		int Width { get; set; }

		int Height { get; set; }
	}
}
