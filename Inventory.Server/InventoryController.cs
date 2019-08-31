using System;
using System.Data.Entity.Migrations;
using System.Threading.Tasks;
using IgiCore.Inventory.Server.Models;
using IgiCore.Inventory.Server.Storage;
using JetBrains.Annotations;
using NFive.SDK.Core.Diagnostics;
using NFive.SDK.Server.Controllers;
using NFive.SDK.Server.Events;
using NFive.SDK.Server.Rcon;
using NFive.SDK.Server.Rpc;
using IgiCore.Inventory.Shared;

namespace IgiCore.Inventory.Server
{
	[PublicAPI]
	public class InventoryController : ConfigurableController<Configuration>
	{
		public InventoryController(ILogger logger, IEventManager events, IRpcHandler rpc, IRconManager rcon, Configuration configuration) : base(logger, events, rpc, rcon, configuration)
		{
			// Send configuration when requested
			this.Rpc.Event(InventoryEvents.Configuration).On(e => e.Reply(this.Configuration));
		}

		public override Task Started()
		{
			try
			{
				using (var context = new StorageContext())
				using (var transaction = context.Database.BeginTransaction())
				{
					var bottleDefinition = new ItemDefinition
					{
						Name = "Bottle",
						Description = "No longer wet.",
						Weight = 50,
						Width = 1,
						Height = 1,
						TotalUses = 0
					};
					var cakeDefinition = new ItemDefinition
					{
						Name = "Cake",
						Description = "Not a lie.",
						Weight = 5000,
						Width = 2,
						Height = 2,
						TotalUses = 10
					};
					context.ItemDefinitions.AddOrUpdate(
						cakeDefinition,
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
						bottleDefinition
					);

					context.SaveChanges();
					transaction.Commit();
				}
			}
			catch (Exception e)
			{
				this.Logger.Debug(e.GetType().Name);
				this.Logger.Debug(e.Message);
				this.Logger.Debug(e.StackTrace);
				this.Logger.Debug(e.InnerException?.Message ?? "");
			}

			return base.Started();
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
