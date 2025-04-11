namespace App_VentasCompras.DTOs
{
    public class UsuarioDTO
    {
        public int IDUsuario { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public Roles Rol { get; set; }  // si es un usuario nuevo el registro por default, el rol es de tipo usuario, solo un admin puede crear un usuario con rol elegible

        public int IDPersona { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string NroCelular { get; set; }

        public string Pais { get; set; }
        public string Localidad { get; set; }
        public string Provincia { get; set; }
        //estos datos son opcionales
        public string? CodigoPostal { get; set; }
        public string? Calle { get; set; }
        public string? NroCalle { get; set; }

        // la tabla status y valoraciones que corresponden al usuario, se crearan al crear al usuario, con datos null ya que es un usuario nuevo

    }
}
