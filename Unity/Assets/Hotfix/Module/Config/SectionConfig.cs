using System;
using System.Text;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using System.Globalization;

namespace ETHotfix {
public class SectionConfigData {
    public int Id; // 节id
    public int Nextid; // 下一节id
    public int Fstsectionid; // 章节起始ID
    public int Chapterid; // 章id
    public int Sectionlockid; // 节解锁属性id（对应unit里的id编号）
    public int Sectionlocknum; // 属性id参数值
    public int Transsectionid; // 不满足条件时，跳转节节id
    public int Talk0; // 对话0
    public int Talk1; // 对话1
    public int Talk2; // 对话2
    public int Talk3; // 对话3
    public int Talk4; // 对话4
    public int Talk5; // 对话5
    public int Talk6; // 对话6
    public int Talk7; // 对话7
    public int Talk8; // 对话8
    public int Talk9; // 对话9
    public int Talk10; // 对话10
    public int Talk11; // 对话11
    public int Talk12; // 对话12
    public int Talk13; // 对话13
    public int Talk14; // 对话14
    public int Talk15; // 对话15
    public int Branchgotoid0; // 分支0跳转id
    public string Branchawardid0; // 分支0奖励属性
    public string Branchawardnum0; // 分支0奖励属性参数
    public int Branchgotoid1; // 分支1跳转id
    public string Branchawardid1; // 分支1奖励属性
    public string Branchawardnum1; // 分支1奖励属性数值
    public int Branchgotoid2; // 分支2跳转id
    public string Branchawardid2; // 分支2奖励属性
    public string Branchawardnum2; // 分支2奖励属性数值
}

public class SectionConfig : IConfig {
    private readonly Dictionary<int, SectionConfigData> _datas;
    public List<SectionConfigData> Datas => _datas.Values.ToList();

    public string ConfigFileName { get; }

    public SectionConfig() {
        _datas = new Dictionary<int, SectionConfigData>();
        ConfigFileName = "SectionConfig";
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
            var data = new SectionConfigData();
            int.TryParse(tables[i, 0], out data.Id);
            int.TryParse(tables[i, 1], out data.Nextid);
            int.TryParse(tables[i, 2], out data.Fstsectionid);
            int.TryParse(tables[i, 3], out data.Chapterid);
            int.TryParse(tables[i, 4], out data.Sectionlockid);
            int.TryParse(tables[i, 5], out data.Sectionlocknum);
            int.TryParse(tables[i, 6], out data.Transsectionid);
            int.TryParse(tables[i, 7], out data.Talk0);
            int.TryParse(tables[i, 8], out data.Talk1);
            int.TryParse(tables[i, 9], out data.Talk2);
            int.TryParse(tables[i, 10], out data.Talk3);
            int.TryParse(tables[i, 11], out data.Talk4);
            int.TryParse(tables[i, 12], out data.Talk5);
            int.TryParse(tables[i, 13], out data.Talk6);
            int.TryParse(tables[i, 14], out data.Talk7);
            int.TryParse(tables[i, 15], out data.Talk8);
            int.TryParse(tables[i, 16], out data.Talk9);
            int.TryParse(tables[i, 17], out data.Talk10);
            int.TryParse(tables[i, 18], out data.Talk11);
            int.TryParse(tables[i, 19], out data.Talk12);
            int.TryParse(tables[i, 20], out data.Talk13);
            int.TryParse(tables[i, 21], out data.Talk14);
            int.TryParse(tables[i, 22], out data.Talk15);
            int.TryParse(tables[i, 23], out data.Branchgotoid0);
            data.Branchawardid0 = tables[i, 24];
            data.Branchawardnum0 = tables[i, 25];
            int.TryParse(tables[i, 26], out data.Branchgotoid1);
            data.Branchawardid1 = tables[i, 27];
            data.Branchawardnum1 = tables[i, 28];
            int.TryParse(tables[i, 29], out data.Branchgotoid2);
            data.Branchawardid2 = tables[i, 30];
            data.Branchawardnum2 = tables[i, 31];
            if(_datas.ContainsKey(data.Id)) {
                throw new Exception(data.Id + "(字典中已存在具有相同Key的元素)");
            }
            else {
                _datas.Add(data.Id, data);
            }
        }
    }

    public SectionConfigData GetDataAt(int Id) {
        if (_datas.ContainsKey(Id)) {
            return _datas[Id];
        }
        return null;
    }

    public bool Add(SectionConfigData data) {
        if (_datas.ContainsKey(data.Id)) {
            return false;
        }
        _datas.Add(data.Id, data);
        return true;
    }

    public bool Remove(SectionConfigData data) {
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
