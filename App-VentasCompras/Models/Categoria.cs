using System.ComponentModel.DataAnnotations;

namespace App_VentasCompras.Models
{
    public class Categoria
    {
        public int IDCategoria { get; set; }

        //[Required(ErrorMessage = "El nombre de la categoría es obligatorio.")]
        public string Nombre { get; set; }

        public virtual ICollection<Producto>? Producto { get; set; }
    }
}
