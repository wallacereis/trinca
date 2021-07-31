using ChurrasTrincaAPP.ViewModels.Home;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ChurrasTrincaAPP.Views.Home
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePageView : ContentPage
    {
        public HomePageView()
        {
            InitializeComponent();
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await (BindingContext as HomePageViewModel)?.LoadEventsAsync();
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }
    }
}