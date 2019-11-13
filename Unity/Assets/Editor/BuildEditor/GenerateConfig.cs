using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel;
using LitJson;
using UnityEditor;
using UnityEngine;

public static class GenerateConfig {

    private static string ExcelFilePath = Application.dataPath + "/_Design/Config/";
    private static string ModelPath = Application.dataPath + "/Hotfix/Base/Config/";
    private static string ConfigFilePath = Application.dataPath + "/Bundles/Configs/";
    private static string ConfigClassPath = Application.dataPath + "/Hotfix/Module/Config/";
    
    [MenuItem("Tools/清除Progress", false, 101)]
    private static void ClearProgress() {
        EditorUtility.ClearProgressBar();
    }
    
    [MenuItem("Tools/生成配置文件", false, 100)]
    private static void GenerateConfigFile() {
        try {
            var allNames = new List<string>();
            var paths = GetConfigPaths(ExcelFilePath);
            var curProgress = 1.0f;
            var totalProgress = paths.Count;
            for (var i = 0; i < totalProgress; i++) {
                var progress = curProgress / totalProgress;
                Progress($"正在生成配置文件", $"生成进度: {curProgress}/{totalProgress}", progress);


                var path = paths[i];
                using (var stream = File.Open(path, FileMode.Open, FileAccess.Read)) {
                    using (var reader = ExcelReaderFactory.CreateOpenXmlReader(stream)) {
                        Debug.Log(reader.AsDataSet().Tables.Count);
                        Debug.Log(reader.AsDataSet().Tables[0].CreateDataReader().FieldCount);
                        var tabs = reader.AsDataSet().Tables;
                        for (var j = 0; j < tabs.Count; j++) {
                            var table = tabs[j].CreateDataReader();
                            var name = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(tabs[j].TableName);
                            var fields = GenerateClassFields(table);
                            GenerateClassFile(fields, name);

                            var bytes = TableToBytes(tabs[j]);
                            File.WriteAllText($"{ConfigFilePath}/{name}Config.txt", bytes.ToJson());
                            allNames.Add($"{name}Config");
                        }
                    }
                }
                curProgress++;
            }
            AssetDatabase.Refresh();
            GenerateConfigDataFile(allNames);
            Debug.Log("配置表生成完成!");
            EditorUtility.ClearProgressBar();
        }
        catch (Exception e) {
            Debug.LogError(e);
            EditorUtility.ClearProgressBar();
            throw;
        }
    }

    private static JsonData TableToBytes(DataTable dt) {
        var jsonRoot = new JsonData();
        for (var i = 0; i < dt.Rows.Count; i++) {
            var jsonNode = new JsonData();
            for (var j = 0; j < dt.Rows[i].ItemArray.Length; j++) {
                jsonNode.Add(dt.Rows[i].ItemArray[j].ToString());
            }

            if (string.IsNullOrEmpty(dt.Rows[i].ItemArray[0].ToString())) {
                continue;
            }
            jsonRoot.Add(jsonNode);
        }
        return jsonRoot;
    }

    private static List<string[]> GenerateClassFields(DataTableReader reader) {
        var fields = new List<string[]>();
        reader.Read();
        for (var j = 0; j < reader.FieldCount; j++) {
            var str = new string[3];
            str[0] = reader.GetValue(j)?.ToString().ToLower();
            fields.Add(str);
        }
        reader.Read();
        for (var j = 0; j < reader.FieldCount; j++) {
            var str = fields[j];
            str[2] = reader.GetValue(j)?.ToString();
        }
        reader.Read();
        for (var j = 0; j < reader.FieldCount; j++) {
            var str = fields[j];
            var t = reader.GetValue(j)?.ToString();
            if (t != null) {
                str[1] = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(t);
            }
            else {
                str[1] = t;
            }
        }
        return fields;
    }

