using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using AppBarCode.servicios;
using GetRest;
using Inventario.modelos;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace Inventario
{


    public partial class RegistProdPage : ContentPage
    {
        public static List<Categoria> categoria;
       
        public RegistProdPage()
        {
          
            InitializeComponent();

            etCodigo.TextChanged += OnTextChanged;
            etCantidad.TextChanged += OnTextChanged;
            llenarPicker();


        }

       async void guardar_Clicked(object sender, System.EventArgs e)
        {
			var auth = false;
			string compra="", venta="";
            if(string.IsNullOrEmpty(etCodigo.Text)||etCodigo.Text.Length<13){
                txtcod.TextColor = Color.Red;
                auth = false;
               
			}
			else
			{
                txtcod.TextColor = Color.Black;
                etCodigo.IsEnabled = false;
				auth = true;
			}
            if (string.IsNullOrEmpty(etNombre.Text))
			{
                txtnom.TextColor = Color.Red;
				auth = false;
				
			}
			else
            {
                etNombre.IsEnabled = false;
                txtnom.TextColor = Color.Black;
				auth = true;
			}
            if (string.IsNullOrEmpty(etDescripcion.Text))
			{
                txtdesc.TextColor = Color.Red;
				auth = false;

			}
			else
			{
                etDescripcion.IsEnabled=false;
                txtdesc.TextColor = Color.Black;
				auth = true;
			}

            if(string.IsNullOrEmpty(etPrecioVenta.Text)){
                txtPrev.TextColor = Color.Red;
				auth = false;
				
            }else{
                etPrecioVenta.IsEnabled = false;
				double ventad = double.Parse(etPrecioVenta.Text);
                ventad = Math.Round(ventad, 2);
                venta =string.Format("{0:0.00}", ventad);
                System.Diagnostics.Debug.WriteLine(string.Format("{0:0.00}", venta));
				txtPrev.TextColor = Color.Black;
                auth = true;
            }
            if (string.IsNullOrEmpty(etPrecioCompra.Text))
			{
                txtPrec.TextColor = Color.Red;
				auth = false;

			}
			else
            {
                etPrecioCompra.IsEnabled = false;
				double comprad = double.Parse(etPrecioVenta.Text);
				comprad = Math.Round(comprad, 2);
				compra = string.Format("{0:0.00}", comprad);
				System.Diagnostics.Debug.WriteLine(string.Format("{0:0.00}", compra));
				txtPrev.TextColor = Color.Black;
				auth = true;
			}

            if (string.IsNullOrEmpty(etCantidad.Text))
			{
                txtcant.TextColor = Color.Red;
				auth = false;

			}
			else
			{
                etCantidad.IsEnabled = false;  
                txtcant.TextColor = Color.Black;
				auth = true;
			}
            if(!(etCategoria.SelectedIndex>-1)){
                txtcat.TextColor = Color.Red;
                auth = false;
            }else{
                etCategoria.IsEnabled = false;
                txtcat.TextColor = Color.Black;
				auth = true;
            }

            if (auth)
            {

                if (CrossConnectivity.Current.IsConnected)
                { 


                HttpResponseMessage response;
                string sUrl = "http://192.168.0.29:8080/producto/insertar";
                string sContentType = "application/json"; // or application/xml


                System.Diagnostics.Debug.WriteLine("###########" + compra);
                List<producto> prod = new List<producto>{
                    new producto{
                        codigoBarras = etCodigo.Text,
                        nombre = etNombre.Text,
                        descripcion=etDescripcion.Text,
                        cantidad= int.Parse(etCantidad.Text),
                        precioCompra =double.Parse(compra),
                        precioVenta =double.Parse(venta),
                        categoria =etCategoria.Items[etCategoria.SelectedIndex]


                    }

                   };

                var jsonstring = JsonConvert.SerializeObject(prod);

                jsonstring = jsonstring.Substring(1, jsonstring.Length - 2);
                System.Diagnostics.Debug.WriteLine(jsonstring);


                HttpClient oHttpClient = new HttpClient();
                response = await oHttpClient.PostAsync(sUrl, new StringContent(jsonstring, Encoding.UTF8, sContentType));
                var oTaskPostAsync = oHttpClient.PostAsync(sUrl, new StringContent(jsonstring, Encoding.UTF8, sContentType));

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    System.Diagnostics.Debug.WriteLine("SE GUARDO");
                    await DisplayAlert("Guardar", "¡Producto creado con exito!", "ok");
                    await Navigation.PopToRootAsync();
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotModified)
                {
                    await DisplayAlert("Error", "El producto ya existe", "ok");

                }
                }else{
                    await DisplayAlert("Error de conexion", "No hay coneccion a internet", "ok");
                }
              }
			}


	
		public void OnTextChanged(object sender, TextChangedEventArgs args)
		{
			if (!Regex.IsMatch(args.NewTextValue, "^[0-9]+$", RegexOptions.CultureInvariant))
				(sender as Entry).Text = Regex.Replace(args.NewTextValue, "[^0-9]", string.Empty);
			Entry entry = sender as Entry;
			String val = entry.Text; 

			if (val.Length > 13)
			{
				val = val.Remove(val.Length - 1); 
				entry.Text = val; 
			}
		}

       async void escaner_Clicked(object sender, System.EventArgs e)
        {
			var scanner = DependencyService.Get<IQrCodeScanningService>();
			var result = await scanner.ScanAsync();
			if (result != null)
                etCodigo.Text = result;
        }

        void llenarPicker(){
			Device.BeginInvokeOnMainThread(async () =>
            {
                if (CrossConnectivity.Current.IsConnected) {  
                CLienteRest client = new CLienteRest();
                var httpclient = await client.Get<Categorias>("http://192.168.0.29:8080/categoria/leer");
                categoria = new List<Categoria>();
                if (httpclient != null)
                {

                    foreach (var dato in httpclient.data)
                    {
                        categoria.Add(new Categoria
                        {
                            nombre = (dato as Categoria).nombre,
                            descripcion = (dato as Categoria).descripcion
                        });
                    }

                    foreach (var nom in categoria)
                    {
                        etCategoria.Items.Add(nom.nombre);
                    }
                }


				}
				else { await DisplayAlert("Error de conexion", "No hay coneccion a internet", "ok"); }

			});
        }

    }
     
    }

