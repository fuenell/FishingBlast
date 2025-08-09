using UnityEngine;
using UnityEngine.UI;
using VContainer;
using Cysharp.Threading.Tasks;

namespace FishingBlast.Title
{
    public class TitleStartButtonHandler : MonoBehaviour
    {
        [SerializeField] private Button startButton;

        private TitleFlowController _flow;

        [Inject]
        public void Construct(TitleFlowController flow)
        {
            _flow = flow;
        }

        private void Awake()
        {
            startButton.onClick.AddListener(OnClick);
        }

        private void OnDestroy()
        {
            startButton.onClick.RemoveListener(OnClick);
        }

        private void OnClick()
        {
            startButton.interactable = false;

            _flow.OnClickStartButton().Forget();
        }
    }
}
