using JetBrains.Annotations;
using NFive.SDK.Server.Migrations;
using IgiCore.Inventory.Server.Storage;

namespace IgiCore.Inventory.Server.Migrations
{
	[UsedImplicitly]
	public sealed class Configuration : MigrationConfiguration<StorageContext>
	{
	}
}
