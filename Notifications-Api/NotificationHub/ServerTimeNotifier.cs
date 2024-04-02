
using Microsoft.AspNetCore.SignalR;

namespace Notifications_Api.NotificationHub
{
    public class ServerTimeNotifier : BackgroundService
    {
        private static readonly TimeSpan _period = TimeSpan.FromSeconds(5);
        private readonly ILogger<ServerTimeNotifier> _logger;
        private readonly IHubContext<NotificationsHub, INotificationClient> _context;

        public ServerTimeNotifier(ILogger<ServerTimeNotifier> logger, IHubContext<NotificationsHub, INotificationClient> context)
        {
            _logger = logger;
            _context = context;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var timer = new PeriodicTimer(_period);

            while (!stoppingToken.IsCancellationRequested
                && await timer.WaitForNextTickAsync(stoppingToken))
            {
                var dateTime = DateTime.Now;

                _logger.LogInformation("Executando {Service} {Timer}", nameof(ServerTimeNotifier), dateTime);

                await _context.Clients.All.ReceiveNotification($"Server time = {dateTime}");
            }
        }
    }
}
