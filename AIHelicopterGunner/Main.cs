﻿using ModLoader.Framework;
using ModLoader.Framework.Attributes;
using UnityEngine;

namespace AIHelicopterGunner
{
    [ItemId("cheese.aihelicoptergunner")] // Harmony ID for your mod, make sure this is unique
    public class Main : VtolMod
    {
        private void Awake()
        {
            Debug.Log($"{modName}: Ready to gun!");
            Debug.Log($"{aiGunnerName} here, I am combat ready!");
        }

        public override void UnLoad()
        {
            // Destroy any objects
        }

        public const string modName = "AIHelicopterGunner";
        public const string aiGunnerName = "Aurora";
    }
}