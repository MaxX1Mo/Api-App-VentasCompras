using System.Data;

namespace App_VentasCompras.Models
{
    public class Usuario
    {
        public int IDUsuario { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public virtual Persona Persona { get; set; }

        public Roles Rol { get; set; }

        //public int IDUbicacion { get; set; }
        //public virtual Ubicacion Ubicacion { get; set; }

        //el status es opcional porque puede no tener nada
        public virtual Status? Status { get; set; }

        public virtual ICollection<Producto>? Productos { get; set; }
    }
}
