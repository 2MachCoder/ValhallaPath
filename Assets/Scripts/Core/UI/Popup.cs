using Core.Views;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Core.UI
{
    public abstract class Popup : UIView
    {
        [Inject] private RootCanvas _mainCanvas;
        [SerializeField] private Image background;

        public override async UniTask  Show()
        {
            background.gameObject.SetActive(true); 
            background.GetComponent<RectTransform>().sizeDelta = _mainCanvas.GetComponent<RectTransform>().sizeDelta*6;
            base.Show().Forget();
            transform.localScale = new Vector3(0.5f,.5f,0f);
            await transform.DOScale(1.02f, 0.2f);
            await transform.DOScale(1f, 0.2f);
        }
    
        public override async UniTask Hide()
        {
            await transform.DOScale(1.03f, .2f);
            base.Hide().Forget();
            await transform.DOScale(.5f, .2f);
        }
    }
}