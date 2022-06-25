using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace SpaceInvaders
{
    public class AddressablesService
    {
        private Dictionary<string, GameObject> _assets = new Dictionary<string,GameObject>();

        public async UniTask Load(string key)
        {
            if (string.IsNullOrEmpty(key) || _assets.ContainsKey(key))
            {
                return;
            }

            try
            {
                var go = await Addressables.LoadAssetAsync<GameObject>(key).ToUniTask();
                _assets.Add(key, go);
            }
            catch (InvalidKeyException e)
            {
                Debug.LogException(e);
                return;
            }
        }

        public GameObject GetGameObject(string key)
        {
            if (string.IsNullOrEmpty(key) || !_assets.TryGetValue(key, out GameObject go))
            {
                return null;
            }

            return go;
        }
    }
}
