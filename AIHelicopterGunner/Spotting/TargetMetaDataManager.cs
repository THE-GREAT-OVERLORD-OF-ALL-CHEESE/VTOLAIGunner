using System.Collections.Generic;

namespace CheeseMods.AIHelicopterGunner.Spotting
{
    public class TargetMetaDataManager
    {
        public Dictionary<Actor, TargetMetaData> metaDatas = new Dictionary<Actor, TargetMetaData>();

        public TargetMetaData GetMetaData(Actor actor)
        {
            if (metaDatas.TryGetValue(actor, out TargetMetaData metaData))
            {
                return metaData;
            }

            return CreateMetaData(actor);
        }

        public TargetMetaData CreateMetaData(Actor actor)
        {
            TargetMetaData metaData = new TargetMetaData(actor);
            metaDatas[actor] = metaData;
            return metaData;
        }
    }
}
