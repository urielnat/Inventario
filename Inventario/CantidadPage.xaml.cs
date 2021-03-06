﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using GetRest;
using Inventario.modelos;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace Inventario
{
    public partial class CantidadPage : ContentPage
    {
		public static List<Categoria> categoria;
		public producto prod { set; get; }
		public CantidadPage(producto pd)
        {
            prod = pd;
            InitializeComponent();
            iniciar();
            cantidad.TextChanged += OnTextChanged;
            BindingContext= pd;
        }

        async private void iniciar()
        {
            CLienteRest client = new CLienteRest();
			var httpclient = await client.Get<Categorias>("http://192.168.0.29:8080/categoria/leer");
			categoria = new List<Categoria>();
			if (httpclient != null)
			{

				foreach (var dato in httpclient.data)
				{
					categoria.Add(new Categoria
                    {    id= (dato as Categoria).id,
						nombre = (dato as Categoria).nombre,
						descripcion = (dato as Categoria).descripcion
					});
				}

				
			}
        }

        async void Handle_Clicked(object sender, System.EventArgs e)
        {
            if (CrossConnectivity.Current.IsConnected){
                if (!string.IsNullOrEmpty(cantidad.Text))
                {

                    var x = categoria.Where(nom => nom.nombre.Contains(prod.categoria));
                    //  DisplayAlert("ok",(x.ElementAt(0).id).ToString(),"ok");   


                    HttpResponseMessage response;
                    string sUrl = "http://192.168.0.29:8080/producto/modificar";
                    string sContentType = "application/json"; // or application/xml



                    List<ProdCat> produ = new List<ProdCat>{
                new ProdCat{
                    codigoBarras = prod.codigoBarras,
                    nombre = prod.nombre,
                    descripcion=prod.descripcion,
                    cantidad= int.Parse(cantidad.Text),
                    precioCompra =prod.precioCompra,
                    precioVenta =prod.precioVenta,
                    categoria =x.ElementAt(0).id


                    }

                   };

                    var jsonstring = JsonConvert.SerializeObject(produ);

                    jsonstring = jsonstring.Substring(1, jsonstring.Length - 2);
                    System.Diagnostics.Debug.WriteLine(jsonstring);


                    HttpClient oHttpClient = new HttpClient();
                    response = await oHttpClient.PutAsync(sUrl, new StringContent(jsonstring, Encoding.UTF8, sContentType));

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        System.Diagnostics.Debug.WriteLine("SE GUARDO");
                        await DisplayAlert("Guardar", "¡Producto modificado con exito!", "ok");
                        MessagingCenter.Send(this, "change");
                        await Navigation.PopModalAsync();
                    }
                    else
                    {
                        await DisplayAlert("Error", "El producto no se pudo modificar", "ok");

                    }
                }
            }else{await DisplayAlert("Error de conexion", "No hay coneccion a internet", "ok");
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
    }
}
