using CheeseMods.AIHelicopterGunner.Components;
using HarmonyLib;
using UnityEngine;

namespace CheeseMods.AIHelicopterGunner.Patches;


[HarmonyPatch(typeof(Actor), "Awake")]
class Patch_Actor_Awake
{
    [HarmonyPostfix]
    static void Postfix(Actor __instance)
    {
        if (__instance.isPlayer || __instance.isHumanPlayer)
        {
            Debug.Log("Adding AI Gunner to player aircraft");
            __instance.gameObject.AddComponent<AIGunner>();
        }
    }
}
