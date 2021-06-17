using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Save_Our_Pets.Models
{
    public class EstadoSaludModel : AbstractModel<Estado_salud>
    {

        public int devolver_id_Salud(int idMascota)
        {
            var s = ctx.Estado_salud
        .Where(Estado_salud => Estado_salud.id_mascota == idMascota)
        .Select(Estado_salud => Estado_salud.id_estado_salud)
        .FirstOrDefault();

                return Convert.ToInt32(s);

        }

        public String devolver_estado_salud(int idMascota)
        {
            var consulta =
            from s in ctx.Estado_salud
            where (s.id_mascota == idMascota)
            select s.descripcion_salud;

            return consulta.FirstOrDefault();
        }

    }
}