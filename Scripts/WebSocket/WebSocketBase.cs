using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using Godot;
using Newtonsoft.Json;
using Websocket.Client;

public class WebSocketBase {
    const string LocalWebSocketServer = "ws://127.0.0.1:3000";
    Uri url;
    Server server;
    protected WebsocketClient client = null;
    IDisposable messageReceivedRef = null;
    IDisposable reconnectionRef = null;

    public WebSocketBase(Server server) {
        this.server = server;
    }

    public void Initialize() {
        url = new Uri(LocalWebSocketServer);

        if (client is not null) {
            client.Stop(WebSocketCloseStatus.NormalClosure, "Replace");
            client.Dispose();
        }

        client = new WebsocketClient(url);

        client.StartOrFail();

        SetReconnectTimeout(60);

        SetOnReconnectionHappened((info) => {
            GD.Print("Reconnection happened, type: " + info.Type);
            switch (info.Type) {
                case ReconnectionType.Lost: Initialize(); return;
            }
        });

        SetOnMessageReceived((message) => {
            GD.Print("[OnMessageReceived] Message received: " + message);
            WSMessage receivedMessage = JsonConvert.DeserializeObject<WSMessage>(message.ToString());
            switch (receivedMessage.type) {
                case WSMessageType.Unit: TriggerUnitEvent(receivedMessage); break;
            }
        });
    }

    void TriggerUnitEvent(WSMessage receivedMessage) {
        Callable.From(() => {
            Unit mutatedUnit = server.Manager.FindUnit(receivedMessage.playerName, receivedMessage.unitName); 
            mutatedUnit.UnitWSBind.OnMessageReceivedWSEvent(receivedMessage);
        }).CallDeferred();
    }

    public void SetReconnectTimeout(int seconds) {
        client.ReconnectTimeout = TimeSpan.FromSeconds(seconds);
    }
    public void SetOnReconnectionHappened(Action<ReconnectionInfo> onReconnection) {
        SetDisposable(reconnectionRef, client.ReconnectionHappened.Subscribe(onReconnection));
    }
    public void SetOnMessageReceived(Action<ResponseMessage> onMessageReceived) {
        SetDisposable(messageReceivedRef, client.MessageReceived.Subscribe(onMessageReceived));
    }
    void SetDisposable(IDisposable target, IDisposable value) {
        if (target is not null) target.Dispose();
        target = value;
    }

    public void SendMessage(WSMessage message) {
        client.Send(SerializeAction(message));
    }

    string SerializeAction(WSMessage message) {
        string jsonString = JsonConvert.SerializeObject(message);
        GD.Print($"[SerializeAction] Send => {jsonString}");
        return jsonString;
    }
}


public class WSMessage {
    public string type;
    public string action; // Mapped to WSActions
    public string unitName;
    public string playerName;
    public object payload;
    public string simplePayload;
}
