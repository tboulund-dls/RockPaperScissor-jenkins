﻿using EasyNetQ;
using Events;
using Helpers;
using Monolith;

namespace RandomPlayerService;

public static class Program
{
    private static readonly IPlayer Player = new RandomPlayer();
    
    public static async Task Main()
    {
        var connectionEstablished = false;

        while (!connectionEstablished)
        {
            var bus = ConnectionHelper.GetRMQConnection();
            var subscriptionResult = bus.PubSub.SubscribeWithTracingAsync<GameStartedEvent>("RPS_" + Player.GetPlayerId(), e =>
            {                    
                
                var moveEvent = Player.MakeMove(e);
                bus.PubSub.PublishWithTracingAsync(moveEvent);
            }).AsTask();

            await subscriptionResult.WaitAsync(CancellationToken.None);
            connectionEstablished = subscriptionResult.Status == TaskStatus.RanToCompletion;
            if(!connectionEstablished) Thread.Sleep(1000);
            
            bus.PubSub.SubscribeWithTracingAsync<GameFinishedEvent>("RPS_" + Player.GetPlayerId(), e =>
            {
                Player.ReceiveResult(e);
            });
        }

        while (true) Thread.Sleep(5000);
    }
}