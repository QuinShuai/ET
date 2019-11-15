using System;
using System.Net;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix {
    [ObjectSystem]
    public class UiLoginComponentSystem : AwakeSystem<UILoginComponent> {
        public override void Awake(UILoginComponent self) {
            self.Awake();
        }
    }

    public class UILoginComponent : UIBaseComponent {
        private GameObject account;
        private GameObject loginBtn;

        public void Awake() {
            AddOnClickEvent("LoginBtn", this.OnLogin);
            this.account = FindChild("Account");
        }

        public void OnLogin(GameObject go) {
            LoginHelper.OnLoginAsync(this.account.GetComponent<InputField>().text).Coroutine();
        }
    }
}
