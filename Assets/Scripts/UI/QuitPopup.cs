using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace FishingBlast.UI
{
    public class QuitPopup : BasePopup
    {
        [SerializeField] Button _cancelButton;
        [SerializeField] Button _quitButton;

        private void Awake()
        {
            _cancelButton.onClick.AddListener(Close);
            _quitButton.onClick.AddListener(Quit);
        }
        private void OnDestroy()
        {
            _cancelButton.onClick.RemoveListener(Close);
            _quitButton.onClick.RemoveListener(Quit);
        }

        [Inject]
        public void Construct()
        {
            // 게임 흐름 매니저
        }

        private void Quit()
        {
            Application.Quit();
        }
    }
}
