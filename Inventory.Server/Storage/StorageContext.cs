using System.Data.Entity;
using IgiCore.Inventory.Server.Models;
using NFive.SDK.Server.Storage;

namespace IgiCore.Inventory.Server.Storage
{
	public class StorageContext : EFContext<StorageContext>
	{
		public virtual DbSet<Container> Containers { get; set; }

		public virtual DbSet<Item> Items { get; set; }

		public virtual DbSet<ItemDefinition> ItemDefinitions { get; set; }

		public virtual DbSet<WorldItem> WorldItems { get; set; }
	}
}
