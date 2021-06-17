using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Save_Our_Pets.Models
{
    public class MascotaModel:AbstractModel<Mascota>
    {

        public IOrderedQueryable<Mascota> Listar()
        {
            var result = from u in ctx.Mascota
                         orderby u.nombre_mascota
                         select u;
            return result;
        }

        public IOrderedQueryable<Mascota> BuscarporRaza(string id)
        {
            int raza = int.Parse(id);
            var result = from u in ctx.Mascota
                         where u.id_raza == raza
                         orderby u.nombre_mascota
                         select u;
            return result;
        }

        public IOrderedQueryable<Mascota> BuscarPorEspecie(string id)
        {
            int especie = int.Parse(id);
            var result = from u in ctx.Mascota
                         where u.id_especie == especie
                         orderby u.id_especie descending
                         select u;
            return result;
        }

        public IOrderedQueryable<Mascota> BuscarPorRazayEspecie(string raza, string especie)
        {
            int razaId = int.Parse(raza);
            int especieId = int.Parse(especie);

            var result = from u in ctx.Mascota
                         where u.id_especie == especieId
                         && (u.id_raza == razaId)
                         orderby u.id_especie descending
                         select u;
            return result;
        }

        public IOrderedQueryable<Mascota> BuscarPorNombreyEspecie(string nombre, string especie)
        {
            int especieId = int.Parse(especie);

            var result = from u in ctx.Mascota
                         where (u.nombre_mascota.Contains(nombre))
                         && (u.id_especie == especieId)
                         orderby u.nombre_mascota descending
                         select u;
            return result;
        }

        public IOrderedQueryable<Raza> ListarRazas()
        {
            var result = from u in ctx.Raza
                         orderby u.id_raza
                         select u;
            return result;
        }

        public IOrderedQueryable<Raza> BuscarNombreRaza(string dato)
        {

            var result = from u in ctx.Raza
                         where (u.nombre.Contains(dato))
                         orderby u.id_raza descending
                         select u;
            return result;
        }

        public IOrderedQueryable<Raza> RazasporEspecie(string id)
        {
            int especie = int.Parse(id);
            var result = from u in ctx.Raza
                         where u.id_especie == especie
                         orderby u.id_raza descending
                         select u;
            return result;
        }

        public IOrderedQueryable<Raza> BuscarNombreRazayEspecie(string nombre, string especie)
        {
            int especieId = int.Parse(especie);

            var result = from u in ctx.Raza
                         where (u.nombre.Contains(nombre))
                         && (u.id_especie == especieId)
                         orderby u.id_raza descending
                         select u;
            return result;
        }


        public Mascota obtenerMascotaporID(int id)
        {
            var consulta = (
         from a in ctx.Mascota
         where (a.id_mascota == id)
         select a).FirstOrDefault();

            return consulta;
        }

        public int verificarRazayEspecie(int id)
        {
            var consulta = (
         from a in ctx.Mascota
         where (a.id_raza == id)
         select a.id_especie).FirstOrDefault();

            return consulta;
        }

        public int obtenerRazaDesconocida(int id)
        {
            var consulta = (
         from a in ctx.Raza
         where (a.id_especie == id &&
         a.nombre.Contains("Desconocida"))
         select a.id_raza).FirstOrDefault();

            return Convert.ToInt32(consulta);
        }

        public int obtenerultimoRegistro(int idRaza)
        {
            var consulta = (
         from a in ctx.Mascota
         where (a.id_raza == idRaza)
         select a.id_mascota).LastOrDefault();

            return Convert.ToInt32(consulta);
        }
    }
}