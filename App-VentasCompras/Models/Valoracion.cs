namespace App_VentasCompras.Models
{
    public class Valoracion
    {
        public int IDValoraciones { get; set; }
        public int? Bueno { get; set; }
        public int? Malo { get; set; }
        public int? Regular { get; set; }
        public int IDStatus { get; set; }
        public virtual Status Status { get; set; }
    }
}
