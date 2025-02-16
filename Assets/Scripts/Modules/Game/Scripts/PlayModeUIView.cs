using Core.Views;
using Modules.Game.Scripts.Popups;
using Modules.Level.Scripts.Popups;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Modules.Game.Scripts
{
    public class PlayModeUIView : UIView
    {
        public GameObject healthUI;
        public Slider healthBar;
        public LevelResultPopup winPopup;
        public LevelResultPopup losePopup;
        public StartLevelPopup startPopup;
    }
}