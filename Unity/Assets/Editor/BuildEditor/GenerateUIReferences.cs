using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

public static class GenerateUIReferences {

    private static string RootPath = Application.dataPath + "/Bundles/";
    private static string UIPanelPath = Application.dataPath + "/Bundles/UI/";
    private static string ReferencesClassPath = Application.dataPath + "/Hotfix/Module/UI/";

    [MenuItem("Tools/生成UI引用", false, 201)]
    private static void GenerateReferencesFile() {
        try {
            var paths = GetUIPaths(UIPanelPath);
            var panelStr = GenerateUITypes(paths);
            var uIReferencesStr = GenerateUIReferencesFile(paths);

            File.WriteAllText($"{ReferencesClassPath}UIReferences.cs", panelStr + uIReferencesStr);
            AssetDatabase.Refresh();
            Debug.Log("UI引用生成完成!");
        }
        catch (Exception e) {
            Debug.LogError(e);
            throw;
        }
    }

    private static string GenerateUITypes(List<string> itemPaths) {
        var file = new StringBuilder();
        file.AppendLine("using System.Collections.Generic;");
        file.AppendLine("");
        file.AppendLine("namespace ETHotfix {");
        file.AppendLine("    public enum UIType {");
        file.AppendLine("        Invaild = 0,");
        for (var i = 0; i < itemPaths.Count; i++) {
            file.AppendLine($"        {itemPaths[i].Replace(UIPanelPath, "").Replace("/", "_").Replace(".prefab", "")},");
        }
        file.AppendLine("    }");
        file.AppendLine("");

        return file.ToString();
    }

    private static string GenerateUIReferencesFile(List<string> windowPaths) {
        var file = new StringBuilder();
        file.AppendLine("    public class UIResource {");
        file.AppendLine("        public static Dictionary<UIType, string> PanelPath = new Dictionary<UIType, string> {");
        for (var i = 0; i < windowPaths.Count; i++) {
            var key = windowPaths[i].Replace(UIPanelPath, "").Replace("/", "_").Replace(".prefab", "");
            var value = windowPaths[i].Replace(RootPath, "");
            file.AppendLine($"            {{UIType.{key}, \"{value}\" }},");
        }
        file.AppendLine("        };");
        file.AppendLine("        ");
        file.AppendLine("        public static Dictionary<UIType, string> PanelStr = new Dictionary<UIType, string> {");
        for (var i = 0; i < windowPaths.Count; i++) {
            var key = windowPaths[i].Replace(UIPanelPath, "").Replace("/", "_").Replace(".prefab", "");
            file.AppendLine($"            {{UIType.{key}, \"{key}\" }},");
        }
        file.AppendLine("        };");
        file.AppendLine("        ");
        file.AppendLine("        public static string GetPanelPath(UIType uiType) {");
        file.AppendLine("            if (PanelPath.TryGetValue(uiType, out var path)) {");
        file.AppendLine("                return path;");
        file.AppendLine("            }");
        file.AppendLine("            ");
        file.AppendLine("            return string.Empty;");
        file.AppendLine("        }");
        file.AppendLine("        ");
        file.AppendLine("        public static string GetPanelStr(UIType uiType) {");
        file.AppendLine("            if (PanelStr.TryGetValue(uiType, out var str)) {");
        file.AppendLine("                return str;");
        file.AppendLine("            }");
        file.AppendLine("            ");
        file.AppendLine("            return string.Empty;");
        file.AppendLine("        }");
        file.AppendLine("    }");
        file.AppendLine("}");
        return file.ToString();
    }

    private static List<string> GetUIPaths(string path) {
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
