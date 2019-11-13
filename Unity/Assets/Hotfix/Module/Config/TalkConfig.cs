using System;
using System.Text;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using System.Globalization;

namespace ETHotfix {
public class TalkConfigData {
    public int Talkid; // 对话编号
    public string Bg; // 背景
    public string Bgeffect; // 背景特效
    public int Bganimid; // 背景动效
    public string Bgm; // 音乐
    public string Sound; // 音效
    public int Frametype; // 对话框类型（1=npc微信框，2=主角微信框，3=微信消息框,4=npc情景对话框，5=主角情景对话框，6=旁白框）
    public int Delay; // 消息延迟(ms)
    public int Unitid; // 讲话者
    public string Text; // 文本内容
    public int Texttype; // 消息类型（1=文本，2=表情包，3=主角发图）
    public string Npc; // 立绘
    public int Npcanimid; // 立绘动效
    public int Frameanimid; // 对话框动效
    public int Roleexp; // 主角立绘表情
    public int Wordsize; // 文字大小
    public string Mark; // 标记
}

public class TalkConfig : IConfig {
    private readonly Dictionary<int, TalkConfigData> _datas;
    public List<TalkConfigData> Datas => _datas.Values.ToList();

    public string ConfigFileName { get; }

    public TalkConfig() {
        _datas = new Dictionary<int, TalkConfigData>();
        ConfigFileName = "TalkConfig";
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
            var data = new TalkConfigData();
            int.TryParse(tables[i, 0], out data.Talkid);
            data.Bg = tables[i, 1];
            data.Bgeffect = tables[i, 2];
            int.TryParse(tables[i, 3], out data.Bganimid);
            data.Bgm = tables[i, 4];
            data.Sound = tables[i, 5];
            int.TryParse(tables[i, 6], out data.Frametype);
            int.TryParse(tables[i, 7], out data.Delay);
            int.TryParse(tables[i, 8], out data.Unitid);
            data.Text = tables[i, 9];
            int.TryParse(tables[i, 10], out data.Texttype);
            data.Npc = tables[i, 11];
            int.TryParse(tables[i, 12], out data.Npcanimid);
            int.TryParse(tables[i, 13], out data.Frameanimid);
            int.TryParse(tables[i, 14], out data.Roleexp);
            int.TryParse(tables[i, 15], out data.Wordsize);
            data.Mark = tables[i, 16];
            if(_datas.ContainsKey(data.Talkid)) {
                throw new Exception(data.Talkid + "(字典中已存在具有相同Key的元素)");
            }
            else {
                _datas.Add(data.Talkid, data);
            }
        }
    }

    public TalkConfigData GetDataAt(int Talkid) {
        if (_datas.ContainsKey(Talkid)) {
            return _datas[Talkid];
        }
        return null;
    }

    public bool Add(TalkConfigData data) {
        if (_datas.ContainsKey(data.Talkid)) {
            return false;
        }
        _datas.Add(data.Talkid, data);
        return true;
    }

    public bool Remove(TalkConfigData data) {
        if (!_datas.ContainsKey(data.Talkid)) {
            return false;
        }
        _datas.Remove(data.Talkid);
        return true;
    }

    public bool RemoveAt(int Talkid) {
        if (_datas.ContainsKey(Talkid)) {
            _datas.Remove(Talkid);
            return true;
        }
        return false;
    }
}
}
