using ChurrasTrincaAPP.Models;
using ChurrasTrincaAPP.ViewModels.Eventos;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ChurrasTrincaAPP.Views.Eventos
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetalhesBBQView : ContentPage
    {
        public DetalhesBBQView()
        {
            InitializeComponent();
        }

        private async void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            var _participant = (sender as ListView)?.SelectedItem as Participant;
            //------------------------- Action Sheet Config -------------------------
            var actionSheet = await DisplayActionSheet("O que deseja fazer?", "Cancelar", null, "Editar Participante", "Remover Participante");
            switch (actionSheet)
            {
                case "Cancelar":
                    IsBusy = false;
                    await (BindingContext as DetalhesBBQViewModel)?.RefreshCommand.ExecuteAsync();
                    break;

                case "Editar Participante":
                    await (BindingContext as DetalhesBBQViewModel)?.EditCommand.ExecuteAsync(_participant);
                    break;

                case "Remover Participante":
                    await (BindingContext as DetalhesBBQViewModel)?.RemoveCommand.ExecuteAsync(_participant);
                    break;
            }
            ((ListView)sender).SelectedItem = null;
        }
    }
}