using Acr.UserDialogs;
using ChurrasTrincaAPP.Interfaces;
using ChurrasTrincaAPP.Models;
using ChurrasTrincaAPP.ViewModels.Base;
using ChurrasTrincaAPP.ViewModels.Modal;
using System;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace ChurrasTrincaAPP.ViewModels.Cadastros
{
    sealed class EditarParticipanteViewModel : BasePageViewModel
    {
        #region [Properties]
        /*---------------------- NomeParticipante Properties ----------------------*/
        private string _nomeParticipante;
        public string NomeParticipante
        {
            get { return _nomeParticipante; }
            set { SetProperty(ref _nomeParticipante, value); }
        }
        /*---------------------- ValorPago Properties ----------------------*/
        private decimal _valorPago;
        public decimal ValorPago
        {
            get { return _valorPago; }
            set { SetProperty(ref _valorPago, value); }
        }
        /*---------------------- IsConfirmed Properties ----------------------*/
        private bool _isConfirmed;
        public bool IsConfirmed
        {
            get { return _isConfirmed; }
            set { SetProperty(ref _isConfirmed, value); }
        }

        private Credential _credential;
        private BBQ _bbq;
        private Participant _participant;

        private readonly IFlurlAPI<Participant> _flurlAPI;
        private readonly IDialogService _dialogService;

        public IAsyncCommand ConfirmCommand { get; }
        #endregion

        public EditarParticipanteViewModel()
        {
            _flurlAPI = DependencyService.Get<IFlurlAPI<Participant>>();
            _dialogService = DependencyService.Get<IDialogService>();

            ConfirmCommand = new AsyncCommand(ExecuteConfirmCommandAsync, allowsMultipleExecutions: false);
        }

        public override async Task Initialize(params object[] args)
        {
            await base.Initialize(args);
            //*------------- Load Parameters -------------*/
            _credential = (Credential)args[0];
            _bbq = (BBQ)args[1];
            _participant = (Participant)args[2];
            //*------------- Load Participant -------------*/
            NomeParticipante = _participant.name;
            IsConfirmed = _participant.confirmed;
            ValorPago = _participant.value_paid;
        }

        private async Task ExecuteConfirmCommandAsync()
        {
            if (!InternetConnectionActive())
            {
                await NavigationService.Navigate<SemConexaoViewModel>();
                return;
            }
            if (!ValidaFormulario())
            {
                //Toast Messages
                _dialogService.ShowToast("Por favor, informe o nome do Participante!");
                return;
            }
            await UpdateParticipantAsync();
        }

        private async Task UpdateParticipantAsync()
        {
            if (!IsBusy)
            {
                Exception Error = null;
                try
                {
                    IsBusy = true;
                    //Verificar se o Usuário está cadastrado via Backend
                    using (UserDialogs.Instance.Loading("Atualizando...", null, null, true, MaskType.Gradient))
                    {
                        _participant.bbq_id = _bbq.id;
                        _participant.name = NomeParticipante;
                        _participant.confirmed = IsConfirmed;
                        _participant.value_paid = ValorPago;
                        var result = await _flurlAPI.UpdateItemAsync("participant", _participant.id, _participant, _credential.Token);
                    }
                }
                catch (Exception ex)
                {
                    IsBusy = false;
                    Error = ex;
                }
                finally
                {
                    IsBusy = false;
                    await NavigationService.Navigate<MensagemViewModel>(_credential, _bbq, "Participante atualizado com sucesso!");
                }
                if (Error is not null)
                {
                    IsBusy = false;
                    await DisplayAlert("Atenção!", "Ocorreu algo inesperado!" + Environment.NewLine + "Por favor, tente novamente!", "OK");
                }
            }
        }

        private bool ValidaFormulario()
        {
            if (String.IsNullOrWhiteSpace(NomeParticipante.Trim()))
                return false;
            return true;
        }
    }
}