namespace App_VentasCompras.Models
{
    public class Ubicacion
    {
        public int IDUbicacion { get; set; }
        public string Pais { get; set; }
        public string? CodigoPostal { get; set; }
        public string Localidad { get; set; }
        public string Provincia { get; set; }
        public string? Calle { get; set; }
        public string? NroCalle { get; set; }
    }
}
