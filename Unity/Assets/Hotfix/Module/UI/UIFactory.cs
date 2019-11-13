using System;
using ETModel;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace ETHotfix {
    public static class UIFactory {

        public static async ETTask<UI> Create(UIType uiType) {
            try {
                var gameObject = await Addressables.InstantiateAsync(UIResource.PanelPath[uiType]).Task;
                var ui = ComponentFactory.Create<UI, string, GameObject>(UIResource.PanelStr[uiType], gameObject, false);
                return ui;
            }
            catch (Exception e) {
                Log.Error(e);
                return null;
            }
        }
    }
}
