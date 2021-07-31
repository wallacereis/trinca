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
    sealed class EditarBBQViewModel : BasePageViewModel
    {
        #region [Properties]
        /*---------------------- Titulo Properties ----------------------*/
        private string _titulo;
        public string Titulo
        {
            get { return _titulo; }
            set { SetProperty(ref _titulo, value); }
        }
        /*---------------------- Descricao Properties ----------------------*/
        private string _descricao;
        public string Descricao
        {
            get { return _descricao; }
            set { SetProperty(ref _descricao, value); }
        }
        /*---------------------- DataEvento Properties ----------------------*/
        private DateTime _dataEvento;
        public DateTime DataEvento
        {
            get { return _dataEvento; }
            set { SetProperty(ref _dataEvento, value); }
        }
        /*---------------------- ValorPessoa Properties ----------------------*/
        private decimal _valorPessoa;
        public decimal ValorPessoa
        {
            get { return _valorPessoa; }
            set { SetProperty(ref _valorPessoa, value); }
        }

        private Credential _credential;
        private BBQ _bbq;

        private readonly IFlurlAPI<BBQ> _flurlAPI;
        private readonly IDialogService _dialogService;

        public IAsyncCommand ConfirmCommand { get; }
        #endregion

        public EditarBBQViewModel()
        {
            _flurlAPI = DependencyService.Get<IFlurlAPI<BBQ>>();
            _dialogService = DependencyService.Get<IDialogService>();

            ConfirmCommand = new AsyncCommand(ExecuteConfirmCommandAsync, allowsMultipleExecutions: false);
        }

        public override async Task Initialize(params object[] args)
        {
            await base.Initialize(args);
            //*------------- Load Parameters -------------*/
            _credential = (Credential)args[0];
            _bbq = (BBQ)args[1];
            //*------------- Load BBQ -------------*/
            Titulo = _bbq.title;
            Descricao = _bbq.description;
            DataEvento = _bbq.date;
            ValorPessoa = _bbq.value_per_person;
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
                _dialogService.ShowToast("Por favor, informe todos os campos!");
                return;
            }
            await UpdateBBQAsync();
        }

        private async Task UpdateBBQAsync()
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
                        _bbq.title = Titulo;
                        _bbq.description = Descricao;
                        _bbq.date = DataEvento;
                        _bbq.value_per_person = ValorPessoa;
                        var result = await _flurlAPI.UpdateItemAsync("", _bbq.id, _bbq, _credential.Token);
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
                    await NavigationService.Navigate<MensagemViewModel>(_credential, _bbq, "BBQ atualizado com sucesso!");
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
            if (String.IsNullOrWhiteSpace(Titulo.Trim()))
                return false;
            if (String.IsNullOrWhiteSpace(Descricao.Trim()))
                return false;
            if (DataEvento == null)
                return false;
            if (ValorPessoa == 0)
                return false;
            return true;
        }
    }
}
