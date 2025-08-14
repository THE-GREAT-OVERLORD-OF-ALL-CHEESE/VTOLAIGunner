using CheeseMods.AIHelicopterGunner.Character;
using CheeseMods.AIHelicopterGunnerAssets.ScriptableObjects;
using ModLoader.Framework;
using ModLoader.Framework.Attributes;
using System.Drawing.Drawing2D;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace CheeseMods.AIHelicopterGunner;

[ItemId("cheese.aihelicoptergunner")] // Harmony ID for your mod, make sure this is unique
public class Main : VtolMod
{
    private void Awake()
    {
        Debug.Log($"{modName}: Ready!");
        Debug.Log($"{aiGunnerName} here, I am combat ready!");

        string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        path = Path.Combine(path,"gunnerassets.ab");
        AssetLoader.LoadAssetBundle(new FileInfo(path));
    }

    public override void UnLoad()
    {
        // Destroy any objects
        AssetLoader.UnloadAssets();

        Debug.Log($"{modName}: Assets unloaded.");
        Debug.Log($"{aiGunnerName} here, come again!");
    }

    public const string modName = "AIHelicopterGunner";
    public const string aiGunnerName = "Marisa";
}