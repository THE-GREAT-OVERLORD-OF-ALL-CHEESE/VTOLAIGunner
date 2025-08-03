using CheeseMods.AIHelicopterGunner.AIHelpers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CheeseMods.AIHelicopterGunner.Spotting
{
    public class GunnerTargetSpotter
    {
        public Transform tf;
        public IEnumerable<TargetMetaData> visibleTargets;
        public MissileHelper missileHelper;
        public TargetMetaDataManager targetMetadataManager;

        public GunnerTargetSpotter(TargetMetaDataManager metaDataManager, Transform tf, MissileHelper helper)
        {
            targetMetadataManager = metaDataManager;
            this.tf = tf;
            missileHelper = helper;
        }

        public void UpdateVisibleTargets()
        {
            visibleTargets = TargetManager.instance.enemyUnits
            .Where(u => u.alive
                    && DetermineVisible(u))
            .Select(u => targetMetadataManager.GetMetaData(u));
        }

        public bool DetermineVisible(Actor actor)
        {
            Vector3 offset = actor.position - tf.position;
            float angularSize = Mathf.Atan2(actor.physicalRadius, offset.magnitude) * Mathf.Rad2Deg * 2f;

            return (angularSize > GunnerAIConfig.minimumTargetSizeAngular
                || (UnitIconManager.showIcons && UnitIconManager.instance.GetIconVisibility(offset.magnitude) > 0.1f))
                && !Physics.Raycast(tf.position,
                    offset,
                    out RaycastHit hit,
                    offset.magnitude - 5f,
            1,
            QueryTriggerInteraction.Ignore)
                && !TargetManager.IsOccludedByClouds(tf.position, actor.position, 2.2f);
        }
    }
}
