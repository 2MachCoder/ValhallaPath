using System;
using System.Collections.Generic;
using Core.Controllers;
using Modules.Game.Scripts;
using Modules.MainMenu.Scripts;
using Modules.StartGame.Scripts;
using Zenject;

namespace Start
{
    public class ControllerMapper
    {
        private readonly Dictionary<ControllerMap, Type> _map;
        private readonly DiContainer _diContainer;

        public ControllerMapper(DiContainer diContainer)
        {
            _diContainer = diContainer;
            _map = new Dictionary<ControllerMap, Type>
            {
                { ControllerMap.StartGame, typeof(StartGameController)},
                { ControllerMap.MainMenu, typeof(MainMenuController)},
                { ControllerMap.PlayMode, typeof(PlayModeController)}
            };
        }

        public IController Resolve(ControllerMap controllerMap)
        {
            return (IController)_diContainer.Resolve(_map[controllerMap]);
        }
    }
}