    private static void GenerateConfigDataFile(List<string> configs) {
        var file = new StringBuilder();
        file.AppendLine("using System.Collections.Generic;");
        //file.AppendLine("using GF.Unity;");
        file.AppendLine("using System.Linq;");
        file.AppendLine("");
        file.AppendLine("namespace ETHotfix {");
        file.AppendLine($"public class ConfigData " + "{");
        file.AppendLine("");
        for (var i = 0; i < configs.Count; i++) {
            file.AppendLine($"    public {configs[i]} {configs[i]} " + "{ get; }");
        }
        file.AppendLine("");
        file.AppendLine($"    private Dictionary<string, IConfig> _configs;");
        file.AppendLine($"    public List<IConfig> Configs => _configs.Values.ToList();");
        file.AppendLine("");
        file.AppendLine($"    public ConfigData() " + "{");
        file.AppendLine($"        _configs = new Dictionary<string, IConfig>();");
        for (var i = 0; i < configs.Count; i++) {
            file.AppendLine("");
            file.AppendLine($"        {configs[i]} = new {configs[i]}();");
            file.AppendLine($"        _configs.Add({configs[i]}.ConfigFileName.ToLower(), {configs[i]});");
        }
        file.AppendLine("    }");
        file.AppendLine("");
        file.AppendLine($"    public bool Parse(byte[] bytes, string configFileName) " + "{");
        file.AppendLine($"        if (_configs.ContainsKey(configFileName)) " + "{");
        file.AppendLine($"            var iConfig = _configs[configFileName];");
        file.AppendLine($"            iConfig.Deserialize(bytes);");
        file.AppendLine($"            return true;");
        file.AppendLine("        }");
        file.AppendLine($"        return false;");
        file.AppendLine("    }");
        file.AppendLine("}");
        file.AppendLine("}");
        File.WriteAllText($"{ModelPath}/ConfigData.cs", file.ToString());
        AssetDatabase.Refresh();
    }

    private static void GenerateClassFile(List<string[]> fields, string name) {
        var file = new StringBuilder();
        file.AppendLine("using System;");
        file.AppendLine("using System.Text;");
        file.AppendLine("using System.Linq;");
        file.AppendLine("using UnityEngine;");
        //file.AppendLine("using GF.Unity;");
        file.AppendLine("using System.Collections.Generic;");
        file.AppendLine("using System.Globalization;");
        file.AppendLine("");
        file.AppendLine("namespace ETHotfix {");
        file.AppendLine($"public class {name}ConfigData " + "{");
        for (var j = 0; j < fields.Count; j++) {
            var str = fields[j];
            if (string.IsNullOrEmpty(str[0])) {
                continue;
            }
            file.AppendLine($"    public {str[0]} {str[1]}; // {str[2]}");
        }
        file.AppendLine("}");
        file.AppendLine("");
        file.AppendLine($"public class {name}Config " + ": IConfig {");
        file.AppendLine($"    private readonly Dictionary<{fields[0][0]}, {name}ConfigData> _datas;");
        file.AppendLine($"    public List<{name}ConfigData> Datas => _datas.Values.ToList();");
        file.AppendLine("");
        file.AppendLine("    public string ConfigFileName { get; }");
        file.AppendLine("");
        file.AppendLine($"    public {name}Config() " + "{");
        file.AppendLine($"        _datas = new Dictionary<{fields[0][0]}, {name}ConfigData>();");
        file.AppendLine($"        ConfigFileName = \"{name}Config\";");
        file.AppendLine("    }");
        file.AppendLine("");
        file.AppendLine($"    public void Deserialize(byte[] bytes) " + "{");
        file.AppendLine($"        _datas.Clear();");
        file.AppendLine($"        var memoryDataPackage = new MemoryDataPackage(true);");
        file.AppendLine($"        memoryDataPackage.memoryStream.Write(bytes, 0, bytes.Length);");
        file.AppendLine($"        memoryDataPackage.Position = 0;");
        file.AppendLine($"        var rowsCount = memoryDataPackage.ReadInt();");
        file.AppendLine($"        var columnsCount = memoryDataPackage.ReadInt();");
        file.AppendLine($"        var tables = new string[rowsCount,columnsCount];");
        file.AppendLine($"        for (var i = 0; i < rowsCount; i++) " + "{");
        file.AppendLine($"            for(var index =0;index < columnsCount; index++) " + "{");
        file.AppendLine($"                tables[i,index] = memoryDataPackage.ReadString();");
        file.AppendLine($"            " + "}");
        file.AppendLine($"        " + "}");
        file.AppendLine($"        for (var i = 3; i < rowsCount; i++) " + "{");
        file.AppendLine($"            var data = new {name}ConfigData();");
        for (var i = 0; i < fields.Count; i++) {
            var str = fields[i];
            if (string.IsNullOrEmpty(str[0])) {
                continue;
            }
            if (str[0] == "string") {
                file.AppendLine($"            data.{str[1]} = tables[i, {i}];");
            }
            else if (str[0] == "float" || str[0] == "double") {
                file.AppendLine($"            {str[0]}.TryParse(tables[i, {i}], NumberStyles.Any, CultureInfo.InvariantCulture, out data.{str[1]});");
            }
            else if (str[0].Contains("List")) {
                file.AppendLine(GetListStr(str, i));
            }
            else {
                file.AppendLine($"            {str[0]}.TryParse(tables[i, {i}], out data.{str[1]});");
            }
        }
        file.AppendLine($"            if(_datas.ContainsKey(data.{fields[0][1]})) " + "{");
        file.AppendLine($"                throw new Exception(data.{fields[0][1]} + \"(字典中已存在具有相同Key的元素)\");");
        file.AppendLine("            }");
        file.AppendLine("            else {");
        file.AppendLine($"                _datas.Add(data.{fields[0][1]}, data);");
        file.AppendLine("            }");
        file.AppendLine("        }");
        file.AppendLine("    }");
        file.AppendLine("");
        file.AppendLine($"    public {name}ConfigData GetDataAt({fields[0][0]} {fields[0][1]}) " + "{");
        file.AppendLine($"        if (_datas.ContainsKey({fields[0][1]})) " + "{");
        file.AppendLine($"            return _datas[{fields[0][1]}];");
        file.AppendLine("        }");
        file.AppendLine($"        return null;");
        file.AppendLine("    }");
        file.AppendLine("");
        file.AppendLine($"    public bool Add({name}ConfigData data) " + "{");
        file.AppendLine($"        if (_datas.ContainsKey(data.{fields[0][1]})) " + "{");
        file.AppendLine($"            return false;");
        file.AppendLine("        }");
        file.AppendLine($"        _datas.Add(data.{fields[0][1]}, data);");
        file.AppendLine($"        return true;");
        file.AppendLine("    }");
        file.AppendLine("");
        file.AppendLine($"    public bool Remove({name}ConfigData data) " + "{");
        file.AppendLine($"        if (!_datas.ContainsKey(data.{fields[0][1]})) " + "{");
        file.AppendLine($"            return false;");
        file.AppendLine("        }");
        file.AppendLine($"        _datas.Remove(data.{fields[0][1]});");
        file.AppendLine($"        return true;");
        file.AppendLine("    }");
        file.AppendLine("");
        file.AppendLine($"    public bool RemoveAt({fields[0][0]} {fields[0][1]}) " + "{");
        file.AppendLine($"        if (_datas.ContainsKey({fields[0][1]})) " + "{");
        file.AppendLine($"            _datas.Remove({fields[0][1]});");
        file.AppendLine($"            return true;");
        file.AppendLine("        }");
        file.AppendLine($"        return false;");
        file.AppendLine("    }");
        file.AppendLine("}");
        file.AppendLine("}");
        File.WriteAllText($"{ConfigClassPath}/{name}Config.cs", file.ToString());
        //AssetDatabase.Refresh();
    }

