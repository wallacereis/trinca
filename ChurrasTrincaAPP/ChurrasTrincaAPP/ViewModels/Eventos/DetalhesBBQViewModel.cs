using ChurrasTrincaAPP.Interfaces;
using ChurrasTrincaAPP.Models;
using ChurrasTrincaAPP.ViewModels.Base;
using ChurrasTrincaAPP.ViewModels.Cadastros;
using ChurrasTrincaAPP.ViewModels.Home;
using ChurrasTrincaAPP.ViewModels.Modal;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace ChurrasTrincaAPP.ViewModels.Eventos
{
    sealed class DetalhesBBQViewModel : BasePageViewModel
    { 
        #region [Properties]
        /*---------------------- Participants Properties ----------------------*/
        private ObservableCollection<Participant> _participant;
        public ObservableCollection<Participant> Participants
        {
            get { return _participant; }
            set { SetProperty(ref _participant, value); }
        }
        /*---------------------- DataEvento Properties ----------------------*/
        private DateTime _dataEvento;
        public DateTime DataEvento
        {
            get { return _dataEvento; }
            set { SetProperty(ref _dataEvento, value); }
        }
        /*---------------------- Titulo Properties ----------------------*/
        private string _titulo;
        public string Titulo
        {
            get { return _titulo; }
            set { SetProperty(ref _titulo, value); }
        }
        /*---------------------- TotalParticipantes Properties ----------------------*/
        private int _totalParticipantes;
        public int TotalParticipantes
        {
            get { return _totalParticipantes; }
            set { SetProperty(ref _totalParticipantes, value); }
        }
        /*---------------------- ValorTotalPago Properties ----------------------*/
        private decimal _valorTotalPago;
        public decimal ValorTotalPago
        {
            get { return _valorTotalPago; }
            set { SetProperty(ref _valorTotalPago, value); }
        }

        private readonly IFlurlAPI<BBQ> _flurlAPI;
        private readonly IDialogService _dialogService;
        private BBQ _bbq;
        private Credential _credential;

        public IAsyncCommand<Participant> EditCommand { get; }
        public IAsyncCommand<Participant> RemoveCommand { get; }
        public IAsyncCommand ReturnCommand { get; }
        public IAsyncCommand RefreshCommand { get; }
        #endregion

        public DetalhesBBQViewModel()
        {
            _flurlAPI = DependencyService.Get<IFlurlAPI<BBQ>>();
            _dialogService = DependencyService.Get<IDialogService>();

            EditCommand = new AsyncCommand<Participant>((Participant obj) => ExecuteEditCommandAsync(obj), allowsMultipleExecutions: false);
            RemoveCommand = new AsyncCommand<Participant>((Participant obj) => ExecuteRemoveCommandAsync(obj), allowsMultipleExecutions: false);
            ReturnCommand = new AsyncCommand(ExecuteReturnCommandAsync, allowsMultipleExecutions: false);
            RefreshCommand = new AsyncCommand(ExecuteRefreshCommandAsync, allowsMultipleExecutions: false);
        }

        public override async Task Initialize(params object[] args)
        {
            await base.Initialize(args);
            //*------------- Load Parameters -------------*/
            _credential = (Credential)args[0];
            _bbq = (BBQ)args[1];
            //*------------- Carregar Detalhes BBQ -------------*/
            Titulo = _bbq.title;
            DataEvento = _bbq.date;
            //*------------- Carregar Participantes BBQ -------------*/
            IsBusy = false;
            await LoadParticipantsAsync();
        }

        private async Task ExecuteEditCommandAsync(Participant obj)
        {
            if (!InternetConnectionActive())
            {
                await NavigationService.Navigate<SemConexaoViewModel>();
                return;
            }
            await NavigationService.Navigate<EditarParticipanteViewModel>(_credential, _bbq, obj);
        }

        private async Task ExecuteRemoveCommandAsync(Participant obj)
        {
            var remove = await DisplayAlert("Atenção!", "Excluir Participante :( ?", "Sim", "Não");
            if (remove)
            {
                //*------------- Remover Participante -------------*/
                await _flurlAPI.DeleteItemAsync("participant", obj.id, _credential.Token);
                //Toast Messages
                _dialogService.ShowToast("Participante removido com sucesso (: !");
                IsBusy = false;
                await LoadParticipantsAsync();
            }
        }

        private async Task ExecuteReturnCommandAsync()
        {
            if (!InternetConnectionActive())
            {
                await NavigationService.Navigate<SemConexaoViewModel>();
                return;
            }
            await NavigationService.Navigate<HomePageViewModel>(_credential);
        }

        private async Task ExecuteRefreshCommandAsync()
        {
            IsRefresh = false;
            await LoadParticipantsAsync();
        }

        /*---------------------- Load Participants Async ----------------------*/
        private async Task LoadParticipantsAsync()
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
                    var result = await _flurlAPI.GetItemByIdAsync(string.Empty, _bbq.id, _credential.Token);
                    Participants = new ObservableCollection<Participant>(result.Participants);
                    TotalParticipantes = Participants.Count;
                    ValorTotalPago = Participants.Select(p => p.value_paid).Sum();
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