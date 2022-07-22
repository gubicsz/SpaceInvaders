using UnityEngine;

namespace SpaceInvaders
{
    public class StorageService : IStorageService
    {
        public void Save<T>(string key, T obj)
        {
            // Handle error
            if (string.IsNullOrEmpty(key) || obj == null)
            {
                return;
            }

            // Serialize object to json
            string json = JsonUtility.ToJson(obj);

            // Save to player prefs
            PlayerPrefs.SetString(key, json);
            PlayerPrefs.Save();
        }

        public T Load<T>(string key)
        {
            // Handle error
            if (string.IsNullOrEmpty(key))
            {
                return default;
            }

            // Load json from player prefs
            string json = PlayerPrefs.GetString(key);

            // Deserialize object from json
            return JsonUtility.FromJson<T>(json);
        }
    }
}
