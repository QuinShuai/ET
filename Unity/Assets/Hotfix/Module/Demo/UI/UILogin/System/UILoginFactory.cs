using System;
using ETModel;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace ETHotfix {
    public static class UILoginFactory {

        public static async ETTask<UI> Create() {
            try {
                UI ui = await UIFactory.Create("UILogin");
                ui.AddComponent<UILoginComponent>();

                Game.Scene.GetComponent<UIComponent>().Add(ui);
                return ui;
            }
            catch (Exception e) {
                Log.Error(e);
                return null;
            }
        }
    }
}