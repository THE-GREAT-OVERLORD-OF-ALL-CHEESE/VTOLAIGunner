using CheeseMods.AIHelicopterGunnerAssets.ScriptableObjects;
using System.IO;
using System.Linq;
using UnityEngine;

namespace CheeseMods.AIHelicopterGunner.Character
{
    public static class AssetLoader
    {
        public static AssetBundle assetBundle;
        public static CharacterVoice voice;

        public static void LoadAssetBundle(FileInfo file)
        {
            assetBundle = AssetBundle.LoadFromFile(file.FullName);

            Object[] bundles = assetBundle.LoadAllAssets(typeof(CharacterVoice));

            voice = bundles.First() as CharacterVoice;
        }

        public static void UnloadAssets()
        {
            if (assetBundle == null)
                return;

            assetBundle.Unload(false);
            assetBundle = null;
            voice = null;
        }
    }
}
