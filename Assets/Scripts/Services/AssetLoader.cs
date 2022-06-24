using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace SpaceInvaders
{
    public class AssetLoader
    {
        private Dictionary<string, GameObject> _assets = new Dictionary<string,GameObject>();

        public async UniTask LoadAsset(string key)
        {
            if (string.IsNullOrEmpty(key) || _assets.ContainsKey(key))
            {
                return;
            }

            var asset = await Addressables.LoadAssetAsync<GameObject>(key).ToUniTask();
            _assets.Add(key, asset);
        }

        public GameObject GetAsset(string key)
        {
            if (string.IsNullOrEmpty(key) || !_assets.TryGetValue(key, out GameObject asset))
            {
                return null;
            }

            return asset;
        }
    }
}
