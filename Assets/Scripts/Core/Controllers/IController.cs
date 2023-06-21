using System;
using Cysharp.Threading.Tasks;

namespace Core.Controllers
{
    public interface IController : IDisposable
    {
        UniTask Run(object param);
        UniTask Stop();
    }
}