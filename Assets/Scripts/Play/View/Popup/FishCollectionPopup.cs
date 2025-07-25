using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Scene.Play
{
    public class FishCollectionPopup : BasePopup
    {
        [SerializeField] GameObject _root;
        [SerializeField] GameObject _test;

        [SerializeField] Button _closeButton;

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
        public void Construct()
        {
            // 도감 데이터 주입 받기
        }

        public override void Open()
        {
            _root.SetActive(true);
            _test.SetActive(true);
        }

        protected override void OnClose()
        {
            _root.SetActive(false);
        }

        public override void Back()
        {
            if (_test.activeSelf)
            {
                _test.SetActive(false);
            }
            else
            {
                Close();
            }
        }
    }
}
