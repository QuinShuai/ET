using System;
using System.Threading;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace ETModel {
    public class Init : MonoBehaviour {
        private void Start() {
            this.StartAsync().Coroutine();
        }

        private async ETVoid StartAsync() {
            try {
                SynchronizationContext.SetSynchronizationContext(OneThreadSynchronizationContext.Instance);
                //var assets = await Addressables.LoadResourceLocationsAsync("All").Task;
                //foreach (var asset in assets) {
                //    await Addressables.DownloadDependenciesAsync(asset.PrimaryKey).Task;
                //    var size = await Addressables.GetDownloadSizeAsync(asset.PrimaryKey).Task;
                //    Debug.Log(size);
                //}
                var download = Addressables.DownloadDependenciesAsync("All");
                download.Completed += (t) => {
                    Debug.Log(t + "   " + download);
                };
                
                //while (!download.IsDone || download.Status == AsyncOperationStatus.None) {
                //    Debug.Log(download.PercentComplete);
                //    Debug.Log(download.Status);
                //    Thread.Sleep(1);
                //}

                //DontDestroyOnLoad(gameObject);
                //Game.EventSystem.Add(DLLType.Model, typeof(Init).Assembly);

                //Game.Scene.AddComponent<TimerComponent>();
                //Game.Scene.AddComponent<GlobalConfigComponent>();
                //Game.Scene.AddComponent<NetOuterComponent>();
                //Game.Scene.AddComponent<ResourcesComponent>();
                //Game.Scene.AddComponent<PlayerComponent>();
                //Game.Scene.AddComponent<UnitComponent>();
                //Game.Scene.AddComponent<UIComponent>();

                //// 下载ab包
                ////await BundleHelper.DownloadBundle();

                //await Game.Hotfix.LoadHotfixAssembly();

                //// 加载配置
                ////Game.Scene.AddComponent<ConfigComponent>();
                //Game.Scene.AddComponent<OpcodeTypeComponent>();
                //Game.Scene.AddComponent<MessageDispatcherComponent>();

                //Game.Hotfix.GotoHotfix();

                //Game.EventSystem.Run(EventIdType.TestHotfixSubscribMonoEvent, "TestHotfixSubscribMonoEvent");
            }
            catch (Exception e) {
                Log.Error(e);
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