using System.Data.Entity.Migrations;
using IgiCore.Inventory.Server.Storage;
using JetBrains.Annotations;
using NFive.SDK.Core.Diagnostics;
using NFive.SDK.Server.Controllers;
using NFive.SDK.Server.Events;
using NFive.SDK.Server.Rcon;
using NFive.SDK.Server.Rpc;
using IgiCore.Inventory.Shared;
using IgiCore.Inventory.Shared.Models;
using NFive.SDK.Core.Models;

namespace IgiCore.Inventory.Server
{
	[PublicAPI]
	public class InventoryController : ConfigurableController<Configuration>
	{
		public InventoryController(ILogger logger, IEventManager events, IRpcHandler rpc, IRconManager rcon, Configuration configuration) : base(logger, events, rpc, rcon, configuration)
		{
			// Send configuration when requested
			this.Rpc.Event(InventoryEvents.Configuration).On(e => e.Reply(this.Configuration));

			using (var context = new StorageContext())
			using (var transaction = context.Database.BeginTransaction())
			{
				var bottle = new ItemDefinition
				{
					Name = "Bottle",
					Description = "No longer wet.",
					Weight = 50,
					Width = 1,
					Height = 1,
					TotalUses = 0
				};

				var cake = new ItemDefinition
				{
					Name = "Cake",
					Description = "Not a lie.",
					Weight = 5000,
					Width = 2,
					Height = 2,
					TotalUses = 10
				};

				context.ItemDefinitions.AddOrUpdate(
					cake,
					new ItemDefinition
					{
						Name = "Slice of Cake",
						Description = "Only a slight lie.",
						Weight = 500,
						Width = 1,
						Height = 1,
						TotalUses = 1
					},
					new ItemDefinition
					{
						Name = "Bottle of Water",
						Description = "Wet.",
						Weight = 500,
						Width = 1,
						Height = 1,
						TotalUses = 8
					},
					bottle
				);

				var container = new Container()
				{
					Height = 10,
					Width = 20,
				};

				context.Containers.AddOrUpdate(container);

				var item = new Item()
				{
					ItemDefinition = bottle,
				};

				context.ContainerItems.AddOrUpdate(new ContainerItem()
				{
					Container = container,
					Item = item,
					X = 0,
					Y = 0,
				});

				var worldItem = new Item()
				{
					ItemDefinition = cake,
				};

				context.WorldItems.AddOrUpdate(new WorldItem()
				{
					Item = worldItem,
					Position = new Position()
				});

				context.SaveChanges();
				transaction.Commit();
			}
		}

		public override void Reload(Configuration configuration)
		{
			// Update local configuration
			base.Reload(configuration);

			// Send out new configuration
			this.Rpc.Event(InventoryEvents.Configuration).Trigger(this.Configuration);
		}
	}
}
