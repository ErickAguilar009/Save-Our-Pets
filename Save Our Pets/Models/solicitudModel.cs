using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Save_Our_Pets.Models
{
    public class solicitudModel : AbstractModel<solicitud_adopcion>
    {

        public solicitud_adopcion buscarsolicitudporID(int id)
        {
            var consulta = (
         from a in ctx.solicitud_adopcion
         where (a.id_usuario == id)
         select a).FirstOrDefault();

            return consulta;
        }

        public int obtenerIDSolicitud(int id)
        {
            var consulta = (
         from a in ctx.solicitud_adopcion
         where (a.id_usuario == id)
         select a.id_solicitud).FirstOrDefault();

            return consulta;
        }

        public IOrderedQueryable<solicitud_adopcion> Listar()
        {
            var result = from u in ctx.solicitud_adopcion
                         orderby u.id_solicitud
                         select u;
            return result;
        }

        public IOrderedQueryable<solicitud_adopcion> BuscarNombre(string dato)
        {

            var result = from u in ctx.solicitud_adopcion
                         where (u.Usuarios.nombres.Contains(dato))
                         orderby u.id_solicitud descending
                         select u;
            return result;
        }

        public IOrderedQueryable<solicitud_adopcion> solicitudesporEstado(string id)
        {
            int idSelect = int.Parse(id);
            var result = from u in ctx.solicitud_adopcion
                         where u.estado_solicitud == idSelect
                         orderby u.id_solicitud descending
                         select u;
            return result;
        }

        public IOrderedQueryable<solicitud_adopcion> BuscarNombreyEstado(string nombre, string estado)
        {
            int estadoId = int.Parse(estado);

            var result = from u in ctx.solicitud_adopcion
                         where (u.Usuarios.nombres.Contains(nombre))
                         && (u.estado_solicitud == estadoId)
                         orderby u.id_solicitud descending
                         select u;
            return result;
        }

    }
}