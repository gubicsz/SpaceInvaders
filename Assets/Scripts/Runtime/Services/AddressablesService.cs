using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace SpaceInvaders.Services
{
    public class AddressablesService : IAssetService, IDisposable
    {
        readonly Dictionary<string, object> _assets = new();
        readonly List<AsyncOperationHandle> _handles = new();

        public async UniTask Load<T>(string key)
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

        public T Get<T>(string key)
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
            for (int i = 0; i < _handles.Count; i++)
            {
                Addressables.Release(_handles[i]);
            }
        }
    }
}
