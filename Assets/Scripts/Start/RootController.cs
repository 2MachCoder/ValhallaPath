using System.Threading;
using Core.Controllers;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Start
{
    public class RootController : MonoBehaviour, IRootController
    {
        [Inject] private ControllerMapper _controllerMapper;
        private readonly SemaphoreSlim _semaphoreSlim = new (1, 1);
        private IController _currentController;

        private void Start() => RunController(ControllerMap.StartGame).Forget();
        
        public async UniTaskVoid RunController(ControllerMap controllerMap, object param = null)
        {
            await _semaphoreSlim.WaitAsync();
            
            try
            {
                _currentController = _controllerMapper.Resolve(controllerMap);
                await _currentController.Run(param);
                await _currentController.Stop();
                _currentController.Dispose();
            }
            finally { _semaphoreSlim.Release(); }
        }
    }
}