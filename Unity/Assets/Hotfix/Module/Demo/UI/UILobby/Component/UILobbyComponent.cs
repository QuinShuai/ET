using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix {
    [ObjectSystem]
    public class UiLobbyComponentSystem : AwakeSystem<UILobbyComponent> {
        public override void Awake(UILobbyComponent self) {
            self.Awake();
        }
    }

    public class UILobbyComponent : UIBaseComponent {
        private GameObject enterMap;
        private Text text;

        public void Awake() {

            AddOnClickEvent("EnterMap", EnterMap);

            this.text = FindChild<Text>("Text");
        }

        private void EnterMap(GameObject go) {
            MapHelper.EnterMapAsync().Coroutine();
        }


    }
}
