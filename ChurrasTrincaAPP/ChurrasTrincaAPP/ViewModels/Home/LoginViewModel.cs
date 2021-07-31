using Acr.UserDialogs;
using ChurrasTrincaAPP.Helpers;
using ChurrasTrincaAPP.Interfaces;
using ChurrasTrincaAPP.Models;
using ChurrasTrincaAPP.Services.Http;
using ChurrasTrincaAPP.ViewModels.Base;
using ChurrasTrincaAPP.ViewModels.Home;
using ChurrasTrincaAPP.ViewModels.Modal;
using Flurl;
using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace ChurrasTrincaAPP.ViewModels
{
    sealed class LoginViewModel : BasePageViewModel
    {
        #region [Properties]
        private readonly IFlurlAPI<Credential> _flurlCredentialAPI;
        private readonly IDialogService _dialogService;

        public IAsyncCommand LoginCommand { get; }
        public Credential CredentialModel { get; set; }

        #endregion

        public LoginViewModel()
        {
            _flurlCredentialAPI = DependencyService.Get<IFlurlAPI<Credential>>();
            _dialogService = DependencyService.Get<IDialogService>();

            LoginCommand = new AsyncCommand(ExecuteLoginCommandAsync, allowsMultipleExecutions: false);

            CredentialModel = new Credential
            {
                Username = string.Empty,
                Password = string.Empty
            };
        }

        private async Task ExecuteLoginCommandAsync()
        {
            if (!InternetConnectionActive())
            {
                await NavigationService.Navigate<SemConexaoViewModel>();
                return;
            }
            if (!ValidaLogin())
            {
                //Toast Messages
                _dialogService.ShowToast("Por favor, informe os campos para Login!");
                return;
            }
            await LoginUsuarioAsync();
        }

        private async Task LoginUsuarioAsync()
        {
            if (!IsBusy)
            {
                Exception Error = null;
                try
                {
                    IsBusy = true;
                    //Verificar se o Usuário está cadastrado via Backend
                    using (UserDialogs.Instance.Loading("Autenticando...", null, null, true, MaskType.Gradient))
                    {
                        var data = new Dictionary<string, object>
                        {
                            {"username", CredentialModel.Username.ToString().Trim()},
                            {"password", CredentialModel.Password.ToString().Trim()},
                        };

                        /*------------- Get Credentials -------------*/
                        var result = await Constants.BaseAddressAPI
                            .AppendPathSegment("auth")
                            .WithHeader("Accept", "application/json")
                            .AllowHttpStatus("400-500")
                            .WithTimeout(TimeSpan.FromMinutes(2))
                            .PostJsonAsync(data)
                            .ReceiveJson<Credential>();

                        /*------------- Valid Login -------------*/
                        if (string.IsNullOrWhiteSpace(result.Token))
                        {
                            //Toast Messages
                            _dialogService.ShowToast("Login ou Senha inválidos!");
                            return;
                        }

                        await NavigationService.Navigate<HomePageViewModel>(result);
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

        /*---------------------- ValidaLogin ----------------------*/
        private bool ValidaLogin()
        {
            if (String.IsNullOrWhiteSpace(CredentialModel.Username.Trim()))
                return false;
            if (String.IsNullOrWhiteSpace(CredentialModel.Password.Trim()))
                return false;
            return true;
        }

    }
}
