namespace App_VentasCompras.Models
{
    public class ProductoVenta
    {
        public int IDProductoVenta { get; set; }
        public DateTime Fecha { get; set; }
        public EstadoProducto EstadoProducto { get; set; }
        public EstadoVenta EstadoVenta { get; set; }
        public int Cantidad { get; set; }

        public int IDProducto { get; set; }
        public Producto Producto { get; set; }
    }
}
