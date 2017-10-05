using System;
using System.Collections.Generic;
using System.Linq;
using GetRest;
using Inventario.modelos;
using Xamarin.Forms;

namespace Inventario
{
    public partial class CantProdPage : ContentPage
    {
		public static List<producto> list;
        public CantProdPage()
        {
            InitializeComponent();
            llenarLista();
			conectar.Clicked += (sender, e) =>
			{
                llenarLista();

			};
        }
		void Handle_SearchButtonPressed(object sender, System.EventArgs e)
		{
			string palabra = buscar.Text;

			if (string.IsNullOrEmpty(palabra))
			{
				lista.ItemsSource = list;
				System.Diagnostics.Debug.WriteLine("entro");
			}
			else
			{
                IEnumerable<producto> resultado = list.Where(nom => nom.nombre.ToLower().Contains(palabra.ToLower()));
                var x=list.Where(nom => nom.nombre.ToLower().Contains(palabra.ToLower()));
                //DisplayAlert("algo", x.ElementAt(0).descripcion,"ok");
                lista.ItemsSource = resultado; 
				palabra = string.Empty;
			}
		}


         void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            //await DisplayAlert("algo",(e.SelectedItem as producto).descripcion,"ok");
            var unproducto = (producto)e.SelectedItem;
            //Device.BeginInvokeOnMainThread(async () => await Navigation.PopAsync());
           
            Navigation.PushModalAsync(new CantidadPage(unproducto));
            Navigation.PopToRootAsync();
        }

        void llenarLista(){
			Device.BeginInvokeOnMainThread(async () =>
			{
				carga.IsVisible = true;

				CLienteRest client = new CLienteRest();
				var httpclient = await client.Get<Productos>("http://192.168.0.29:8080/producto/leer"); //revizar ip
				list = new List<producto>();
				if (httpclient != null)
				{
					carga.IsVisible = false;
					foreach (var dato in httpclient.data)
					{
						list.Add(new producto
						{
							codigoBarras = (dato as producto).codigoBarras,
							nombre = (dato as producto).nombre,
							descripcion = (dato as producto).descripcion,
							cantidad = (dato as producto).cantidad,
							precioCompra = (dato as producto).precioCompra,
							precioVenta = (dato as producto).precioVenta,
							categoria = (dato as producto).categoria


						});



					}
					buscar.IsEnabled = true;
				}
				var image = new Image { Source = "tag2.png" };
				lista.ItemTemplate = new DataTemplate(typeof(ImageCell));
				lista.ItemsSource = list;

				lista.ItemTemplate.SetBinding(TextCell.TextProperty, "nombre");
				lista.ItemTemplate.SetBinding(TextCell.DetailProperty, "cantidad");
				lista.ItemTemplate.SetValue(ImageCell.ImageSourceProperty, image.Source);



			});

		}
    }
}
