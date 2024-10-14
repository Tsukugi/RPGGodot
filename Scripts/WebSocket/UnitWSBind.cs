using System;
using Godot;

public class UnitWSBind {
    Unit unit;
    Server server;
    WSClient wsClient;

    public UnitWSBind(Unit unit, Server server) {
        this.unit = unit;
        this.server = server;
        wsClient = server.WSClient;
    }

    public void RegisterUnit() {
        SendMessage(WSActions.AddExistingUnit, unit.UnitAttributes.GetAttributesAsDictionary());
    }

    public void SendNewUnit() {
        SendMessage(WSActions.AddUnit);
    }

    public void CalculateMutations() {
        SendMessage(WSActions.CalculateMutations);
    }
    public void GetMutatedAttributes() {
        SendMessage(WSActions.GetMutatedAttributes);
    }
    public void ApplyDamage(double value) {
        SendMessage(WSActions.ApplyDamage, value);
    }

    public void AddMutation(MutationDTO mutation) {
        SendMessage(WSActions.AddMutation, mutation);
    }

    public void OnMessageReceivedWSEvent(WSMessage receivedMessage) {
        Callable.From(() => {
            if (receivedMessage.payload.ToString() == "Success") return;
            GD.Print(receivedMessage.payload);
            UnitAttributesDTO payload = JSONLoader.ForceToType<UnitAttributesDTO>(receivedMessage.payload);
            switch (receivedMessage.action) {
                case WSActions.CalculateMutations:
                    unit.UnitAttributes.ApplyMutations(payload);
                    break;
                case WSActions.GetMutatedAttributes:
                    unit.UnitAttributes.ApplyMutatedAttributes(payload);
                    break;

                default: GD.Print(" ---- > MessageReceivedEnd"); break;
            }
        }).CallDeferred();
    }

    void SendMessage(string action, object payload = null, string simplePayload = null) {
        wsClient.SendMessage(new() {
            unitName = unit.Name.ToString(),
            playerName = unit.Player.Name.ToString(),
            action = action,
            type = WSMessageType.Unit,
            payload = payload,
            simplePayload = simplePayload
        });
    }

}