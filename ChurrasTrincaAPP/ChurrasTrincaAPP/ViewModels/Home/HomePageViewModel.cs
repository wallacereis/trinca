using Acr.UserDialogs;
using ChurrasTrincaAPP.Interfaces;
using ChurrasTrincaAPP.Models;
using ChurrasTrincaAPP.ViewModels.Base;
using ChurrasTrincaAPP.ViewModels.Cadastros;
using ChurrasTrincaAPP.ViewModels.Eventos;
using ChurrasTrincaAPP.ViewModels.Modal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace ChurrasTrincaAPP.ViewModels.Home
{
    sealed class HomePageViewModel : BasePageViewModel
    {
        #region [ Properties ]
        /*---------------------- BBQs Properties ----------------------*/
        private ObservableCollection<BBQ> _bbq;
        public ObservableCollection<BBQ> BBQs
        {
            get { return _bbq; }
            set { SetProperty(ref _bbq, value); }
        }

        private Credential _credential;

        private readonly IFlurlAPI<Root> _flurlAPI;
        private readonly IFlurlAPI<BBQ> _flurlBBQAPI;
        private readonly IDialogService _dialogService;

        public IAsyncCommand RegisterBBQCommand { get; }
        public IAsyncCommand RefreshCommand { get; }

        private AsyncCommand<BBQ> _SelectCommand;
        public AsyncCommand<BBQ> SelectCommand
            => _SelectCommand
            ??= new AsyncCommand<BBQ>(
                execute: SelectCommandExecute,
                canExecute: SelectCommandCanExecute,
                onException: CommandOnException);

        private bool SelectCommandCanExecute(object arg) => true;

        private List<BBQ> _listaBBQ = new List<BBQ>();

        private static Page CurrentMainPage { get { return Application.Current.MainPage; } }

        #endregion

        public HomePageViewModel()
        {
            _flurlAPI = DependencyService.Get<IFlurlAPI<Root>>();
            _flurlBBQAPI = DependencyService.Get<IFlurlAPI<BBQ>>();
            _dialogService = DependencyService.Get<IDialogService>();

            RegisterBBQCommand = new AsyncCommand(ExecuteRegisterBBQCommandAsync, allowsMultipleExecutions: false);
            RefreshCommand = new AsyncCommand(ExecuteRefreshCommandAsync, allowsMultipleExecutions: false);
        }

        public override async Task Initialize(params object[] args)
        {
            await base.Initialize(args);
            //*------------- Load Parameters -------------*/
            _credential = (Credential)args[0];
            //*------------- Carregar Eventos BBQ -------------*/
            IsBusy = false;
            await LoadEventsAsync();
        }

        private async Task SelectCommandExecute(BBQ obj)
        {
            if (!InternetConnectionActive())
            {
                await NavigationService.Navigate<SemConexaoViewModel>();
                return;
            }
            //------------------------- Action Sheet Config -------------------------
            var actionSheet = await CurrentMainPage.DisplayActionSheet("O que deseja fazer?", "Cancelar", null, "Ver Detalhes", "Adicionar Participantes", "Editar BBQ", "Remover BBQ");
            switch (actionSheet)
            {
                case "Cancelar":
                    IsBusy = false;
                    await LoadEventsAsync();
                    break;

                case "Ver Detalhes":
                    await NavigationService.Navigate<DetalhesBBQViewModel>(_credential, obj);
                    break;

                case "Adicionar Participantes":
                    await NavigationService.Navigate<RegistrarParticipanteViewModel>(_credential, obj);
                    break;

                case "Editar BBQ":
                    await NavigationService.Navigate<EditarBBQViewModel>(_credential, obj);
                    break;

                case "Remover BBQ":
                    await ExecuteRemoveBBQAsync(obj);
                    break;
            }
        }

        private async Task ExecuteRegisterBBQCommandAsync()
        {
            if (!InternetConnectionActive())
            {
                await NavigationService.Navigate<SemConexaoViewModel>();
                return;
            }
            await NavigationService.Navigate<RegistrarBBQViewModel>(_credential);
        }

        private async Task ExecuteRemoveBBQAsync(BBQ obj)
        {
            var remove = await DisplayAlert("Atenção!", "Excluir BBQ :( ?", "Sim", "Não");
            if (remove)
            {
                //*------------- Remover BBQ -------------*/
                await _flurlAPI.DeleteItemAsync("", obj.id, _credential.Token);
                //Toast Messages
                _dialogService.ShowToast("BBQ removido com sucesso (: !");
                IsBusy = false;
                await LoadEventsAsync();
            }
        }

        private async Task ExecuteRefreshCommandAsync()
        {
            IsRefresh = false;
            await LoadEventsAsync();
        }

        /*---------------------- Load Events Async ----------------------*/
        public async Task LoadEventsAsync()
        {
            if (!InternetConnectionActive())
            {
                await NavigationService.Navigate<SemConexaoViewModel>();
                return;
            }
            if (!IsBusy)
            {
                Exception Error = null;
                try
                {
                    IsBusy = true;
                    IsRefresh = true;
                    _listaBBQ.Clear();
                    var result = await _flurlAPI.GetItemsDynamicAsync(_credential.Token);
                    foreach (var item in result.data)
                    {
                        var bbq = await _flurlBBQAPI.GetItemByIdAsync(string.Empty, item.id, _credential.Token);
                        _listaBBQ.Add(bbq);
                    }
                    BBQs = new ObservableCollection<BBQ>(_listaBBQ);
                }
                catch (Exception ex)
                {
                    IsBusy = false;
                    IsRefresh = false;
                    Error = ex;
                }
                finally
                {
                    IsBusy = false;
                    IsRefresh = false;
                }
                if (Error is not null)
                {
                    IsBusy = false;
                    IsRefresh = false;
                    await DisplayAlert("Ooops!", "Ocorreu algo inesperado!" + Environment.NewLine + "Por favor, tente novamente!", "OK");
                }
            }
        }
    }
}
