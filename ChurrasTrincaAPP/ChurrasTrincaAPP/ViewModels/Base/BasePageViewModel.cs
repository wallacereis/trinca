using System.Threading.Tasks;

namespace ChurrasTrincaAPP.ViewModels.Base
{
    abstract class BasePageViewModel : BaseViewModel
    {
        public virtual Task Initialize(params object[] args) => Task.CompletedTask;
    }
}
