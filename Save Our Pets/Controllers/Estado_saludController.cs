using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Save_Our_Pets.Models;

namespace Save_Our_Pets.Controllers
{
    public class Estado_saludController : Controller
    {
        private save_our_petsEntities db = new save_our_petsEntities();

        // GET: Estado_salud
        public ActionResult Index()
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

                    var estado_salud = db.Estado_salud.Include(e => e.Mascota);
                    return View(estado_salud.ToList());
                }
                else
                {
                    ViewBag.Title = "Perfil";
                    return RedirectToAction("Index", "Perfil", null);
                }
            }

        }

        // GET: Estado_salud/Details/5
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
                        TempData["errorMessage"] = "Debe seleccionar un registro";
                        return RedirectToAction("Index");
                    }
                    Estado_salud estado_salud = db.Estado_salud.Find(id);
                    if (estado_salud == null)
                    {
                        //return HttpNotFound();
                        TempData["errorMessage"] = "No se ha encontrado el registro de salud";
                        return RedirectToAction("Index");
                    }
                    return View(estado_salud);
                }
                else
                {
                    ViewBag.Title = "Perfil";
                    return RedirectToAction("Index", "Perfil", null);
                }
            }


            
        }

        // GET: Estado_salud/Create
        public ActionResult Create()
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
                    ViewBag.id_mascota = new SelectList(db.Mascota, "id_mascota", "nombre_mascota");
                    return View();
                }
                else
                {
                    ViewBag.Title = "Perfil";
                    return RedirectToAction("Index", "Perfil", null);
                }
            }

            
        }

        // POST: Estado_salud/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_estado_salud,id_mascota,descripcion_salud")] Estado_salud estado_salud)
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
                    if (ModelState.IsValid)
                    {
                        db.Estado_salud.Add(estado_salud);
                        db.SaveChanges();
                        TempData["successMessage"] = "Registro agregado de forma exitosa";
                        return RedirectToAction("Index");
                    }

                    foreach (ModelState modelState in ModelState.Values)
                    {
                        foreach (ModelError error in modelState.Errors)
                        {
                            TempData["errorMessage"] = error.ErrorMessage;
                        }
                    }

                    ViewBag.id_mascota = new SelectList(db.Mascota, "id_mascota", "nombre_mascota", estado_salud.id_mascota);
                    return View(estado_salud);
                }
                else
                {
                    ViewBag.Title = "Perfil";
                    return RedirectToAction("Index", "Perfil", null);
                }
            }

            
        }

        // GET: Estado_salud/Edit/5
        public ActionResult Edit(int? id)
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
                        TempData["errorMessage"] = "Debe seleccionar un registro";
                        return RedirectToAction("Index");
                    }
                    Estado_salud estado_salud = db.Estado_salud.Find(id);
                    if (estado_salud == null)
                    {
                        TempData["errorMessage"] = "No se ha encontrado el registro de salud";
                        return RedirectToAction("Index");
                    }
                    ViewBag.id_mascota = new SelectList(db.Mascota, "id_mascota", "nombre_mascota", estado_salud.id_mascota);
                    return View(estado_salud);
                }
                else
                {
                    ViewBag.Title = "Perfil";
                    return RedirectToAction("Index", "Perfil", null);
                }
            }


            
        }

        // POST: Estado_salud/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_estado_salud,id_mascota,descripcion_salud")] Estado_salud estado_salud)
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
                    if (ModelState.IsValid)
                    {
                        db.Entry(estado_salud).State = EntityState.Modified;
                        db.SaveChanges();
                        TempData["successMessage"] = "Registro modificado de forma exitosa";
                        return RedirectToAction("Index");
                    }

                    foreach (ModelState modelState in ModelState.Values)
                    {
                        foreach (ModelError error in modelState.Errors)
                        {
                            TempData["errorMessage"] = error.ErrorMessage;
                        }
                    }

                    ViewBag.id_mascota = new SelectList(db.Mascota, "id_mascota", "nombre_mascota", estado_salud.id_mascota);
                    return View(estado_salud);
                }
                else
                {
                    ViewBag.Title = "Perfil";
                    return RedirectToAction("Index", "Perfil", null);
                }
            }


            
        }

        // GET: Estado_salud/Delete/5
        public ActionResult Delete(int id)
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
                        TempData["errorMessage"] = "Debe seleccionar un registro";
                        return RedirectToAction("Index");
                    }
                    Estado_salud estado_salud = db.Estado_salud.Find(id);
                    if (estado_salud == null)
                    {
                        TempData["errorMessage"] = "No se ha encontrado el registro de salud";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        db.Estado_salud.Remove(estado_salud);
                        if (db.SaveChanges() > 0)
                        {
                            TempData["successMessage"] = "Registro eliminado de forma exitosa";
                        }
                        else
                        {
                            TempData["errorMessage"] = "No se pudo eliminar el registro que seleccionó";
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
