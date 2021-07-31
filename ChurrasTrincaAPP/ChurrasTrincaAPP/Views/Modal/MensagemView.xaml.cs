using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ChurrasTrincaAPP.Views.Modal
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MensagemView : ContentPage
    {
        public MensagemView()
        {
            InitializeComponent();
        }
        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}