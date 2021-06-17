using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Save_Our_Pets.Models
{
    public class reporteSeguimientoModel : AbstractModel<reporte_seguimiento>
    {
        public List<reporte_seguimiento> obtenerlistadodeVisitas(int id_masc)
        {
            var consulta = (
         from m in ctx.Mascota
         join a in ctx.Adopciones
         on m.id_mascota equals a.id_mascota
         join r in ctx.reporte_seguimiento
         on a.id_adopcion equals r.id_adopcion
         where (m.id_mascota == id_masc)
         select r).ToList();

            return consulta;

        }

        public Adopciones obtenerIdAdopcion(int id_masc)
        {
            var consulta = (from s in ctx.Adopciones
                           where s.id_mascota == id_masc
                           select s).FirstOrDefault();

            return consulta;

        }

        public int contarVisitas(int id_seg)
        {
            var consulta = (from s in ctx.reporte_seguimiento
                            where s.id_seguimiento == id_seg
                            select s).Count();

            return consulta;

        }

        public IOrderedQueryable<reporte_seguimiento> Listar()
        {
            var result = from u in ctx.reporte_seguimiento
                         orderby u.id_seguimiento
                         select u;
            return result;
        }

        public IOrderedQueryable<reporte_seguimiento> BuscarporMascota(string dato)
        {

            var result = from r in ctx.reporte_seguimiento
                         join a in ctx.Adopciones
                         on r.id_adopcion equals a.id_adopcion
                         where (a.Mascota.nombre_mascota.Contains(dato))
                         orderby r.id_seguimiento descending
                         select r;
            return (IOrderedQueryable<reporte_seguimiento>)result;
        }

        public IOrderedQueryable<reporte_seguimiento> BuscarporAdoptador(string dato)
        {
            var result = from r in ctx.reporte_seguimiento
                         join a in ctx.Adopciones
                         on r.id_adopcion equals a.id_adopcion
                         where (a.Usuarios.nombres.Contains(dato))
                         orderby r.id_seguimiento descending
                         select r;
            return (IOrderedQueryable<reporte_seguimiento>)result;
        }

        public IOrderedQueryable<reporte_seguimiento> BuscarNombreyMascota(string nombre, string mascota)
        {
            var result = from r in ctx.reporte_seguimiento
                         join a in ctx.Adopciones
                         on r.id_adopcion equals a.id_adopcion
                         where (a.Usuarios.nombres.Contains(nombre) && a.Mascota.nombre_mascota.Contains(mascota))
                         orderby r.id_seguimiento descending
                         select r;
            return (IOrderedQueryable<reporte_seguimiento>)result;
        }

    }
}