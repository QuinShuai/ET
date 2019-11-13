using System.Collections.Generic;

namespace ETHotfix {
    public enum UIType {
        Invaild = 0,
        UILobby,
        UILogin,
    }

    public class UIResource {
        public static Dictionary<UIType, string> PanelPath = new Dictionary<UIType, string> {
            {UIType.UILobby, "UI/UILobby.prefab" },
            {UIType.UILogin, "UI/UILogin.prefab" },
        };
        
        public static Dictionary<UIType, string> PanelStr = new Dictionary<UIType, string> {
            {UIType.UILobby, "UILobby" },
            {UIType.UILogin, "UILogin" },
        };
        
        public static string GetPanelPath(UIType uiType) {
            if (PanelPath.TryGetValue(uiType, out var path)) {
                return path;
            }
            
            return string.Empty;
        }
        
        public static string GetPanelStr(UIType uiType) {
            if (PanelStr.TryGetValue(uiType, out var str)) {
                return str;
            }
            
            return string.Empty;
        }
    }
}
