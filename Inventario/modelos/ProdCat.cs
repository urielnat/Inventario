using System;
namespace Inventario.modelos
{
    public class ProdCat
    {
        

	    public string codigoBarras { get; set; }
		public string nombre { get; set; }
		public string descripcion { get; set; }
		public int cantidad { get; set; }
		public double precioCompra { get; set; }
		public double precioVenta { get; set; }
		public int categoria { get; set; }
       
    }
}
