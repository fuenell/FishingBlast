using FishingBlast.AppScope;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace FishingBlast.UI
{
    public class SettingsPopup : BasePopup
    {
        [SerializeField] Button _closeButton;
        [SerializeField] TMP_Dropdown _volumeDropdown;
        [SerializeField] Toggle _muteToggle;

        SoundManager _soundManager;

        private void Awake()
        {
            _closeButton.onClick.AddListener(Close);
        }

        private void OnDestroy()
        {
            _closeButton.onClick.RemoveListener(Close);
        }

        [Inject]
        public void Construct(SoundManager soundManager)
        {
            _soundManager = soundManager;
        }
    }
}
