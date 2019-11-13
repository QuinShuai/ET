using System;
using System.Text;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using System.Globalization;

namespace ETHotfix {
public class UnitConfigData {
    public int Unitid; // 角色ID
    public string Name; // 姓名
    public string Head; // 头像图片
    public string Fullname; // 大图名
    public string Intro; // 介绍
}

public class UnitConfig : IConfig {
    private readonly Dictionary<int, UnitConfigData> _datas;
    public List<UnitConfigData> Datas => _datas.Values.ToList();

    public string ConfigFileName { get; }

    public UnitConfig() {
        _datas = new Dictionary<int, UnitConfigData>();
        ConfigFileName = "UnitConfig";
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
            var data = new UnitConfigData();
            int.TryParse(tables[i, 0], out data.Unitid);
            data.Name = tables[i, 1];
            data.Head = tables[i, 2];
            data.Fullname = tables[i, 3];
            data.Intro = tables[i, 4];
            if(_datas.ContainsKey(data.Unitid)) {
                throw new Exception(data.Unitid + "(字典中已存在具有相同Key的元素)");
            }
            else {
                _datas.Add(data.Unitid, data);
            }
        }
    }

    public UnitConfigData GetDataAt(int Unitid) {
        if (_datas.ContainsKey(Unitid)) {
            return _datas[Unitid];
        }
        return null;
    }

    public bool Add(UnitConfigData data) {
        if (_datas.ContainsKey(data.Unitid)) {
            return false;
        }
        _datas.Add(data.Unitid, data);
        return true;
    }

    public bool Remove(UnitConfigData data) {
        if (!_datas.ContainsKey(data.Unitid)) {
            return false;
        }
        _datas.Remove(data.Unitid);
        return true;
    }

    public bool RemoveAt(int Unitid) {
        if (_datas.ContainsKey(Unitid)) {
            _datas.Remove(Unitid);
            return true;
        }
        return false;
    }
}
}
