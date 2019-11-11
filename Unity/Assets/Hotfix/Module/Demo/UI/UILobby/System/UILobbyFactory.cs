using System;
using System.Threading.Tasks;
using ETModel;
using UnityEngine;

namespace ETHotfix {
    public static class UILobbyFactory {
        public static async ETTask<UI> Create() {
            try {
                UI ui = await UIFactory.Create(UIType.UILobby);
                ui.AddComponent<UILobbyComponent>();
                
                Game.Scene.GetComponent<UIComponent>().Add(ui, UILayerType.Normal);
                return ui;
            }
            catch (Exception e) {
                Log.Error(e);
                return null;
            }
        }
    }
}