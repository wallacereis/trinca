using Acr.UserDialogs;
using ChurrasTrincaAPP.Interfaces;
using ChurrasTrincaAPP.Models;
using ChurrasTrincaAPP.ViewModels.Base;
using ChurrasTrincaAPP.ViewModels.Modal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace ChurrasTrincaAPP.ViewModels.Cadastros
{
    sealed class RegistrarBBQViewModel : BasePageViewModel
    {
        #region [Properties]
        /*---------------------- ValorPessoa Properties ----------------------*/
        private decimal _valorPessoa;
        public decimal ValorPessoa
        {
            get { return _valorPessoa; }
            set { SetProperty(ref _valorPessoa, value); }
        }
        private Credential _credential;
        
        private readonly IFlurlAPI<BBQ> _flurlAPI;
        private readonly IDialogService _dialogService;

        public IAsyncCommand ConfirmCommand { get; }
        public BBQ BBQModel { get; set; }
        #endregion

        public RegistrarBBQViewModel()
        {
            _flurlAPI = DependencyService.Get<IFlurlAPI<BBQ>>();
            _dialogService = DependencyService.Get<IDialogService>();

            ConfirmCommand = new AsyncCommand(ExecuteConfirmCommandAsync, allowsMultipleExecutions: false);

            BBQModel = new BBQ
            {
                title = string.Empty,
                description = string.Empty,
                date = DateTime.Now,
                value_per_person = 0
            };
        }

        public override async Task Initialize(params object[] args)
        {
            await base.Initialize(args);
            //*------------- Load Parameters -------------*/
            _credential = (Credential)args[0];
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
            await RegisterBBQAsync();
        }

        private async Task RegisterBBQAsync()
        {
            if (!IsBusy)
            {
                Exception Error = null;
                try
                {
                    IsBusy = true;
                    //Verificar se o Usuário está cadastrado via Backend
                    using (UserDialogs.Instance.Loading("Registrando...", null, null, true, MaskType.Gradient))
                    {
                        BBQ objBBQ = new BBQ
                        {
                            title = BBQModel.title,
                            description = BBQModel.description,
                            date = BBQModel.date,
                            value_per_person = ValorPessoa
                        };
                        var result = await _flurlAPI.AddItemAsync(string.Empty, objBBQ, _credential.Token);
                        await NavigationService.Navigate<MensagemViewModel>(_credential, result, "BBQ criado com sucesso!");
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
            if (String.IsNullOrWhiteSpace(BBQModel.title.Trim()))
                return false;
            if (String.IsNullOrWhiteSpace(BBQModel.description.Trim()))
                return false;
            if (ValorPessoa == 0)
                return false;
            return true;
        }
    }
}
