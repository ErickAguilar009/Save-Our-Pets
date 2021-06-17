using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Save_Our_Pets.Models
{
    public class reporteVacunasModel:AbstractModel<Reporte_vacunas>
    {

        public List<Reporte_vacunas> obtenerlistadoVacunas(int id_masc)
        {
            var consulta = (
         from a in ctx.Reporte_vacunas
         where (a.id_mascota == id_masc)
         select a).ToList();

            return consulta;
        }

        public IOrderedQueryable<Reporte_vacunas> Listar()
        {
            var result = from u in ctx.Reporte_vacunas
                         orderby u.id_vacuna_aplicada
                         select u;
            return result;
        }

        public IOrderedQueryable<Reporte_vacunas> BuscarporNombreMascota(string dato)
        {

            var result = from r in ctx.Reporte_vacunas
                         join m in ctx.Mascota
                         on r.id_mascota equals m.id_mascota
                         where (m.nombre_mascota.Contains(dato))
                         orderby r.id_vacuna_aplicada descending
                         select r;
            return (IOrderedQueryable<Reporte_vacunas>)result;
        }


    }
}