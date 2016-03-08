using UnityEngine;
using UnityEditor;
using NUnit.Framework;

public class CreateBiomeSet
{
    [MenuItem("Assets/Create/Biome Set")]
    public static BiomeSet Create()
    {
        BiomeSet asset = ScriptableObject.CreateInstance<BiomeSet>();

        AssetDatabase.CreateAsset(asset, "Assets/NewBiomeSet.asset");
        AssetDatabase.SaveAssets();
        return asset;
    }
}
