using System;
using System.Text;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using System.Globalization;

namespace ETHotfix {
public class TipsConfigData {
    public int Id; // tips ID
    public string Desc; // 事件描述
}

public class TipsConfig : IConfig {
    private readonly Dictionary<int, TipsConfigData> _datas;
    public List<TipsConfigData> Datas => _datas.Values.ToList();

    public string ConfigFileName { get; }

    public TipsConfig() {
        _datas = new Dictionary<int, TipsConfigData>();
        ConfigFileName = "TipsConfig";
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
            var data = new TipsConfigData();
            int.TryParse(tables[i, 0], out data.Id);
            data.Desc = tables[i, 1];
            if(_datas.ContainsKey(data.Id)) {
                throw new Exception(data.Id + "(字典中已存在具有相同Key的元素)");
            }
            else {
                _datas.Add(data.Id, data);
            }
        }
    }

    public TipsConfigData GetDataAt(int Id) {
        if (_datas.ContainsKey(Id)) {
            return _datas[Id];
        }
        return null;
    }

    public bool Add(TipsConfigData data) {
        if (_datas.ContainsKey(data.Id)) {
            return false;
        }
        _datas.Add(data.Id, data);
        return true;
    }

    public bool Remove(TipsConfigData data) {
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
