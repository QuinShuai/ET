using System;
using System.Collections.Generic;
using System.Linq;
using ETModel;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace ETHotfix {
    [ObjectSystem]
    public class ConfigComponentAwakeSystem : AwakeSystem<ConfigComponent> {
        public override void Awake(ConfigComponent self) {
            self.Awake();
        }
    }

    //[ObjectSystem]
    //public class ConfigComponentLoadSystem : LoadSystem<ConfigComponent> {
    //    public override void Load(ConfigComponent self) {
    //        self.Load().Coroutine();
    //    }
    //}

    /// <summary>
    /// Config组件会扫描所有的有ConfigAttribute标签的配置,加载进来
    /// </summary>
    public class ConfigComponent : Component {

        private Dictionary<string, IConfig> _configs;
        public List<IConfig> Configs => _configs.Values.ToList();

        public void Awake() {
            _configs = new Dictionary<string, IConfig>();
            this.Load().Coroutine();
        }

        public async ETTask Load() {

            var configs = await Addressables.LoadResourceLocationsAsync("Config").Task;
            foreach (var location in configs) {
                var config = await Addressables.LoadAssetAsync<TextAsset>(location).Task;
                var name = location.PrimaryKey.Replace("Config/", "").Replace(".txt", "");
            }

            List<Type> types = Game.EventSystem.GetTypes();

            foreach (Type type in types) {
                object[] attrs = type.GetCustomAttributes(typeof(ConfigAttribute), false);
                if (attrs.Length == 0) {
                    continue;
                }

                ConfigAttribute configAttribute = attrs[0] as ConfigAttribute;
                // 只加载指定的配置
                if (!configAttribute.Type.Is(AppType.ClientH)) {
                    continue;
                }

                object obj = Activator.CreateInstance(type);

                ACategory iCategory = obj as ACategory;
                if (iCategory == null) {
                    throw new Exception($"class: {type.Name} not inherit from ACategory");
                }
                iCategory.BeginInitAsync();
                iCategory.EndInit();

                //this.allConfig[iCategory.ConfigType] = iCategory;
            }
        }

        public IConfig GetOne(Type type) {
            ACategory configCategory = null;
            //if (!this.allConfig.TryGetValue(type, out configCategory)) {
            //    throw new Exception($"ConfigComponent not found key: {type.FullName}");
            //}
            return configCategory.GetOne();
        }

        public IConfig Get(Type type, int id) {
            ACategory configCategory = null;
            //if (!this.allConfig.TryGetValue(type, out configCategory)) {
            //    throw new Exception($"ConfigComponent not found key: {type.FullName}");
            //}

            return configCategory.TryGet(id);
        }

        public IConfig TryGet(Type type, int id) {
            ACategory configCategory = null;
            //if (!this.allConfig.TryGetValue(type, out configCategory)) {
            //    return null;
            //}
            return configCategory.TryGet(id);
        }

        public IConfig[] GetAll(Type type) {
            ACategory configCategory = null;
            //if (!this.allConfig.TryGetValue(type, out configCategory)) {
            //    throw new Exception($"ConfigComponent not found key: {type.FullName}");
            //}
            return configCategory.GetAll();
        }

        public ACategory GetCategory(Type type) {
            ACategory configCategory = null;
            //bool ret = this.allConfig.TryGetValue(type, out configCategory);
            return null;
        }
    }
}