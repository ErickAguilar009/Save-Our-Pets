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
    public class EspeciesController : Controller
    {
        private save_our_petsEntities db = new save_our_petsEntities();

        // GET: Especies
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
                    return View(db.Especie.ToList());
                }
                else
                {
                    ViewBag.Title = "Perfil";
                    return RedirectToAction("Index", "Perfil", null);
                }
            }


            
        }

        // GET: Especies/Details/5
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
                    Especie especie = db.Especie.Find(id);
                    if (especie == null)
                    {
                        //return HttpNotFound();
                        TempData["errorMessage"] = "No se ha encontrado ese registro de especies";
                        return RedirectToAction("Index");
                    }
                    return View(especie);
                }
                else
                {
                    ViewBag.Title = "Perfil";
                    return RedirectToAction("Index", "Perfil", null);
                }
            }


           
        }

        // GET: Especies/Create
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

        // POST: Especies/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_especie,nombre")] Especie especie)
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
                        db.Especie.Add(especie);
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

                    return View(especie);
                }
                else
                {
                    ViewBag.Title = "Perfil";
                    return RedirectToAction("Index", "Perfil", null);
                }
            }



            
        }

        // GET: Especies/Edit/5
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
                    Especie especie = db.Especie.Find(id);
                    if (especie == null)
                    {
                        //return HttpNotFound();
                        TempData["errorMessage"] = "No se ha encontrado ese registro de especies";
                        return RedirectToAction("Index");
                    }
                    return View(especie);
                }
                else
                {
                    ViewBag.Title = "Perfil";
                    return RedirectToAction("Index", "Perfil", null);
                }
            }

            
        }

        // POST: Especies/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_especie,nombre")] Especie especie)
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
                        db.Entry(especie).State = EntityState.Modified;
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
                    return View(especie);
                }
                else
                {
                    ViewBag.Title = "Perfil";
                    return RedirectToAction("Index", "Perfil", null);
                }
            }

            
        }

        // GET: Especies/Delete/5
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
                    Especie especie = db.Especie.Find(id);
                    if (especie == null)
                    {
                        TempData["errorMessage"] = "No se ha encontrado ese registro de especies";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        db.Especie.Remove(especie);
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
