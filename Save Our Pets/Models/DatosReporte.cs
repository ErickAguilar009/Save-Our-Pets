using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Save_Our_Pets.Models
{
    public class DatosReporte
    {

        public reporte_rescate infoRescate { get; set; }

        public Mascota infoMascota { get; set; }

        public Usuarios datosUsuario { get; set; }

    }
}