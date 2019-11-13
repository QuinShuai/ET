using System;
using System.Text;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using System.Globalization;

namespace ETHotfix {
public class DateConfigData {
    public int Id; // ID
    public string Date; // 日期
    public string Week; // 星期
    public string Text; // 文本
    public int Sectionid; // 对应关系
}

public class DateConfig : IConfig {
    private readonly Dictionary<int, DateConfigData> _datas;
    public List<DateConfigData> Datas => _datas.Values.ToList();

    public string ConfigFileName { get; }

    public DateConfig() {
        _datas = new Dictionary<int, DateConfigData>();
        ConfigFileName = "DateConfig";
    }

    public void Deserialize(byte[] bytes) {
        _datas.Clear();
        var memoryDataPackage = new MemoryDataPackage(true);
        memoryDataPackage.memoryStream.Write(bytes, 0, bytes.Length);
        memoryDataPackage.Position = 0;
        var rowsCount = memoryDataPackage.ReadInt();
        var columnsCount = memoryDataPackage.ReadInt();
        var tables = new string[rowsCount,columnsCount];
        for (var i = 0; i < rowsCount; i++) {
            for(var index =0;index < columnsCount; index++) {
                tables[i,index] = memoryDataPackage.ReadString();
            }
        }
        for (var i = 3; i < rowsCount; i++) {
            var data = new DateConfigData();
            int.TryParse(tables[i, 0], out data.Id);
            data.Date = tables[i, 1];
            data.Week = tables[i, 2];
            data.Text = tables[i, 3];
            int.TryParse(tables[i, 4], out data.Sectionid);
            if(_datas.ContainsKey(data.Id)) {
                throw new Exception(data.Id + "(字典中已存在具有相同Key的元素)");
            }
            else {
                _datas.Add(data.Id, data);
            }
        }
    }

    public DateConfigData GetDataAt(int Id) {
        if (_datas.ContainsKey(Id)) {
            return _datas[Id];
        }
        return null;
    }

    public bool Add(DateConfigData data) {
        if (_datas.ContainsKey(data.Id)) {
            return false;
        }
        _datas.Add(data.Id, data);
        return true;
    }

    public bool Remove(DateConfigData data) {
        if (!_datas.ContainsKey(data.Id)) {
            return false;
        }
        _datas.Remove(data.Id);
        return true;
    }

    public bool RemoveAt(int Id) {
        if (_datas.ContainsKey(Id)) {
            _datas.Remove(Id);
            return true;
        }
        return false;
    }
}
}
