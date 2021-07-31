using ChurrasTrincaAPP.ViewModels;
using ChurrasTrincaAPP.ViewModels.Base;
using ChurrasTrincaAPP.ViewModels.Cadastros;
using ChurrasTrincaAPP.ViewModels.Eventos;
using ChurrasTrincaAPP.ViewModels.Home;
using ChurrasTrincaAPP.ViewModels.Modal;
using ChurrasTrincaAPP.Views.Cadastros;
using ChurrasTrincaAPP.Views.Eventos;
using ChurrasTrincaAPP.Views.Home;
using ChurrasTrincaAPP.Views.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ChurrasTrincaAPP.Services.Navigation
{
    sealed class NavigationService
    {
        #region [ Properties ]
        private static Lazy<NavigationService> _lazy = new Lazy<NavigationService>(() => new NavigationService());
        private readonly Dictionary<Type, Type> _mappings;
        public static NavigationService Current { get => _lazy.Value; }
        #endregion

        private NavigationService()
        {
            _mappings = new Dictionary<Type, Type>();
            CreateViewModelMappings();
        }

        private void CreateViewModelMappings()
        {
            //---------------- Rotas Home Pages ----------------//
            _mappings.Add(typeof(LoginViewModel), typeof(LoginView));
            _mappings.Add(typeof(HomePageViewModel), typeof(HomePageView));

            //---------------- Rotas Cadastro ----------------// 
            _mappings.Add(typeof(RegistrarBBQViewModel), typeof(RegistrarBBQView));
            _mappings.Add(typeof(RegistrarParticipanteViewModel), typeof(RegistrarParticipanteView));
            _mappings.Add(typeof(EditarBBQViewModel), typeof(EditarBBQView));
            _mappings.Add(typeof(EditarParticipanteViewModel), typeof(EditarParticipanteView));

            //---------------- Rotas Operações ----------------//
            _mappings.Add(typeof(DetalhesBBQViewModel), typeof(DetalhesBBQView));

            //---------------- Rotas Modal Pages ----------------//
            _mappings.Add(typeof(MensagemViewModel), typeof(MensagemView));
            _mappings.Add(typeof(SemConexaoViewModel), typeof(SemConexaoView));
        }

        private Application CurrentApplication => Application.Current;

        private INavigation Navigation { get => ((CustomNavigationPage)CurrentApplication.MainPage).Navigation; }

        internal Task Navigate<TViewModel>(params object[] args)
            where TViewModel : BasePageViewModel
            => InternalNavigate(typeof(TViewModel), args);

        private async Task InternalNavigate(Type viewModelType, params object[] args)
        {
            try
            {
                Page page = null;

                // Verificar se estou indo para a página inicial...
                if (viewModelType == typeof(HomePageViewModel))
                {
                    // Se a página inicial não foi carregada...
                    if (!Navigation.NavigationStack.Any(lbda => lbda.BindingContext.GetType() == typeof(HomePageViewModel)))
                    {
                        page = CreateAndBindPage(viewModelType);

                        var pagesToRemove = Navigation.NavigationStack.ToList();

                        await Navigation.PushAsync(page);

                        foreach (var pageToRemove in pagesToRemove)
                        {
                            Navigation.RemovePage(pageToRemove);
                        }
                    }
                    else
                        await GoBack(toRoot: true);
                }
                else
                {
                    page = CreateAndBindPage(viewModelType);

                    if (viewModelType.BaseType == typeof(BaseModalPageViewModel))
                        await Navigation.PushModalAsync(page, true);
                    else
                        await Navigation.PushAsync(page, true);
                }

                if (!(page is null))
                    await (page.BindingContext as BasePageViewModel).Initialize(args);
            }
            catch
            {
                throw;
            }
        }

        public Task GoBack(bool toRoot = false, bool animated = true)
        {
            if (toRoot)
                return Navigation.PopToRootAsync(animated);

            if (Navigation.ModalStack.Count > 0)
                return Navigation.PopModalAsync(animated);

            return Navigation.PopAsync(animated);
        }

        private Type GetPageTypeForViewModel(Type viewModelType)
        {
            try
            {
                if (!_mappings.ContainsKey(viewModelType))
                    throw new KeyNotFoundException($"No map for ${viewModelType} was found on navigation mappings");

                return _mappings[viewModelType];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private Page CreateAndBindPage(Type viewModelType)
        {
            try
            {
                Type pageType = GetPageTypeForViewModel(viewModelType);

                if (pageType == null)
                    throw new Exception($"Mapping type for {viewModelType} is not a page");

                Page page = Activator.CreateInstance(pageType) as Page;
                page.BindingContext = Activator.CreateInstance(viewModelType) as BasePageViewModel;

                return page;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal void Initialize(params object[] args)
        {
            //---------------- Custom Navigation Page ----------------//
            CurrentApplication.MainPage = new CustomNavigationPage();

            Device.BeginInvokeOnMainThread(async () => await Navigate<LoginViewModel>());
        }
    }
}