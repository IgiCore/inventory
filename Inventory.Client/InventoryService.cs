using JetBrains.Annotations;
using NFive.SDK.Client.Commands;
using NFive.SDK.Client.Events;
using NFive.SDK.Client.Services;
using NFive.SDK.Core.Diagnostics;
using NFive.SDK.Core.Models.Player;
using System.Threading.Tasks;
using IgiCore.Inventory.Shared;
using NFive.SDK.Client.Communications;
using NFive.SDK.Client.Interface;

namespace IgiCore.Inventory.Client
{
	[PublicAPI]
	public class InventoryService : Service
	{
		private readonly ICommunicationManager comms;
		private Configuration config;

		public InventoryService(ILogger logger, ITickManager ticks, ICommunicationManager comms, ICommandManager commands, IOverlayManager overlay, User user) : base(logger, ticks, comms, commands, overlay, user)
		{
			this.comms = comms;
		}

		public override async Task Started()
		{
			// Request server configuration
			this.config = await this.comms.Event(InventoryEvents.Configuration).ToServer().Request<Configuration>();

			// Update local configuration on server configuration change
			this.comms.Event(InventoryEvents.Configuration).FromServer().On<Configuration>((e, c) => this.config = c);
		}
	}
}
