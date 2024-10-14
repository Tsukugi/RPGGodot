using System.Collections.Generic;
using Godot;
using Newtonsoft.Json;

public class PlayerWSBind {
    PlayerBase player;
    Server server;
    WSClient wsClient;

    public PlayerWSBind(PlayerBase player, Server server) {
        this.player = player;
        this.server = server;
        wsClient = server.WSClient;
    }

    public void AddPlayer() {
        SendMessage(WSActions.AddPlayer);
    }

    public void AddMutation(MutationDTO mutation) {
        SendMessage(WSActions.AddMutation, mutation);
    }

    public void OnMessageReceivedWSEvent(WSMessage receivedMessage) {
        Callable.From(() => {
            switch (receivedMessage.action) {
                case WSActions.CalculateMutations:
                    break;
                default: GD.Print(" ---- > MessageReceivedEnd"); break;
            }
        }).CallDeferred();
    }

    void SendMessage(string action, object payload = null, string simplePayload = null) {
        wsClient.SendMessage(new() {
            playerName = player.Name.ToString(),
            action = action,
            type = WSMessageType.Player,
            payload = payload,
            simplePayload = simplePayload
        });
    }

    WSMessage CreateWSPayload(object simplePayload = null) {
        return new() {
            playerName = player.Name.ToString(),
            simplePayload = simplePayload?.ToString()
        };
    }
    WSMessage CreateWSPayload(Dictionary<string, object> payload) {
        return new() {
            playerName = player.Name.ToString(),
            payload = payload
        };
    }

}