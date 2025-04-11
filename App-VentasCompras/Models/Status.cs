namespace App_VentasCompras.Models
{
    public class Status
    {
        public int IDStatus { get; set; }
        public int? VentasExitosas { get; set; }

        public virtual Valoracion Valoracion { get; set; }

        public int IDUsuario { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
