using System;
using Godot;
using Websocket.Client;
public class WSClient : WebSocketBase {
    public WSClient(Server server) : base(server) {
    }

    public new void Initialize() {
        base.Initialize();

        SetOnReconnectionHappened((info) => {
            switch (info.Type) {
                case ReconnectionType.Lost:
                    this.Initialize();
                    return;
            }
        });

        Register();
    }

    void Register() {
        SendMessage(new() {
            action = WSActions.Register,
            type = WSMessageType.Core,
            simplePayload = "Atago"
        });
    }

    public new void SendMessage(WSMessage data) {
        Callable.From(() => base.SendMessage(data)).CallDeferred();
    }
}


public static class WSActions {
    public const string Register = "Register";
    public const string SayHello = "SayHello";
    public const string PublishToAllClients = "PublishToAllClients";
    public const string AddPlayer = "AddPlayer";
    public const string AddUnit = "AddUnit";
    public const string AddExistingUnit = "AddExistingUnit";
    public const string CalculateMutations = "CalculateMutations";
    public const string GetMutatedAttributes = "GetMutatedAttributes";
    public const string ApplyDamage = "ApplyDamage";
    public const string AddMutation = "AddMutation";
}

public static class WSMessageType {
    public const string Core = "Core";
    public const string Unit = "Unit";
    public const string Player = "Player";
}