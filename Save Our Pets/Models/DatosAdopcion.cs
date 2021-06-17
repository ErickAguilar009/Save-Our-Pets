using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Save_Our_Pets.Models
{
    public class DatosAdopcion
    {

        public Usuarios infoUsuario { get; set; }

        public direccion_usuario datosPersonalesUsuario { get; set; }

        public solicitud_adopcion datosSolicitud { get; set; }
    }
}