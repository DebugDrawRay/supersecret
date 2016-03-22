using UnityEngine;
using UnityEditor;
using NUnit.Framework;

public class CreatePlayerAttachment
{
    [MenuItem("Assets/Create/Player Attachment")]
    public static PlayerAttachment Create()
    {
        PlayerAttachment asset = ScriptableObject.CreateInstance<PlayerAttachment>();

        AssetDatabase.CreateAsset(asset, "Assets/NewPlayerAttachment.asset");
        AssetDatabase.SaveAssets();
        return asset;
    }
}
