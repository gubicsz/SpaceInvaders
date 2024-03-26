using SpaceInvaders.Models;
using SpaceInvaders.Services;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SpaceInvaders.Helpers
{
    [RequireComponent(typeof(Button))]
    public class ButtonSfx : MonoBehaviour
    {
        [Inject]
        private readonly IAudioService _audioService;

        private void Start()
        {
            // Play sfx on click
            GetComponent<Button>()
                .OnClickAsObservable()
                .Subscribe(_ => _audioService.PlaySfx(Constants.Audio.Click, 1.0f))
                .AddTo(this);
        }
    }
}
