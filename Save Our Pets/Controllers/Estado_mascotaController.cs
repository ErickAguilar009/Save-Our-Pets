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
    public class Estado_mascotaController : Controller
    {
        private save_our_petsEntities db = new save_our_petsEntities();

        // GET: Estado_mascota
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

                    return View(db.Estado_mascota.ToList());
                }
                else
                {
                    ViewBag.Title = "Perfil";
                    return RedirectToAction("Index", "Perfil", null);
                }
            }

        }

        // GET: Estado_mascota/Details/5
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
                    Estado_mascota estado_mascota = db.Estado_mascota.Find(id);
                    if (estado_mascota == null)
                    {
                        TempData["errorMessage"] = "No se ha encontrado el estado de mascota ingresado";
                        return RedirectToAction("Index");
                    }
                    return View(estado_mascota);
                }
                else
                {
                    ViewBag.Title = "Perfil";
                    return RedirectToAction("Index", "Perfil", null);
                }
            }

            
        }

        // GET: Estado_mascota/Create
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
                    return View();
                }
                else
                {
                    ViewBag.Title = "Perfil";
                    return RedirectToAction("Index", "Perfil", null);
                }
            }

           
        }

        // POST: Estado_mascota/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_estado,nombre_estado")] Estado_mascota estado_mascota)
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
                        db.Estado_mascota.Add(estado_mascota);
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

                    return View(estado_mascota);
                }
                else
                {
                    ViewBag.Title = "Perfil";
                    return RedirectToAction("Index", "Perfil", null);
                }
            }


            
        }

        // GET: Estado_mascota/Edit/5
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
                    Estado_mascota estado_mascota = db.Estado_mascota.Find(id);
                    if (estado_mascota == null)
                    {
                        // return HttpNotFound();
                        TempData["errorMessage"] = "No se ha encontrado el estado de mascota ingresado";
                        return RedirectToAction("Index");
                    }
                    return View(estado_mascota);
                }
                else
                {
                    ViewBag.Title = "Perfil";
                    return RedirectToAction("Index", "Perfil", null);
                }
            }

            
        }

        // POST: Estado_mascota/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_estado,nombre_estado")] Estado_mascota estado_mascota)
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
                        db.Entry(estado_mascota).State = EntityState.Modified;
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

                    return View(estado_mascota);
                }
                else
                {
                    ViewBag.Title = "Perfil";
                    return RedirectToAction("Index", "Perfil", null);
                }
            }

            
        }

        // GET: Estado_mascota/Delete/5
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
                    Estado_mascota estado_mascota = db.Estado_mascota.Find(id);
                    if (estado_mascota == null)
                    {
                        TempData["errorMessage"] = "No se ha encontrado el estado de mascota ingresado";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        db.Estado_mascota.Remove(estado_mascota);
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
