using UnityEngine;
using VContainer;

namespace Scene.Play
{
    public class PopupOpener : MonoBehaviour
    {
        private PlayPopupManager _playPopupManager;

        [Inject]
        public void Construct(PlayPopupManager playPopupManager)
        {
            _playPopupManager = playPopupManager;
        }

        public void OpenSettingsPopup()
        {
            _playPopupManager.Show<SettingsPopup>();
        }

        public void OpenFishCollectionPopup()
        {
            _playPopupManager.Show<FishCollectionPopup>();
        }
    }
}
