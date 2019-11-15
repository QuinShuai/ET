using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;
using Object = System.Object;

public class UpdateAddressableFlag {

    [MenuItem("Tools/添加Addressable标记", false, 200)]
    private static void AddAddressableFlag() {
        var entriesAdded = new List<AddressableAssetEntry>();
        var settings = AddressableAssetSettingsDefaultObject.Settings;

        var group = settings.DefaultGroup;
        var ui = settings.FindGroup("UI");
        var configs = settings.FindGroup("Configs");
        var guids = AssetDatabase.FindAssets("", new[] { "Assets/Bundles" });

        for (int i = 0; i < guids.Length; i++) {
            AddressableAssetEntry entry;
            var path = AssetDatabase.GUIDToAssetPath(guids[i]);
            if (!path.Contains(".")) {
                continue;
            }

            if (path.Contains("/UI/")) {
                entry = settings.CreateOrMoveEntry(guids[i], ui, readOnly: false, postEvent: false);
            }
            else if (path.Contains("/Configs/")) {
                entry = settings.CreateOrMoveEntry(guids[i], configs, readOnly: false, postEvent: false);
                entry.labels.Add("Config");
            }
            else {
                entry = settings.CreateOrMoveEntry(guids[i], group, readOnly: false, postEvent: false);
            }

            entry.address = path.Replace("Assets/Bundles/", "");
            entriesAdded.Add(entry);
        }
        settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, entriesAdded, true);
        Debug.Log("标记添加完成");
    }
}

public static class AddressableEditorExtension {
    /// <summary>
    /// Set Addressables Key/ID of an gameObject.
    /// </summary>
    /// <param name="gameObject">GameObject to set Key/ID</param>
    /// <param name="id">Key/ID</param>
    public static void SetAddressableID(this GameObject gameObject, string id) {
        SetAddressableID(gameObject as UnityEngine.Object, id);
    }

    /// <summary>
    /// Set Addressables Key/ID of an object.
    /// </summary>
    /// <param name="o">Object to set Key/ID</param>
    /// <param name="id">Key/ID</param>
    public static void SetAddressableID(this UnityEngine.Object o, string id) {
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
        return GetAddressableID(gameObject as UnityEngine.Object);
    }

    /// <summary>
    /// Get Addressables Key/ID of an object.
    /// </summary>
    /// <param name="o">object to recive addressables Key/ID</param>
    /// <returns>Addressables Key/ID</returns>
    public static string GetAddressableID(this UnityEngine.Object o) {
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
    public static AddressableAssetEntry GetAddressableAssetEntry(UnityEngine.Object o) {
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