using System;
using ETModel;
using MongoDB.Bson.Serialization.Attributes;
#if !SERVER
using UnityEngine;
#endif

namespace ETHotfix {
    [BsonIgnoreExtraElements]
    public abstract class UIBaseComponent : Component {

        public T FindChild<T>(string name) where T : UnityEngine.Component {
            return GameUtility.FindDeepChild<T>(GetParent<UI>().GameObject.transform, name);
        }

        public GameObject FindChild(string name) {
            return GameUtility.FindDeepChild(GetParent<UI>().GameObject.transform, name).gameObject;
        }

        public void AddOnClickEvent(string name, Action<GameObject> onClickEvent) {
            UIEventTriggerManager.Get(FindChild(name)).OnClick += onClickEvent;
        }

        public void AddOnClickEvent(GameObject obj, Action<GameObject> onClickEvent) {
            UIEventTriggerManager.Get(obj).OnClick += onClickEvent;
        }
    }
}