using System.Collections.Generic;
using Godot;

public partial class Unit : CharacterBody3D {

    bool isKilled = false;
    Area3D interactionArea = null;
    UnitMutableAttributes unitAtributes;
    UnitRender unitRender;
    UnitWSBind unitWSBind;
    readonly Dictionary<string, AbilityCaster> abilities = new();
    protected UnitPlayerBind unitPlayerBind;
    protected Label3D overheadLabel;
    public PlayerBase Player { get => unitPlayerBind.Player; }
    public Area3D InteractionArea { get => interactionArea; }
    public UnitMutableAttributes UnitAttributes { get => unitAtributes; }
    public Label3D OverheadLabel { get => overheadLabel; }
    public Dictionary<string, AbilityCaster> Abilities { get => abilities; }
    public bool IsKilled { get => isKilled; }
    public UnitRender UnitRender { get => unitRender; }
    public UnitWSBind UnitWSBind { get => unitWSBind; }

    public AttributesExport GetAttributes() => UnitAttributes.GetAttributes();

    public override void _Ready() {
        base._Ready();
        unitPlayerBind = new(this);
        unitWSBind = new(this, GetNode<Server>(StaticNodePaths.Server));
        unitAtributes = new(this, unitWSBind);
        unitRender = new(this);
        overheadLabel = GetNode<Label3D>(StaticNodePaths.OverheadLabel);
        interactionArea = GetNodeOrNull<Area3D>(StaticNodePaths.InteractionArea);
        unitAtributes.OnKilled -= OnKilledHandler;
        unitAtributes.OnKilled += OnKilledHandler;
    }

    void OnKilledHandler(Unit unit) {
        Callable.From(() => { unitRender.BodyCollision.Disabled = true; }).CallDeferred();
        CollisionLayer = (uint)CollisionMasks.Corpse;
        isKilled = true;
    }

    protected void MoveAndCollide(Vector3 direction, float delta) {
        // Apply velocity.
        direction = direction.Normalized() * GetAttributes().MovementSpeed;
        MoveAndCollide(direction * (float)delta);
    }

    protected void MoveAndSlide(Vector3 direction) {
        // Apply velocity.
        direction = direction.Normalized() * GetAttributes().MovementSpeed;
        Velocity = direction;
        MoveAndSlide();
    }

    public void MoveTowards(Vector3 targetPosition) {
        MoveAndSlide(GlobalPosition.FullDirectionTo(targetPosition));
    }

    public void AddAbility(AbilityDTO abilityDTO) {
        AbilityCaster abilityCaster = new(this, abilityDTO);
        abilities.Add(abilityDTO.name, abilityCaster);
    }

    public void CastAbility(string name) {
        if (Player.PlayerAbility is null) {
            GD.PrintErr("[CastAbility] Cannot cast " + name + " because Unit's ability module could not be found");
            return;
        }
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
