using System;
using System.Text;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using System.Globalization;

namespace ETHotfix {
public class CgConfigData {
    public int Id; // CG编号
    public int Location; // 触发位置（对应talk表id）
    public string Cg; // CG图片
    public string Tip; // 未开启点击提示
}

public class CgConfig : IConfig {
    private readonly Dictionary<int, CgConfigData> _datas;
    public List<CgConfigData> Datas => _datas.Values.ToList();

    public string ConfigFileName { get; }

    public CgConfig() {
        _datas = new Dictionary<int, CgConfigData>();
        ConfigFileName = "CgConfig";
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
            var data = new CgConfigData();
            int.TryParse(tables[i, 0], out data.Id);
            int.TryParse(tables[i, 1], out data.Location);
            data.Cg = tables[i, 2];
            data.Tip = tables[i, 3];
            if(_datas.ContainsKey(data.Id)) {
                throw new Exception(data.Id + "(字典中已存在具有相同Key的元素)");
            }
            else {
                _datas.Add(data.Id, data);
            }
        }
    }

    public CgConfigData GetDataAt(int Id) {
        if (_datas.ContainsKey(Id)) {
            return _datas[Id];
        }
        return null;
    }

    public bool Add(CgConfigData data) {
        if (_datas.ContainsKey(data.Id)) {
            return false;
        }
        _datas.Add(data.Id, data);
        return true;
    }

    public bool Remove(CgConfigData data) {
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
