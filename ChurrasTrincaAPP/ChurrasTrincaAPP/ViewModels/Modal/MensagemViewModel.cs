using ChurrasTrincaAPP.Models;
using ChurrasTrincaAPP.ViewModels.Base;
using ChurrasTrincaAPP.ViewModels.Eventos;
using ChurrasTrincaAPP.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;

namespace ChurrasTrincaAPP.ViewModels.Modal
{
    sealed class MensagemViewModel : BaseModalPageViewModel
    {
        #region [ Properties ]
        /*---------------------- Mensagem Properties ----------------------*/
        private string _mensagem;
        public string Mensagem
        {
            get { return _mensagem; }
            set { SetProperty(ref _mensagem, value); }
        }
        private Credential _credential;
        private BBQ _bbq;

        public IAsyncCommand CloseCommand { get; }
        #endregion

        public MensagemViewModel()
        {
            CloseCommand = new AsyncCommand(ExecuteCloseCommandAsync, allowsMultipleExecutions: false);
        }

        public override async Task Initialize(params object[] args)
        {
            await base.Initialize(args);
            //*------------- Load Parameters -------------*/
            _credential = (Credential)args[0];
            _bbq = (BBQ)args[1];
            Mensagem = (string)args[2].ToString().Trim();
        }

        private async Task ExecuteCloseCommandAsync()
        {
            Preferences.Clear();
            await App.Current.MainPage.Navigation.PopModalAsync();
            if(Mensagem.Contains("Participante"))
                await NavigationService.Navigate<DetalhesBBQViewModel>(_credential, _bbq);
            else
                await NavigationService.Navigate<HomePageViewModel>(_credential);
        }
    }
}
