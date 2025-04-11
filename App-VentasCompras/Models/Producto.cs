namespace App_VentasCompras.Models
{
    public class Producto
    {
        public int IDProducto { get; set; }
        public string NombreProducto { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public string Imagen { get; set; }

       public int IDUsuario { get; set; }
        public virtual Usuario Usuario { get; set; }

        public virtual ProductoVenta ProductoVenta { get; set; }

        //id categoria es opcional
        public int? IDCategoria { get; set; }
        public virtual Categoria? Categoria { get; set; }
    }
}
