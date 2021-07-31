using ChurrasTrincaAPP.ViewModels.Base;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;

namespace ChurrasTrincaAPP.ViewModels.Modal
{
    sealed class SemConexaoViewModel : BaseModalPageViewModel
    {
        #region [ Properties ]
        public IAsyncCommand TentarNovamenteCommand { get; }
        #endregion

        public SemConexaoViewModel()
        {
            TentarNovamenteCommand = new AsyncCommand(ExecuteTentarNovamenteCommandAsync, allowsMultipleExecutions: false);
        }

        private async Task ExecuteTentarNovamenteCommandAsync()
        {
            if (!InternetConnectionActive())
                return;
            await NavigationService.GoBack(false, true);
        }

    }
}
