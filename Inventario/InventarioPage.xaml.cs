using Xamarin.Forms;

namespace Inventario
{
    public partial class InventarioPage : ContentPage
    {
        public InventarioPage()
        {
            InitializeComponent();
        }

        void agregar_Clicked(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new RegistProdPage());
        }

        void cantidad_Clicked(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new CantProdPage());
        }
    }
}
