namespace App_VentasCompras.DTOs
{
    public class ProductoDTO
    {
        public int IDProducto { get; set; }
        public string NombreProducto { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public string Imagen { get; set; }

        //datos del usuario para su correcto contacto
        public int IDUsuario { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? NroCelular { get; set; }
        // si es null los datos ingresados, se pondra los datos del usuario actual por defecto,
        // es decir se pondra por defecto los datos del usuario que creo el producto

        public string NombreCategoria { get; set; } //se debera verificar si es null que no ingreso nada, si ingreso algo se verificara si existe la categoria con ese nombre
        public int IDCategoria { get; set; }

        public int IDProductoVenta { get; set; }
        public DateTime? Fecha { get; set; }  // la fecha es creada automatica al crear el producto
        public EstadoProducto EstadoProducto { get; set; } // listas
        public EstadoVenta EstadoVenta { get; set; } // listas
        public int Cantidad { get; set; }
    }
}
