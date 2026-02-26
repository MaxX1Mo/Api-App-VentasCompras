Base de datos y Api fueron subidas a Somme, se genero una carpeta al publicar el proyecto, esa carpeta se sube a somme para tener la api hosteada en Somme. publicacionWebApi.zip
url:  http://apiappvyc.somee.com           url para ver enpoints disponibles: http://apiappvyc.somee.com/swagger/index.html

Para subir base de datos de manera local, ejecutar estos comandos en Consola del Administrador de paquetes:
- Add-Migration InitialCreate
- Update-Database
Tambien se debe cambiar la cadena de conexion. (Ya que se encuentra conectado a somme)

