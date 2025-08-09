using FishingBlast.AppScope;
using FishingBlast.UI;
using UnityEngine;
using VContainer;

namespace FishingBlast.Play
{
    public class PopupOpener : MonoBehaviour
    {
        private PopupManager _popupManager;

        [Inject]
        public void Construct(PopupManager popupManager)
        {
            _popupManager = popupManager;
        }

        public void OpenSettingsPopup()
        {
            _popupManager.Show<SettingsPopup>();
        }

        public void OpenFishCollectionPopup()
        {
            _popupManager.Show<FishCollectionPopup>();
        }
    }
}
