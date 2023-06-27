using Core.Views;
using Core.Views.ProgressBars;
using Cysharp.Threading.Tasks;

namespace Modules.StartGame.Scripts
{
    public class StartGameUIView : UIView
    {
        public ProgressBarView progressBarView;
        
        public override UniTask Show()
        {
            return UniTask.CompletedTask;
        }
    }
}