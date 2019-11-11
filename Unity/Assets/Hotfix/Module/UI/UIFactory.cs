using System;
using System.Threading.Tasks;
using ETModel;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace ETHotfix {
    public static class UIFactory {

        public static async ETTask<UI> Create(string uiType) {
            try {
                var gameObject = await Addressables.InstantiateAsync(uiType).Task;
                var ui = ComponentFactory.Create<UI, string, GameObject>(uiType, gameObject, false);
                return ui;
            }
            catch (Exception e) {
                Log.Error(e);
                return null;
            }
        }
    }
}
