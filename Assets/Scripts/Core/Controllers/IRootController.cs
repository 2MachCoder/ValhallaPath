using Cysharp.Threading.Tasks;

namespace Core.Controllers
{
    public interface  IRootController
    {
        UniTaskVoid RunController(ControllerMap controllerMap, object param);
    }
    
    public static class RootControllerExtension
    {
        public static UniTaskVoid RunController(this IRootController self, ControllerMap controllerMap)
        {
            return self.RunController(controllerMap, null);
        }
    }
    
    public enum ControllerMap
    {
        //StartGame = 0,
        MainMenu = 0,
        Settings = 1,
        GameScreen = 2,
        PauseScreen = 3
    }
}