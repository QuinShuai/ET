using System;
using System.Collections.Generic;
using ETModel;
using UnityEngine;

namespace ETHotfix {
    [ObjectSystem]
    public class UIComponentAwakeSystem : AwakeSystem<UIComponent> {
        public override void Awake(UIComponent self) {
            self.Camera = Component.Global.transform.Find("UICamera").GetComponent<Camera>();
            if (self.NormalUIRoot == null) {
                self.NormalUIRoot = new GameObject("NormalUIRoot").transform;
                GameUtility.AddChildToTarget(self.Transform, self.NormalUIRoot);
                GameUtility.ChangeChildLayer(self.NormalUIRoot, self.GameObject.layer);
            }
            if (self.FixedUIRoot == null) {
                self.FixedUIRoot = new GameObject("FixedUIRoot").transform;
                GameUtility.AddChildToTarget(self.Transform, self.FixedUIRoot);
                GameUtility.ChangeChildLayer(self.FixedUIRoot, self.GameObject.layer);
            }
            if (self.PopUpUIRoot == null) {
                self.PopUpUIRoot = new GameObject("PopUpUIRoot").transform;
                GameUtility.AddChildToTarget(self.Transform, self.PopUpUIRoot);
                GameUtility.ChangeChildLayer(self.PopUpUIRoot, self.GameObject.layer);
            }
            if (self.TopUIRoot == null) {
                self.TopUIRoot = new GameObject("TopUIRoot").transform;
                GameUtility.AddChildToTarget(self.Transform, self.TopUIRoot);
                GameUtility.ChangeChildLayer(self.TopUIRoot, self.GameObject.layer);
            }
        }
    }

    /// <summary>
    /// 管理所有UI
    /// </summary>
    public class UIComponent : Component {

        public Camera Camera;
        public Transform NormalUIRoot;
        public Transform FixedUIRoot;
        public Transform PopUpUIRoot;
        public Transform TopUIRoot;
        private const int NormalUIDepth = 2;
        private const int FixedUIDepth = 100;
        private const int PopUpUIDepth = 150;
        private const int TopUIDepth = 200;

        public Dictionary<string, UI> uis = new Dictionary<string, UI>();

        public void Add(UI ui, UILayerType layerType = UILayerType.Normal) {
            ui.Canvas.worldCamera = Camera;

            this.uis.Add(ui.Name, ui);
            ui.Parent = this;
            switch (layerType) {
                case UILayerType.Normal:
                    ui.Transform.SetParent(NormalUIRoot);
                    break;
                case UILayerType.Fixed:
                    ui.Transform.SetParent(this.FixedUIRoot);
                    break;
                case UILayerType.PopUp:
                    ui.Transform.SetParent(this.PopUpUIRoot);
                    break;
                default:
                    ui.Transform.SetParent(this.TopUIRoot);
                    break;
            }
            AdjustUIDepth(ui, layerType);
        }

        private void AdjustUIDepth(UI ui, UILayerType layerType) {
            var needDepth = 1;
            if (layerType == UILayerType.Normal) {
                var value = GameUtility.GetMaxTargetDepth(this.NormalUIRoot.gameObject, false);
                needDepth = Mathf.Clamp(value + 1, NormalUIDepth, int.MaxValue);
            }
            else if (layerType == UILayerType.PopUp) {
                var value = GameUtility.GetMaxTargetDepth(this.PopUpUIRoot.gameObject, false);
                needDepth = Mathf.Clamp(value + 1, PopUpUIDepth, int.MaxValue);
            }
            else if (layerType == UILayerType.Fixed) {
                var value = GameUtility.GetMaxTargetDepth(this.FixedUIRoot.gameObject, false);
                needDepth = Mathf.Clamp(value + 1, FixedUIDepth, int.MaxValue);
            }
            else if (layerType == UILayerType.Top) {
                var value = GameUtility.GetMaxTargetDepth(this.TopUIRoot.gameObject, false);
                needDepth = Mathf.Clamp(value + 1, TopUIDepth, int.MaxValue);
            }
            GameUtility.SetTargetMinPanelDepth(ui.GameObject, needDepth);
        }

        public void Remove(UIType uiType) {
            var name = UIResource.GetPanelStr(uiType);
            if (!this.uis.TryGetValue(name, out UI ui)) {
                return;
            }
            this.uis.Remove(name);
            ui.Dispose();
        }

        public UI Get(string name) {
            UI ui = null;
            this.uis.TryGetValue(name, out ui);
            return ui;
        }
    }
}