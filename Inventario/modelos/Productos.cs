using System;
using System.Collections.Generic;

namespace Inventario.modelos
{
		public class producto
		{
			public string codigoBarras { get; set; }
			public string nombre { get; set; }
			public string descripcion { get; set; }
			public int cantidad { get; set; }
            public double precioCompra { get; set; }
            public double precioVenta { get; set; }
            public string categoria { get; set; }
		}

		public class Productos
		{
			public IList<producto> data { get; set; }
			public int code { get; set; }
		}
    
}
