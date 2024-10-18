using System.Collections.Generic;
using Godot;

namespace TurnBattle {
    public partial class BattleMain : Node {
        readonly LocalDatabase localDatabase = new();
        public LocalDatabase LocalDatabase => localDatabase;

        public override void _Ready() {
            localDatabase.LoadData();
            Init();
        }


        void Init() {
            Callable.From(() => {
                var playerUnit = new UnitDTO() {
                    name = "Test",
                    sprite = "Bard",
                    attributes = new() {
                        maxHitPoints = 100,
                        hitPoints = 100,
                        scale = 2,
                    }
                };

                var enemyUnit = new UnitDTO() {
                    name = "EnemyTest",
                    sprite = "Bard",
                    attributes = new() {
                        maxHitPoints = 1000,
                        hitPoints = 1000,
                        scale = 5,
                    }
                };
                BUIMain bUIMain = GetNode<BUIMain>(NodePaths.UI);
                Player player = GetNode<Player>(NodePaths.Player);
                Player enemyplayer = GetNode<Player>(NodePaths.Hostile);

                List<UnitDTO> units = new() { playerUnit, playerUnit, playerUnit };
                bUIMain.StartStatusPanel(units);
                player.RegisterUnits(units);


                List<UnitDTO> enemyUnits = new() { enemyUnit };
                enemyplayer.RegisterUnits(enemyUnits);

            }).CallDeferred();
        }
    }


    public enum Players {
        Player,
        Hostile
    }
}