using System;
using UnityEngine;

namespace AIHelicopterGunner.Character
{
    public class Callout
    {
        private float shortCoolDown;
        private float longCoolDown;

        private int maxShortMessages;

        private float bucket;
        private float lastSpokenTime;

        private Action sayCallout;

        public Callout(float shortCoolDown, float longCoolDown, int maxShortMessages, Action sayCallout)
        {
            this.shortCoolDown = shortCoolDown;
            this.longCoolDown = longCoolDown;
            this.maxShortMessages = maxShortMessages;

            this.sayCallout = sayCallout;

            lastSpokenTime = Time.time;
        }

        public void SayCallout()
        {
            float delta = Time.time - lastSpokenTime;

            Debug.Log(delta);
            if (shortCoolDown > delta)
            {
                return;
            }

            bucket -= delta * (1f / longCoolDown);
            bucket = Mathf.Max(bucket, 0);
            lastSpokenTime = Time.time;

            Debug.Log(bucket);
            if (bucket > maxShortMessages)
            {
                return;
            }

            bucket += 1f;
            sayCallout();
        }
    }
}
