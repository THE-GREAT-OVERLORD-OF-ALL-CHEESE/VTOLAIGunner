using System.Linq;

namespace CheeseMods.AIHelicopterGunner.Spotting
{
    public class TargetMetaData
    {
        public Actor Actor { get; }

        public TargetTypes TargetType { get; }

        public TargetTypeSpecial SpecialFlags { get; }

        public bool Alive => Actor?.alive ?? false;

        public bool CanAttack { get; }

        public bool Hostile => spawn != null ? spawn.engageEnemies : true;

        public float MaxRange { get; } = 0f;

        public float Value { get; }

        private AIUnitSpawn spawn;

        public TargetMetaData(Actor actor)
        {
            Actor = actor;
            TargetType = ActorClassifier.CalculateTargetType(actor, out TargetTypeSpecial special);
            SpecialFlags = special;
            spawn = actor.GetComponentInParent<AIUnitSpawn>();

            CanAttack = actor.GetComponentInChildren<Gun>() != null || actor.GetComponentInChildren<MissileLauncher>() != null;

            VisualTargetFinder[] targetFinders = actor.gameObject.GetComponentsInChildren<VisualTargetFinder>();
            if (targetFinders.Length > 0)
            {
                MaxRange = targetFinders.Max(v => v.visionRadius);
            }
            Value = actor.health.maxHealth;
        }
    }
}
