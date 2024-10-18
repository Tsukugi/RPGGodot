using Godot;
namespace TurnBattle {
    public partial class Unit : AnimatedSprite2D {

        UnitDTO unitInfo = new();

        [Export]
        public Players player;
        public const string SpriteFramesPath = "res://TurnBattle/SpriteFrames/";
        public UnitDTO UnitInfo { get => unitInfo; }

        public void UpdateUnit(UnitDTO unitInfo) {
            this.unitInfo = unitInfo;

            float scale = (float)(unitInfo.attributes.scale == 0 ? 1 : unitInfo.attributes.scale);

            UpdateSprite(SpriteFramesPath + unitInfo.sprite + ".tres");
            Scale = new Vector2(scale, scale);

        }

        void UpdateSprite(string path) {
            SpriteFrames = GD.Load<SpriteFrames>(path);
        }
    }
}