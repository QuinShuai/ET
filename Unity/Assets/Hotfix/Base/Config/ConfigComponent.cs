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

    public class ConfigComponent : Component {


        private Dictionary<string, IConfig> _configs;
        public List<IConfig> Configs => _configs.Values.ToList();

        public void Awake() {
            _configs = new Dictionary<string, IConfig>();
        }

        public async ETTask Load() {
            var configs = await Addressables.LoadResourceLocationsAsync("Config").Task;
            foreach (var location in configs) {
                var config = await Addressables.LoadAssetAsync<TextAsset>(location).Task;
                var name = location.PrimaryKey.Replace("Configs/", "").Replace(".txt", "").ToLower();
                if (_configs.ContainsKey(name)){
                    var iConfig = _configs[name];
                    iConfig.Deserialize(config.text);
                }
            }
        }
    }
}
