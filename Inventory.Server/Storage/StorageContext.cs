using System.Data.Entity;
using IgiCore.Inventory.Server.Models;
using NFive.SDK.Server.Storage;

namespace IgiCore.Inventory.Server.Storage
{
	public class StorageContext : EFContext<StorageContext>
	{
		public DbSet<Container> Containers { get; set; }

		public DbSet<Item> Items { get; set; }

		public DbSet<ItemDefinition> ItemDefinitions { get; set; }

		public DbSet<WorldItem> WorldItems { get; set; }
	}
}
