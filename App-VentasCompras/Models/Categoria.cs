namespace App_VentasCompras.Models
{
    public class Categoria
    {
        public int IDCategoria { get; set; }
        public string Nombre { get; set; }
        //public bool Activo { get; set; }
        public virtual ICollection<Producto>? Producto { get; set; }
    }
}
