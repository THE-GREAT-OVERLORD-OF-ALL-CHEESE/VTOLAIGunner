using UnityEngine;

namespace AIHelicopterGunner.AIHelpers
{
    public static class RandomHelper
    {
        public static float BellCurve(float min, float max)
        {
            return Mathf.Lerp(min, max, (Random.value + Random.value) * 0.5f);
        }
    }
}
