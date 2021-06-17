using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Save_Our_Pets.Models
{
    public class DatosUsuario
    {
        public Usuarios datos_usuario { get; set; }
        public direccion_usuario direcc_usuario { get; set; }
        public solicitud_adopcion soli_usuario { get; set; }

    }
}