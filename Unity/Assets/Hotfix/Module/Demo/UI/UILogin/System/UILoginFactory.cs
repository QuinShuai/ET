using System;
using ETModel;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace ETHotfix
{
    public static class UILoginFactory
    {
        public static UI Create()
        {
	        try
            {
                var t = Addressables.LoadAsset<GameObject>("UILogin").Result;
                ResourcesComponent resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
				resourcesComponent.LoadBundle(UIType.UILogin.StringToAB());
				GameObject bundleGameObject = (GameObject)resourcesComponent.GetAsset(UIType.UILogin.StringToAB(), UIType.UILogin);
				GameObject gameObject = UnityEngine.Object.Instantiate(t);

		        UI ui = ComponentFactory.Create<UI, string, GameObject>(UIType.UILogin, gameObject, false);

				ui.AddComponent<UILoginComponent>();
				
				return ui;
	        }
	        catch (Exception e)
	        {
				Log.Error(e);
		        return null;
	        }
		}
    }
}