using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using Object = UnityEngine.Object;

public static class GenerateUIReferences {

    private static string UIWindowPath = Application.dataPath + "/BundleResources/Prefabs/UI/Window/";
    private static string UIItemPath = Application.dataPath + "/BundleResources/Prefabs/UI/Item/";
    private static string ReferencesClassPath = Application.dataPath + "/Scripts/Framework/LiteUI/";

    [MenuItem("GearFramework/生成UI引用", false, 200)]
    private static void GenerateReferencesFile() {


        var settings = AddressableAssetSettingsDefaultObject.Settings;
        var group = settings.DefaultGroup;
        var guids = AssetDatabase.FindAssets("", new[] { "Assets/Bundles" });
        var entriesAdded = new List<AddressableAssetEntry>();

        var objs = Selection.objects;
        for (int i = 0; i < guids.Length; i++) {
            var entry = settings.CreateOrMoveEntry(guids[i], group, readOnly: false, postEvent: false);
            entry.address = AssetDatabase.GUIDToAssetPath(guids[i]).Replace("Assets/Bundles/", "");
            entry.labels.Add("MyLabel");

            entriesAdded.Add(entry);
        }


        settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, entriesAdded, true);
        //try {
        //    var itemPaths = GetConfigPaths(UIItemPath);
        //    var windowPaths = GetConfigPaths(UIWindowPath);
        //    var itemIdStr = GenerateItemReferences(itemPaths);
        //    var windowIdStr = GenerateWindowReferences(windowPaths);
        //    var uIReferencesStr = GenerateUIReferencesFile(itemPaths, windowPaths);

        //    File.WriteAllText($"{ReferencesClassPath}UIReferences.cs", itemIdStr + windowIdStr + uIReferencesStr);
        //    AssetDatabase.Refresh();
        //    Debug.Log("UI引用生成完成!");
        //}
        //catch (Exception e) {
        //    Debug.LogError(e);
        //    throw;
        //}
    }

    private static string GenerateItemReferences(List<string> itemPaths) {
        var file = new StringBuilder();
        file.AppendLine("using System.Collections.Generic;");
        file.AppendLine("");
        file.AppendLine("public enum ItemID {");
        file.AppendLine("    Invaild = 0,");
        for (var i = 0; i < itemPaths.Count; i++) {
            file.AppendLine($"    {itemPaths[i].Replace(UIItemPath, "").Replace("/", "_").Replace(".prefab", "")},");
        }
        file.AppendLine("}");
        file.AppendLine("");

        return file.ToString();
    }

    private static string GenerateWindowReferences(List<string> windowPaths) {
        var file = new StringBuilder();
        file.AppendLine("public enum WindowID {");
        file.AppendLine("    Invaild = 0,");
        for (var i = 0; i < windowPaths.Count; i++) {
            file.AppendLine($"    {windowPaths[i].Replace(UIWindowPath, "").Replace("/", "_").Replace(".prefab", "")},");
        }
        file.AppendLine("}");
        file.AppendLine("");

        return file.ToString();
    }

    private static string GenerateUIReferencesFile(List<string> itemPaths, List<string> windowPaths) {
        var path = Application.dataPath + "/BundleResources/";
        var file = new StringBuilder();
        file.AppendLine("public class UIResource {");
        file.AppendLine("    public static Dictionary<WindowID, string> WindowPath = new Dictionary<WindowID, string> {");
        for (var i = 0; i < windowPaths.Count; i++) {
            var key = windowPaths[i].Replace(UIWindowPath, "").Replace("/", "_").Replace(".prefab", "");
            var value = windowPaths[i].Replace(path, "");
            file.AppendLine($"        {{WindowID.{key}, \"{value}\" }},");
        }
        file.AppendLine("    };");
        file.AppendLine("");
        file.AppendLine("    public static Dictionary<ItemID, string> ItemPath = new Dictionary<ItemID, string> {");
        for (var i = 0; i < itemPaths.Count; i++) {
            var key = itemPaths[i].Replace(UIItemPath, "").Replace("/", "_").Replace(".prefab", "");
            var value = itemPaths[i].Replace(path, "");
            file.AppendLine($"        {{ItemID.{key}, \"{value}\" }},");
        }
        file.AppendLine("    };");
        file.AppendLine("");
        file.AppendLine("    public static string GetWindowPath(WindowID id) {");
        file.AppendLine("        return WindowPath[id];");
        file.AppendLine("    }");
        file.AppendLine("");
        file.AppendLine("    public static string GetItemPath(ItemID id) {");
        file.AppendLine("        return ItemPath[id];");
        file.AppendLine("    }");
        file.AppendLine("}");
        return file.ToString();
    }

    private static List<string> GetConfigPaths(string path) {
        var result = new List<string>();
        var direction = new DirectoryInfo(path);
        var files = direction.GetFiles("*", SearchOption.AllDirectories);
        for (var i = 0; i < files.Length; i++) {
            if (files[i].Name.EndsWith(".meta")) {
                continue;
            }
            result.Add(files[i].FullName.Replace("\\", "/"));
        }
        return result;
    }
}

public static class AddressableEditorExtension {
    /// <summary>
    /// Set Addressables Key/ID of an gameObject.
    /// </summary>
    /// <param name="gameObject">GameObject to set Key/ID</param>
    /// <param name="id">Key/ID</param>
    public static void SetAddressableID(this GameObject gameObject, string id) {
        SetAddressableID(gameObject as Object, id);
    }

    /// <summary>
    /// Set Addressables Key/ID of an object.
    /// </summary>
    /// <param name="o">Object to set Key/ID</param>
    /// <param name="id">Key/ID</param>
    public static void SetAddressableID(this Object o, string id) {
        if (id.Length == 0) {
            Debug.LogWarning($"Can not set an empty adressables ID.");
        }
        AddressableAssetEntry entry = GetAddressableAssetEntry(o);
        if (entry != null) {
            entry.address = id;
        }
    }

    /// <summary>
    /// Get Addressables Key/ID of an gameObject.
    /// </summary>
    /// <param name="gameObject">gameObject to recive addressables Key/ID</param>
    /// <returns>Addressables Key/ID</returns>
    public static string GetAddressableID(this GameObject gameObject) {
        return GetAddressableID(gameObject as Object);
    }

    /// <summary>
    /// Get Addressables Key/ID of an object.
    /// </summary>
    /// <param name="o">object to recive addressables Key/ID</param>
    /// <returns>Addressables Key/ID</returns>
    public static string GetAddressableID(this Object o) {
        AddressableAssetEntry entry = GetAddressableAssetEntry(o);
        if (entry != null) {
            return entry.address;
        }
        return "";
    }

    /// <summary>
    /// Get addressable asset entry of an object.
    /// </summary>
    /// <param name="o">>object to recive addressable asset entry</param>
    /// <returns>addressable asset entry</returns>
    public static AddressableAssetEntry GetAddressableAssetEntry(Object o) {
        AddressableAssetSettings aaSettings = AddressableAssetSettingsDefaultObject.Settings;

        AddressableAssetEntry entry = null;
        string guid = string.Empty;
        long localID = 0;
        string path;

        bool foundAsset = AssetDatabase.TryGetGUIDAndLocalFileIdentifier(o, out guid, out localID);
        path = AssetDatabase.GUIDToAssetPath(guid);

        if (foundAsset && (path.ToLower().Contains("assets"))) {
            if (aaSettings != null) {
                entry = aaSettings.FindAssetEntry(guid);
            }
        }

        if (entry != null) {
            return entry;
        }

        return null;
    }
}