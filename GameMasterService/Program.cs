using EasyNetQ;
using Events;
using GameMasterService;
using Helpers;

public class Program
{
    private static Game game = new Game();

    public static async Task Main()
    {
        var connectionEstablished = false;

        Thread.Sleep(5000); // Let's make sure the player services are ready before we start

        using var bus = ConnectionHelper.GetRMQConnection();
        while (!connectionEstablished)
        {
            var subscriptionResult = bus.PubSub
                .SubscribeWithTracingAsync<PlayerMovedEvent>("RPS", e =>
                {
                    var finishedEvent = game.ReceivePlayerEvent(e);
                    if (finishedEvent != null)
                    {
                        bus.PubSub.PublishWithTracingAsync(finishedEvent);
                    }
                })
                .AsTask();

            await subscriptionResult.WaitAsync(CancellationToken.None);
            connectionEstablished = subscriptionResult.Status == TaskStatus.RanToCompletion;
            if (!connectionEstablished) Thread.Sleep(1000);
        }

        for (var i = 0; i < 1; i++)
        {
            using (Monitoring.ActivitySource.StartActivity("Preparing to start game"))
            {
                bus.PubSub.PublishWithTracingAsync(game.Start());
            }
            Thread.Sleep(1500); // Give the players a short break before continuing.
        }

        while (true) Thread.Sleep(5000);
    }
}