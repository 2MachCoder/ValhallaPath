using Core.Views;
using TMPro;
using UnityEngine;
using Button = UnityEngine.UI.Button;

namespace UIModules.GameScreen.Scripts
{
    public class GameScreenUIView : UIView
    {
        [SerializeField] public TextMeshProUGUI messageText;
        [SerializeField] public Button interactButton;
        [SerializeField] public Button crouchButton;
        [SerializeField] public Button dropButton;
        [SerializeField] public Joystick walkJoystick;
    }
}