using System;
using System.Text;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using System.Globalization;

namespace ETHotfix {
public class AnimationConfigData {
    public int Id; // 编号
    public int Animshowtype; // 1=出现，2=消失
    public int Animtype; // 类型
    public int Animparam0; // 参数0
    public int Animparam1; // 参数1
    public int Animparam2; // 参数2
    public int Animparam3; // 参数3
    public int Animparam4; // 参数4
    public int Animparam5; // 参数5
}

public class AnimationConfig : IConfig {
    private readonly Dictionary<int, AnimationConfigData> _datas;
    public List<AnimationConfigData> Datas => _datas.Values.ToList();

    public string ConfigFileName { get; }

    public AnimationConfig() {
        _datas = new Dictionary<int, AnimationConfigData>();
        ConfigFileName = "AnimationConfig";
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
            var data = new AnimationConfigData();
            int.TryParse(tables[i, 0], out data.Id);
            int.TryParse(tables[i, 1], out data.Animshowtype);
            int.TryParse(tables[i, 2], out data.Animtype);
            int.TryParse(tables[i, 3], out data.Animparam0);
            int.TryParse(tables[i, 4], out data.Animparam1);
            int.TryParse(tables[i, 5], out data.Animparam2);
            int.TryParse(tables[i, 6], out data.Animparam3);
            int.TryParse(tables[i, 7], out data.Animparam4);
            int.TryParse(tables[i, 8], out data.Animparam5);
            if(_datas.ContainsKey(data.Id)) {
                throw new Exception(data.Id + "(字典中已存在具有相同Key的元素)");
            }
            else {
                _datas.Add(data.Id, data);
            }
        }
    }

    public AnimationConfigData GetDataAt(int Id) {
        if (_datas.ContainsKey(Id)) {
            return _datas[Id];
        }
        return null;
    }

    public bool Add(AnimationConfigData data) {
        if (_datas.ContainsKey(data.Id)) {
            return false;
        }
        _datas.Add(data.Id, data);
        return true;
    }

    public bool Remove(AnimationConfigData data) {
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
