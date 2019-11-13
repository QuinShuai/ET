using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace ETModel
{
	public static class ConfigHelper
	{
		public static async ETTask<string> GetText(string key)
		{
			try
			{
                var config = await Addressables.LoadAssetAsync<TextAsset>($"Config/{key}.txt").Task;
				return config.text;
			}
			catch (Exception e)
			{
				throw new Exception($"load config file fail, key: {key}", e);
			}
		}
		
		public static string GetGlobal()
		{
			try
			{
				GameObject config = (GameObject)ResourcesHelper.Load("KV");
				string configStr = config.Get<TextAsset>("GlobalProto").text;
				return configStr;
			}
			catch (Exception e)
			{
				throw new Exception($"load global config file fail", e);
			}
		}

		public static T ToObject<T>(string str)
		{
			return JsonHelper.FromJson<T>(str);
		}
	}
}