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
            AddOnClickEvent("Button1", OnClick1);
            AddOnClickEvent("Button2", OnClick2);
            AddOnClickEvent("Button3", OnClick3);
            AddOnClickEvent("Button4", OnClick4);
            this.account = FindChild("Account");
        }

        private void OnClick4(GameObject obj) {
            Debug.LogError("Button4");
        }

        private void OnClick3(GameObject obj) {
            Debug.LogError("Button3");
        }

        private void OnClick2(GameObject obj) {
            Debug.LogError("Button2");
        }

        private void OnClick1(GameObject obj) {
            Debug.LogError("Button1");
        }

        public void OnLogin(GameObject go) {
            LoginHelper.OnLoginAsync(this.account.GetComponent<InputField>().text).Coroutine();
        }
    }
}
