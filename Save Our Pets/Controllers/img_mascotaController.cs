using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Save_Our_Pets.Models;

namespace Save_Our_Pets.Controllers
{
    public class img_mascotaController : Controller
    {
        private save_our_petsEntities db = new save_our_petsEntities();
        ImagenModel imodel = new ImagenModel();
        MascotaModel model_mascota = new MascotaModel();

        // GET: img_mascota
        public ActionResult Index(string currentFilter, string nombreMascota, string especie, string filtroTipo, int? page)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] == null)
            {
                TempData["errorMessage"] = "Debe iniciar sesión antes de continuar";
                return RedirectToAction("IniciarSesion", "Login", null);
            }
            else
            {
                if ((int)System.Web.HttpContext.Current.Session["tipo_usuario"] != 3)
                {
                    ViewBag.id_especies = db.Especie.ToList();

                    if (nombreMascota != null)
                    {
                        page = 1;
                    }
                    else
                    {
                        nombreMascota = currentFilter;
                    }

                    ViewBag.CurrentFilter = nombreMascota;

                    if (especie != null)
                    {
                        page = 1;
                    }
                    else
                    {
                        especie = filtroTipo;
                    }
                    ViewBag.FiltroTipo = especie;


                    var img_mascota = imodel.Listar();

                    if (!string.IsNullOrEmpty(nombreMascota) && string.IsNullOrEmpty(especie))
                    {
                        img_mascota = imodel.BuscarPorNombre(nombreMascota);
                    }
                    /**/
                    if (!string.IsNullOrEmpty(nombreMascota) && !string.IsNullOrEmpty(especie))
                    {
                        img_mascota = imodel.BuscarPorNombreyEspecie(nombreMascota, especie);

                    }


                    if (!string.IsNullOrEmpty(especie) && string.IsNullOrEmpty(nombreMascota))
                    {
                        img_mascota = imodel.BuscarPorEspecie(especie);
                    }
                    /**/

                    int pageSize = 16;
                    int pageNumber = (page ?? 1);

                    return View(img_mascota.ToPagedList(pageNumber, pageSize));
                }
                else
                {
                    ViewBag.Title = "Perfil";
                    return RedirectToAction("Index", "Perfil", null);
                }
            }

            
        }

        public ActionResult ImagenMascota(string id)
        {
            int id2 = Convert.ToInt32(id);

            byte[] cover = imodel.obtenerImagen(id2);
            if (cover != null)
            {

                return File(cover, "image/jpg");
            }
            else
            {

                return Redirect("None/Errore");
            }

            //Si no posee imagen personalizada



        }


        // GET: img_mascota/Details/5
        public ActionResult Details(int? id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] == null)
            {
                TempData["errorMessage"] = "Debe iniciar sesión antes de continuar";
                return RedirectToAction("IniciarSesion", "Login", null);
            }
            else
            {
                if ((int)System.Web.HttpContext.Current.Session["tipo_usuario"] != 3)
                {
                    if (id == null)
                    {
                        TempData["errorMessage"] = "Debe seleccionar un registro de imagen";
                        return RedirectToAction("Index");
                    }
                    img_mascota img_mascota = db.img_mascota.Find(id);
                    if (img_mascota == null)
                    {
                        TempData["errorMessage"] = "No se ha encontrado la imagen";
                        return RedirectToAction("Index");
                        
                    }
                    return View(img_mascota);
                }
                else
                {
                    ViewBag.Title = "Perfil";
                    return RedirectToAction("Index", "Perfil", null);
                }
            }

            
        }

        // GET: img_mascota/Create
        public ActionResult Create(int ?id)
        {

            if (System.Web.HttpContext.Current.Session["usuario"] == null)
            {
                TempData["errorMessage"] = "Debe iniciar sesión antes de continuar";
                return RedirectToAction("IniciarSesion", "Login", null);
            }
            else
            {
                if ((int)System.Web.HttpContext.Current.Session["tipo_usuario"] != 3)
                {
                    if (id != null)
                    {
                        ViewBag.Stater = "Si";
                        ViewBag.id_mascota = new SelectList(db.Mascota.Where(m => m.id_mascota == id), "id_mascota", "nombre_mascota");

                    }
                    else
                    {
                        ViewBag.Stater = "No";
                        ViewBag.id_mascota = new SelectList(db.Mascota, "id_mascota", "nombre_mascota");

                    }
                    return View();
                }
                else
                {
                    ViewBag.Title = "Perfil";
                    return RedirectToAction("Index", "Perfil", null);
                }
            }

            
        }

     
        // POST: img_mascota/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_imagen,id_mascota,imagen_mascota")] img_mascota img_mascota, img_mascota image)
        {

            if (System.Web.HttpContext.Current.Session["usuario"] == null)
            {
                TempData["errorMessage"] = "Debe iniciar sesión antes de continuar";
                return RedirectToAction("IniciarSesion", "Login", null);
            }
            else
            {
                if ((int)System.Web.HttpContext.Current.Session["tipo_usuario"] != 3)
                {
                    HttpPostedFileBase file = Request.Files["ImageData"];
                    image.imagen_mascota = imodel.Convertir_a_Bytes(file);
                    img_mascota.imagen_mascota = image.imagen_mascota;

                    if (ModelState.IsValid)
                    {

                        db.img_mascota.Add(img_mascota);
                        db.SaveChanges();
                        TempData["successMessage"] = "Imagen subida de forma exitosa";
                        return RedirectToAction("Index");
                    }

                    foreach (ModelState modelState in ModelState.Values)
                    {
                        foreach (ModelError error in modelState.Errors)
                        {
                            TempData["errorMessage"] = error.ErrorMessage;
                        }
                    }

                    ViewBag.id_mascota = new SelectList(db.Mascota, "id_mascota", "nombre_mascota", img_mascota.id_mascota);
                    return View(img_mascota);
                }
                else
                {
                    ViewBag.Title = "Perfil";
                    return RedirectToAction("Index", "Perfil", null);
                }
            }

            
        }

        // GET: img_mascota/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                TempData["errorMessage"] = "Debe seleccionar un registro de imagen";
                return RedirectToAction("Index");
            }
            img_mascota img_mascota = db.img_mascota.Find(id);
            if (img_mascota == null)
            {
                TempData["errorMessage"] = "No se ha encontrado la imagen";
                return RedirectToAction("Index");
            }
            ViewBag.id_mascota = new SelectList(db.Mascota, "id_mascota", "nombre_mascota", img_mascota.id_mascota);
            return View(img_mascota);
        }

        // POST: img_mascota/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_imagen,id_mascota,imagen_mascota")] img_mascota img_mascota, img_mascota image )
        {

            if (System.Web.HttpContext.Current.Session["usuario"] == null)
            {
                TempData["errorMessage"] = "Debe iniciar sesión antes de continuar";
                return RedirectToAction("IniciarSesion", "Login", null);
            }
            else
            {
                if ((int)System.Web.HttpContext.Current.Session["tipo_usuario"] != 3)
                {
                    HttpPostedFileBase file = Request.Files["ImageData"];
                    image.imagen_mascota = imodel.Convertir_a_Bytes(file);
                    img_mascota.imagen_mascota = image.imagen_mascota;

                    if (ModelState.IsValid)
                    {
                        imodel.Remove(img_mascota.imagen_mascota);

                        db.Entry(img_mascota).State = EntityState.Modified;
                        db.SaveChanges();
                        TempData["successMessage"] = "Imagen modificada de forma exitosa";
                        return RedirectToAction("Index");
                    }

                    foreach (ModelState modelState in ModelState.Values)
                    {
                        foreach (ModelError error in modelState.Errors)
                        {
                            TempData["errorMessage"] = error.ErrorMessage;
                        }
                    }

                    ViewBag.id_mascota = new SelectList(db.Mascota, "id_mascota", "nombre_mascota", img_mascota.id_mascota);
                    return View(img_mascota);
                }
                else
                {
                    ViewBag.Title = "Perfil";
                    return RedirectToAction("Index", "Perfil", null);
                }
            }

            
        }

        // GET: img_mascota/Delete/5
        public ActionResult Delete(int? id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] == null)
            {
                TempData["errorMessage"] = "Debe iniciar sesión antes de continuar";
                return RedirectToAction("IniciarSesion", "Login", null);
            }
            else
            {
                if ((int)System.Web.HttpContext.Current.Session["tipo_usuario"] != 3)
                {
                    if (id == null)
                    {
                        TempData["errorMessage"] = "Debe seleccionar un registro de imagen";
                        return RedirectToAction("Index");
                    }
                    img_mascota img_mascota = db.img_mascota.Find(id);
                    if (img_mascota == null)
                    {
                        TempData["errorMessage"] = "No se ha encontrado la imagen";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        db.img_mascota.Remove(img_mascota);
                        if (db.SaveChanges() > 0)
                        {
                            TempData["successMessage"] = "Imagen eliminada de forma exitosa";
                        }
                        else
                        {
                            TempData["errorMessage"] = "No se pudo eliminar la imagen que seleccionó";
                        }
                    }

                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Title = "Perfil";
                    return RedirectToAction("Index", "Perfil", null);
                }
            }

            
        }

        

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
