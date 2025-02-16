﻿using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Core.Views
{
    public class UIView : MonoBehaviour, IDisposable
    {
        public virtual async UniTask Show()
        {
            transform.GetComponent<CanvasGroup>().DOFade(0, 0);
            gameObject.SetActive(true);
            await transform.GetComponent<CanvasGroup>().DOFade(1,0.25f); 
        }
        public virtual async UniTask Hide()
        {
            await transform.GetComponent<CanvasGroup>().DOFade(0,0.25f); 
            gameObject.SetActive(false);
        }
        public void Dispose()
        {
            Destroy(gameObject);
        }
    }
}
