using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Scene.Play
{
    public class QuitPopup : BasePopup
    {
        [SerializeField] GameObject _root;

        [SerializeField] Button _cancelButton;
        [SerializeField] Button _quitButton;

        private void Awake()
        {
            _root.SetActive(false);
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
