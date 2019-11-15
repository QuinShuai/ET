using System;
using ETModel;
using UnityEngine.AddressableAssets;

namespace ETHotfix {
    public static class Init {
        public static async void Start() {
#if ILRuntime
			if (!Define.IsILRuntime)
			{
				Log.Error("mono层是mono模式, 但是Hotfix层是ILRuntime模式");
			}
#else
            if (Define.IsILRuntime) {
                Log.Error("mono层是ILRuntime模式, Hotfix层是mono模式");
            }
#endif

            try {
                // 注册热更层回调
                ETModel.Game.Hotfix.Update = () => { Update(); };
                ETModel.Game.Hotfix.LateUpdate = () => { LateUpdate(); };
                ETModel.Game.Hotfix.OnApplicationQuit = () => { OnApplicationQuit(); };

                Game.Scene.AddComponent<UIComponent>();
                Game.Scene.AddComponent<OpcodeTypeComponent>();
                Game.Scene.AddComponent<MessageDispatcherComponent>();

                // 加载热更配置
                Game.Scene.AddComponent<ConfigComponent>();
                await Game.Scene.GetComponent<ConfigComponent>().Load();
                //Game.Scene.GetComponent<ConfigComponent>().Load().Coroutine();
                Log.Debug(Addressables.RuntimePath);
                //var unitConfig = Game.Scene.GetComponent<ConfigComponent>().UnitConfig.GetDataAt(1);
                //Log.Debug($"config {JsonHelper.ToJson(unitConfig)}");

                Game.EventSystem.Run(EventIdType.InitSceneStart);
            }
            catch (Exception e) {
                Log.Error(e);
            }
        }

        public static void Update() {
            try {
                Game.EventSystem.Update();
            }
            catch (Exception e) {
                Log.Error(e);
            }
        }

        public static void LateUpdate() {
            try {
                Game.EventSystem.LateUpdate();
            }
            catch (Exception e) {
                Log.Error(e);
            }
        }

        public static void OnApplicationQuit() {
            Game.Close();
        }
    }
}