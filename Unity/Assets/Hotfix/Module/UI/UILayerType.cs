using System;
using System.Collections.Generic;

namespace ETHotfix {
    public enum UILayerType {
        Normal,    // 可推出界面(UIMainMenu,UIRank等)
        Fixed,     // 固定窗口(UITopBar等)
        PopUp,     // 模式窗口(UIMessageBox, yourPopWindow , yourTipsWindow ......)
        Top,       // 模式窗口最顶层UI
    }
}