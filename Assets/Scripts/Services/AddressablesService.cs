using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace SpaceInvaders
{
    public class AddressablesService : IDisposable
    {
        private Dictionary<string, object> _assets = new Dictionary<string, object>();
        private List<AsyncOperationHandle> _handles = new List<AsyncOperationHandle>();

        public async UniTask LoadAsset<T>(string key)
        {
            // Handle errors
            if (string.IsNullOrEmpty(key) || _assets.ContainsKey(key))
            {
                return;
            }

            try
            {
                // Create load asset handle
                var handle = Addressables.LoadAssetAsync<T>(key);
                _handles.Add(handle);

                // Load addressable asset
                T obj = await handle.ToUniTask();
                _assets.Add(key, obj);
            }
            catch (InvalidKeyException e)
            {
                Debug.LogException(e);
                return;
            }
        }

        public T GetAsset<T>(string key)
        {
            // Handle errors
            if (string.IsNullOrEmpty(key) || !_assets.TryGetValue(key, out object obj))
            {
                return default;
            }

            return (obj is T t) ? t : default;
        }

        public void Dispose()
        {
            // Release addressable handles
            foreach (var handle in _handles)
            {
                Addressables.Release(handle);
            }
        }
    }
}
