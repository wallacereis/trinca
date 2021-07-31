using ChurrasTrincaAPP.Helpers;
using ChurrasTrincaAPP.Interfaces;
using ChurrasTrincaAPP.Models;
using ChurrasTrincaAPP.Services.Http;
using ChurrasTrincaAPP.Services.Message;
using ChurrasTrincaAPP.Services.Navigation;
using Flurl.Http;
using System;
using System.Globalization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ChurrasTrincaAPP
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            /*------------- Start Navigation Service -------------*/
            NavigationService.Current.Initialize();

            /*------------- SettingsPreferences Culture Info -------------*/
            var cultureInfo = new CultureInfo("pt-BR");
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;

            /*------------- Register Dependency Service -------------*/
            RegisterDependencyService();
        }

        /*------------- Register Dependency Service -------------*/
        private void RegisterDependencyService()
        {
            DependencyService.RegisterSingleton<IFlurlAPI<Credential>>(new FlurlAPI<Credential>());
            DependencyService.RegisterSingleton<IFlurlAPI<Root>>(new FlurlAPI<Root>());
            DependencyService.RegisterSingleton<IFlurlAPI<BBQ>>(new FlurlAPI<BBQ>());
            DependencyService.RegisterSingleton<IFlurlAPI<Participant>>(new FlurlAPI<Participant>());
            DependencyService.RegisterSingleton<IDialogService>(new DialogService());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            FlurlHttp.ConfigureClient
            (
                Constants.BaseAddressPDF, cli =>
                cli.Settings.HttpClientFactory = new UntrustedCertClientFactory()
            );
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
