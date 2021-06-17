using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Save_Our_Pets.Models;

namespace Save_Our_Pets.Models
{
    public class UsuariosModel : AbstractModel<Usuarios>
    {
        public Usuarios VerificarCuentaporEmail(String email, String pin)
        {
            string pass = SecurityUtils.EncriptarSHA2(pin);
            return ctx.Usuarios.Where(u => u.email == email && u.contra == pass).FirstOrDefault();
          
        }

        public Usuarios VerificarCorreo(string correo)
        {
            //Verificamos que no este registrado ese correo
            return ctx.Usuarios.Where(u => u.email == correo).FirstOrDefault();
        }

        public int obtenerIDUsuario(string email, string nombre, string apellido)
        {
            var consulta = (
         from u in ctx.Usuarios
         where (u.email == email) && (u.nombres == nombre) && (u.apellidos == apellido)
         select u.id_usuario).FirstOrDefault();

            return consulta;
        }

        public Usuarios obtenerUsuario(int id_user)
        {
            var consulta = (
         from u in ctx.Usuarios
         where u.id_usuario == id_user
         select u).FirstOrDefault();

            return consulta;
        }

        public Usuarios Usuario_por_Key(string clave)
        {
            var consulta = (
         from u in ctx.Usuarios
         where u.contra == clave
         select u).FirstOrDefault();

            return consulta;
        }


        public IOrderedQueryable<Usuarios> Listar()
        {
            var result = from u in ctx.Usuarios
                         orderby u.apellidos
                         select u;
            return result;
        }

        public IOrderedQueryable<Usuarios> BuscarPorNombre(string dato)
        {
            var result = from u in ctx.Usuarios
                         where (u.nombres + " " + u.apellidos).Contains(dato)
                         orderby u.apellidos
                         select u;
            return result;
        }

        public IOrderedQueryable<Usuarios> BuscarPorTipo(string tipoUsuario)
        {
            int tipo = int.Parse(tipoUsuario);

            var result = from u in ctx.Usuarios
                         where u.id_tipo == tipo
                         orderby u.id_tipo descending
                         select u;
            return result;
        }

        public IOrderedQueryable<Usuarios> BuscarPorNombreyTipo(string nombre, string tipoUsuario)
        {
            int tipo = int.Parse(tipoUsuario);

            var result = from u in ctx.Usuarios
                         where (u.nombres.Contains(nombre))
                         || (u.apellidos.Contains(nombre))
                         && (u.id_tipo == tipo)
                         orderby u.apellidos descending
                         select u;
            return result;
        }

    }
}