using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Save_Our_Pets.Models
{
    public class DatosMascota
    {

        public Mascota infoMascota { get; set; }

        public Estado_salud saludMascota { get; set; }

        public img_mascota imgMascota { get; set; }

        public List<Reporte_vacunas> infoVacunas { get; set; }

        public List<int> id_imagenes { get; set; }

        public List<reporte_seguimiento> listaSeguimiento { get; set; }
    }
}