    private static string GetListStr(string[] str, int i) {
        var file = new StringBuilder();
        var listType = str[0].Replace("List<", "").Replace(">", "");
        file.AppendLine($"            tables[i, {i}] = tables[i,{i}].Replace(\" \", string.Empty);");
        file.Append($"            data.{str[1]} = tables[i, {i}] == string.Empty ? new {str[0]}() : ");
        if (listType == "string") {
            file.Append($"tables[i,{i}].Split('|').ToList();");
        }
        else if (listType == "float" || listType == "double") {
            file.Append($"new {str[0]}(Array.ConvertAll(tables[i,{i}].Split('|'), t => ");
            file.Append($"{listType}.Parse(t, NumberStyles.Any, CultureInfo.InvariantCulture)));");
        }
        else {
            file.Append($"new {str[0]}(Array.ConvertAll(tables[i,{i}].Split('|'), {listType}.Parse));");
        }
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
            result.Add(files[i].FullName);
        }
        return result;
    }

    static int _lastProgress = -1;
    static string _lastTitle = "";
    static string _lastMessage = "";
    static void Progress(string title, string message, float progress) {
        var percent = Mathf.RoundToInt(progress * 100.0f);
        if (percent != _lastProgress || _lastTitle != title || _lastMessage != message) {
            _lastTitle = title;
            _lastMessage = message;
            _lastProgress = percent;

            if (EditorUtility.DisplayCancelableProgressBar(title, message, progress)) {
                EditorUtility.ClearProgressBar();
            }
        }
    }
}
