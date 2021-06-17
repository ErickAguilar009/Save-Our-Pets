using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Save_Our_Pets.Models
{
    public class ImagenModel:AbstractModel<img_mascota>
    {

        public byte[] Convertir_a_Bytes(HttpPostedFileBase image)
        {
            byte[] imageBytes = null;
            BinaryReader reader = new BinaryReader(image.InputStream);
            imageBytes = reader.ReadBytes((int)image.ContentLength);
            return imageBytes;
        }

        public byte[] obtenerImagen(int Id)
        {
            var imagen = from o in ctx.img_mascota
                         where o.id_imagen == Id
                         select o.imagen_mascota;
            byte[] img = imagen.First();
            return img;
        }

        public int detectarImagen(int id)
        {
            var consulta = (
         from a in ctx.img_mascota
         where (a.id_mascota == id)
         select a.id_mascota).Count();

            return consulta;
        }

        public int detectar_id_imagen(int id)
        {
            var consulta = (
         from a in ctx.img_mascota
         where (a.id_mascota == id)
         select a.id_imagen).First();

            return consulta;
        }

        public List<int> detectar_imagenes_por_mascota(int id)
        {
            var consulta = (
         from a in ctx.img_mascota
         where (a.id_mascota == id)
         select a.id_imagen).ToList();

            return consulta;
        }

        public List<img_mascota> imagenes_por_mascota(int id)
        {
            var consulta = (
         from a in ctx.img_mascota
         where (a.id_mascota == id)
         select a).ToList();

            return consulta;
        }

        public List<img_mascota> imagenes_en_general()
        {
            var consulta = (
         from a in ctx.img_mascota
         select a).ToList();

            return consulta;
        }


        public IOrderedQueryable<img_mascota> Listar()
        {
            var result = from u in ctx.img_mascota
                         orderby u.id_imagen
                         select u;
            return result;
        }

        public IOrderedQueryable<img_mascota> BuscarPorNombre(string dato)
        {
            var result = from u in ctx.img_mascota
                         join m in ctx.Mascota
                         on u.id_mascota equals m.id_mascota
                         where (m.nombre_mascota).Contains(dato)
                         orderby u.id_imagen
                         select u;
            return (IOrderedQueryable<img_mascota>)result;
        }

        public IOrderedQueryable<img_mascota> BuscarPorNombreyEspecie(string nombre, string especie)
        {
            int especieId = int.Parse(especie);

            var result = from u in ctx.img_mascota
                         join m in ctx.Mascota
                         on u.id_mascota equals m.id_mascota
                         where (m.nombre_mascota).Contains(nombre) &&
                         m.id_especie == especieId
                         orderby u.id_imagen
                         select u;
            return (IOrderedQueryable<img_mascota>)result;
        }

        public IOrderedQueryable<img_mascota> BuscarPorEspecie( string especie)
        {
            int especieId = int.Parse(especie);

            var result = from u in ctx.img_mascota
                         join m in ctx.Mascota
                         on u.id_mascota equals m.id_mascota
                         where m.id_especie == especieId
                         orderby u.id_imagen
                         select u;
            return (IOrderedQueryable<img_mascota>)result;
        }
    }
}