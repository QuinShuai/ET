using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace ETModel {
    public class Init : MonoBehaviour {

        private AsyncOperationHandle _download;

        private void Start() {
            this.StartAsync().Coroutine();
        }

        private async ETVoid StartAsync() {
            try {
                SynchronizationContext.SetSynchronizationContext(OneThreadSynchronizationContext.Instance);

                Debug.Log("检测更新!");
                await Download();

                DontDestroyOnLoad(gameObject);
                Game.EventSystem.Add(DLLType.Model, typeof(Init).Assembly);

                Game.Scene.AddComponent<TimerComponent>();
                Game.Scene.AddComponent<GlobalConfigComponent>();
                Game.Scene.AddComponent<NetOuterComponent>();
                Game.Scene.AddComponent<ResourcesComponent>();
                Game.Scene.AddComponent<PlayerComponent>();
                Game.Scene.AddComponent<UnitComponent>();
                Game.Scene.AddComponent<UIComponent>();

                // 下载ab包
                //await BundleHelper.DownloadBundle();

                await Game.Hotfix.LoadHotfixAssembly();

                // 加载配置
                //Game.Scene.AddComponent<ConfigComponent>();
                Game.Scene.AddComponent<OpcodeTypeComponent>();
                Game.Scene.AddComponent<MessageDispatcherComponent>();

                Game.Hotfix.GotoHotfix();

                Game.EventSystem.Run(EventIdType.TestHotfixSubscribMonoEvent, "TestHotfixSubscribMonoEvent");
            }
            catch (Exception e) {
                Log.Error(e);
            }
        }

        private async ETTask Download() {
            Debug.Log("开始更新!");
            _download = Addressables.DownloadDependenciesAsync("All");
            while (_download.PercentComplete < 1) {
                await Task.Delay(50);
                Debug.Log("更新进度: " + _download.PercentComplete.ToString("P"));
            }
            if (_download.Status == AsyncOperationStatus.Failed) {
                Debug.Log("更新失败: " + _download.OperationException);
                Debug.Log(_download.PercentComplete + "    " + _download.IsDone + "    " + _download.Status + "    " + _download.IsValid() + "    " + _download.OperationException);
                await Download();
            }
            else {
                Debug.Log("更新完成!");
            }
        }

        private void Update() {
            OneThreadSynchronizationContext.Instance.Update();
            Game.Hotfix.Update?.Invoke();
            Game.EventSystem.Update();
        }

        private void LateUpdate() {
            Game.Hotfix.LateUpdate?.Invoke();
            Game.EventSystem.LateUpdate();
        }

        private void OnApplicationQuit() {
            Game.Hotfix.OnApplicationQuit?.Invoke();
            Game.Close();
        }
    }
}