using System;
using System.Text;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using System.Globalization;

namespace ETHotfix {
public class NewsConfigData {
    public int Id; // ID
    public string Photo; // 图片
    public string Poptitle; // 弹出标题
    public string Title; // 标题
    public string Intro; // 简介
    public string Text; // 正文
    public int Talkid; // 对应关系
}

public class NewsConfig : IConfig {
    private readonly Dictionary<int, NewsConfigData> _datas;
    public List<NewsConfigData> Datas => _datas.Values.ToList();

    public string ConfigFileName { get; }

    public NewsConfig() {
        _datas = new Dictionary<int, NewsConfigData>();
        ConfigFileName = "NewsConfig";
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
            var data = new NewsConfigData();
            int.TryParse(tables[i, 0], out data.Id);
            data.Photo = tables[i, 1];
            data.Poptitle = tables[i, 2];
            data.Title = tables[i, 3];
            data.Intro = tables[i, 4];
            data.Text = tables[i, 5];
            int.TryParse(tables[i, 6], out data.Talkid);
            if(_datas.ContainsKey(data.Id)) {
                throw new Exception(data.Id + "(字典中已存在具有相同Key的元素)");
            }
            else {
                _datas.Add(data.Id, data);
            }
        }
    }

    public NewsConfigData GetDataAt(int Id) {
        if (_datas.ContainsKey(Id)) {
            return _datas[Id];
        }
        return null;
    }

    public bool Add(NewsConfigData data) {
        if (_datas.ContainsKey(data.Id)) {
            return false;
        }
        _datas.Add(data.Id, data);
        return true;
    }

    public bool Remove(NewsConfigData data) {
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
