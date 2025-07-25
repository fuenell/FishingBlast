using AppScope.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Scene.Play
{
    public class SettingsPopup : BasePopup
    {
        [SerializeField] GameObject _root;

        [SerializeField] Button _closeButton;
        [SerializeField] TMP_Dropdown _volumeDropdown;
        [SerializeField] Toggle _muteToggle;

        SoundManager _soundManager;

        private void Awake()
        {
            _root.SetActive(false);
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

        public override void Open()
        {
            _root.SetActive(true);
        }

        protected override void OnClose()
        {
            _root.SetActive(false);
        }
    }
}
