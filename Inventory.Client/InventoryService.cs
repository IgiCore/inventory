using JetBrains.Annotations;
using NFive.SDK.Client.Commands;
using NFive.SDK.Client.Events;
using NFive.SDK.Client.Interface;
using NFive.SDK.Client.Rpc;
using NFive.SDK.Client.Services;
using NFive.SDK.Core.Diagnostics;
using NFive.SDK.Core.Models.Player;
using System.Threading.Tasks;
using IgiCore.Inventory.Shared;

namespace IgiCore.Inventory.Client
{
	[PublicAPI]
	public class InventoryService : Service
	{
		private Configuration config;

		public InventoryService(ILogger logger, ITickManager ticks, IEventManager events, IRpcHandler rpc, ICommandManager commands, OverlayManager overlay, User user) : base(logger, ticks, events, rpc, commands, overlay, user) { }

		public override async Task Started()
		{
			// Request server configuration
			this.config = await this.Rpc.Event(InventoryEvents.Configuration).Request<Configuration>();

			// Update local configuration on server configuration change
			this.Rpc.Event(InventoryEvents.Configuration).On<Configuration>((e, c) => this.config = c);
		}
	}
}
