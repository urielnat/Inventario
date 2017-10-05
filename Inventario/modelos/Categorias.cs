using System;
using System.Collections.Generic;

namespace Inventario.modelos
{
    public class Categoria
    {
        public int id { get; set; }
        public string nombre { get; set;}
        public string descripcion { get; set;}
    }

	public class Categorias
	{
        public IList<Categoria> data { get; set; }
		public int code { get; set; }
	}
}
