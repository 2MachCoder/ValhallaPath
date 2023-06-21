using System.Threading;
using Core.Controllers;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Start
{
    public class RootController : MonoBehaviour, IRootController
    {
        private readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);
        
        [Inject]
        private ControllerMapper _controllerMapper;

        private IController _currentController;

        private void Start()
        {
            Application.targetFrameRate = 30;
            RunController(ControllerMap.MainMenu, null).Forget();
        }

        public async UniTaskVoid RunController(ControllerMap controllerMap, object param)
        {
            await _semaphoreSlim.WaitAsync();
            try
            {
                if (_controllerMapper == null)
                    Debug.Log("ControllerMapper - NULL");
                _currentController = _controllerMapper.Resolve(controllerMap);
                await _currentController.Run(param);
                await _currentController.Stop();
                _currentController.Dispose();
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }
    }
}