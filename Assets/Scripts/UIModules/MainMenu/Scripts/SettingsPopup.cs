using Core.UI;
using UnityEngine;
using UnityEngine.UI;
using Popup = Core.UI.Popup;

namespace UIModules.MainMenu.Scripts
{
    [RequireComponent(typeof(Popup))]
    public class SettingsPopup : Popup
    {
        public Button closeButton;
        public Switch musicSwitch;
        public Switch soundsSwitch;
        public Slider musicVolumeSlider;
        public Slider soundsVolumeSlider;
        public Slider sensitivitySlider;
    }
}