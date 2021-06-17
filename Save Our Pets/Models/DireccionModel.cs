using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Save_Our_Pets.Models
{
    public class DireccionModel : AbstractModel<direccion_usuario>
    {

        public direccion_usuario buscarDireccionporID(int id)
        {
            var consulta = (
         from a in ctx.direccion_usuario
         where (a.id_usuario == id)
         select a).FirstOrDefault();

            return consulta;
        }

        public int obtenerIDDireccion(int id)
        {
            var consulta = (
         from a in ctx.direccion_usuario
         where (a.id_usuario == id)
         select a.id_direccion).FirstOrDefault();

            return consulta;
        }

    }
}