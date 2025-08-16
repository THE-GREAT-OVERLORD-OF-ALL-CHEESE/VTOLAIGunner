using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CheeseMods.AIHelicopterGunner.AIHelpers;

public class MissileHelper
{
    public class MissilesPerTarget
    {
        public Actor target;
        public List<Missile> missiles;

        public MissilesPerTarget(Actor target, Missile missile)
        {
            this.target = target;
            missiles = new List<Missile> { missile };
            Debug.Log($"{missiles.Count} missiles on target {target.actorName}");
        }

        public void AddMissile(Missile missile)
        {
            missiles.Add(missile);
            Debug.Log($"{missiles.Count} missiles on target {target.actorName}");
        }

        public void UpdateMissilesPerTarget()
        {
            missiles.RemoveAll(item => item == null);
        }
    }

    private List<MissilesPerTarget> missilesPerTargets = new List<MissilesPerTarget>();

    public void ReportMissilePerTarget(Actor target, Missile missile)
    {
        MissilesPerTarget missilesPerTarget = missilesPerTargets.FirstOrDefault(t => t.target == target);
        if (missilesPerTarget != null)
        {
            Debug.Log($"New missile for existing target {target.actorName}");
            missilesPerTarget.AddMissile(missile);
            return;
        }

        missilesPerTargets.Add(new MissilesPerTarget(target, missile));
        Debug.Log($"New missile for new target {target.actorName}");
    }

    public void UpdateMissilePerTarget()
    {
        foreach (MissilesPerTarget mpt in missilesPerTargets)
        {
            mpt.UpdateMissilesPerTarget();
        }

        MissilesPerTarget missilesPerTarget = missilesPerTargets.FirstOrDefault(t => t.target == null || !t.target.alive || !t.missiles.Any());
        if (missilesPerTarget != null)
        {
            missilesPerTargets.Remove(missilesPerTarget);
            Debug.Log($"No more missiles for target {missilesPerTarget.target.actorName}");
        }
    }

    public int GetMissilesForTarget(Actor target)
    {
        MissilesPerTarget missilesPerTarget = missilesPerTargets.FirstOrDefault(t => t.target == target);
        if (missilesPerTarget != null)
        {
            return missilesPerTarget.missiles.Count();
        }
        return 0;
    }

    public static bool IsOpticalNotFaf(HPEquipOpticalML ml)
    {
        return ml.ml.GetNextMissile() != null
            && ml.ml.GetNextMissile()?.opticalFAF == false
            && ml.fullName.ToLower().Contains("agm-27") == false
            && ml.fullName.ToLower().Contains("tow") == false;
    }

    public static bool IsOpticalFaf(HPEquipOpticalML ml)
    {
        return ml.ml.GetNextMissile() != null
            && ml.ml.GetNextMissile().opticalFAF;
    }

    public static bool IsGuidedRocketLauncher(HPEquipOpticalML ml)
    {
        return ml.ml.GetNextMissile() != null
            && ml.ml.GetNextMissile()?.opticalFAF == false
            && ml.fullName.ToLower().Contains("agm-27");
    }

    public static bool IsTow(HPEquipOpticalML ml)
    {
        return ml.ml.GetNextMissile() != null
            && ml.ml.GetNextMissile()?.opticalFAF == false
            && ml.fullName.ToLower().Contains("tow");
    }
}
