using System.Collections.Generic;
using Godot;

public partial class Unit : CharacterBody3D {

    bool isKilled = false;
    Area3D interactionArea = null;
    UnitAttributes attributes;
    UnitRender unitRender;
    readonly Dictionary<string, AbilityCaster> abilities = new();
    protected UnitPlayerBind unitPlayerBind;
    protected Label3D overheadLabel;
    public PlayerBase Player { get => unitPlayerBind.Player; }
    public Area3D InteractionArea { get => interactionArea; }
    public UnitAttributes Attributes { get => attributes; }
    public Label3D OverheadLabel { get => overheadLabel; }
    public Dictionary<string, AbilityCaster> Abilities { get => abilities; }
    public bool IsKilled { get => isKilled; }
    public UnitRender UnitRender { get => unitRender; }

    public override void _Ready() {
        base._Ready();
        unitPlayerBind = new(this);
        unitRender = new(this);
        attributes = GetNode<UnitAttributes>(StaticNodePaths.Attributes);
        overheadLabel = GetNode<Label3D>(StaticNodePaths.OverheadLabel);
        interactionArea = GetNodeOrNull<Area3D>(StaticNodePaths.InteractionArea);
        attributes.OnKilled -= OnKilledHandler;
        attributes.OnKilled += OnKilledHandler;
    }

    void OnKilledHandler(Unit unit) {
        unitRender.BodyCollision.Disabled = true;
        CollisionLayer = (uint)CollisionMasks.Corpse;
        isKilled = true;
    }

    protected void MoveAndCollide(Vector3 direction, float delta) {
        // Apply velocity.
        direction = direction.Normalized() * Attributes.MovementSpeed;
        MoveAndCollide(direction * (float)delta);
    }

    protected void MoveAndSlide(Vector3 direction) {
        // Apply velocity.
        direction = direction.Normalized() * Attributes.MovementSpeed;
        Velocity = direction;
        MoveAndSlide();
    }

    public void AddAbility(AbilityDTO abilityDTO) {
        AbilityCaster abilityCaster = new(this, abilityDTO);
        abilities.Add(abilityDTO.name, abilityCaster);
    }

    public void CastAbility(string name) {
        Player.DebugLog("[CastAbility] Casting " + name, true);
        Player.PlayerAbility.StartCastingState(abilities[name]);
    }
}

public enum UnitActionState {
    Idle,
    Move,
    Attack,
    Cast,
}

// Ordered counterclock-wise
public enum UnitRenderDirection {
    Right,
    UpRight,
    Up,
    UpLeft,
    Left,
    DownLeft,
    Down,
    DownRight
}
