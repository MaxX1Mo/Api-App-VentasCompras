namespace App_VentasCompras.DTOs
{
    public class UsuarioEditarDTO
    {
        public int IDUsuario { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string? Password { get; set; }
        public Roles Rol { get; set; }  

        public int IDPersona { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string NroCelular { get; set; }

        public string Pais { get; set; }
        public string Localidad { get; set; }
        public string Provincia { get; set; }
       
        public string? CodigoPostal { get; set; }
        public string? Calle { get; set; }
        public string? NroCalle { get; set; }

        public int? VentasExitosas { get; set; }
        public int? Bueno { get; set; }
        public int? Malo { get; set; }
        public int? Regular { get; set; }
    }
}
