using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace SpaceInvaders.Services
{
    public class AddressablesService : IAssetService, IDisposable
    {
        private readonly Dictionary<string, object> _assets = new();
        private readonly List<AsyncOperationHandle> _handles = new();

        public async UniTask Load<T>(string key)
        {
            // Handle errors
            if (string.IsNullOrEmpty(key) || _assets.ContainsKey(key))
                return;

            try
            {
                // Create load asset handle
                var handle = Addressables.LoadAssetAsync<T>(key);
                _handles.Add(handle);

                // Load addressable asset
                var obj = await handle.ToUniTask();
                _assets.Add(key, obj);
            }
            catch (InvalidKeyException e)
            {
                Debug.LogException(e);
            }
        }

        public T Get<T>(string key)
        {
            // Handle errors
            if (string.IsNullOrEmpty(key) || !_assets.TryGetValue(key, out var obj))
                return default;

            return obj is T t ? t : default;
        }

        public void Dispose()
        {
            // Release addressable handles
            for (var i = 0; i < _handles.Count; i++)
                Addressables.Release(_handles[i]);
        }
    }
}
