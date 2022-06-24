using System;
using TMPro;
using UniRx;
using UnityEngine;

namespace SpaceInvaders
{
    public class LoadingController : MonoBehaviour
    {
        [SerializeField]
        TextMeshProUGUI _labelLoading;

        string[] _loadingTexts = new string[4]
        {
            "Loading", "Loading.", "Loading..", "Loading..."
        };

        private void Start()
        {
            // Animate loading text
            Observable.Timer(TimeSpan.FromSeconds(0.25f), TimeSpan.FromSeconds(0.25))
                .Where(_ => gameObject.activeSelf)
                .Subscribe(tick => _labelLoading.text = _loadingTexts[tick % _loadingTexts.Length])
                .AddTo(this);
        }
    }
}
