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
    public class Tipo_usuarioController : Controller
    {
        private save_our_petsEntities db = new save_our_petsEntities();

        // GET: Tipo_usuario
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

                    return View(db.Tipo_usuario.ToList());
                }
                else
                {
                    ViewBag.Title = "Perfil";
                    return RedirectToAction("Index", "Perfil", null);
                }
            }

        }

        // GET: Tipo_usuario/Details/5
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
                        TempData["errorMessage"] = "Debe seleccionar un tipo de usuario";
                        return RedirectToAction("Index");
                    }
                    Tipo_usuario tipo_usuario = db.Tipo_usuario.Find(id);
                    if (tipo_usuario == null)
                    {
                        TempData["errorMessage"] = "No se ha encontrado el tipo de usuario";
                        return RedirectToAction("Index");
                    }
                    return View(tipo_usuario);
                }
                else
                {
                    ViewBag.Title = "Perfil";
                    return RedirectToAction("Index", "Perfil", null);
                }
            }

            
        }

        // GET: Tipo_usuario/Create
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

        // POST: Tipo_usuario/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_tipo,nombre_tipo")] Tipo_usuario tipo_usuario)
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
                        db.Tipo_usuario.Add(tipo_usuario);
                        db.SaveChanges();
                        TempData["successMessage"] = "Tipo de usuario agregado de forma exitosa";
                        return RedirectToAction("Index");
                    }

                    foreach (ModelState modelState in ModelState.Values)
                    {
                        foreach (ModelError error in modelState.Errors)
                        {
                            TempData["errorMessage"] = error.ErrorMessage;
                        }
                    }

                    return View(tipo_usuario);
                }
                else
                {
                    ViewBag.Title = "Perfil";
                    return RedirectToAction("Index", "Perfil", null);
                }
            }

            
        }

        // GET: Tipo_usuario/Edit/5
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
                        TempData["errorMessage"] = "Debe seleccionar un tipo de usuario";
                        return RedirectToAction("Index");
                    }
                    Tipo_usuario tipo_usuario = db.Tipo_usuario.Find(id);
                    if (tipo_usuario == null)
                    {
                        TempData["errorMessage"] = "No se ha encontrado el tipo de usuario";
                        return RedirectToAction("Index");
                    }
                    return View(tipo_usuario);
                }
                else
                {
                    ViewBag.Title = "Perfil";
                    return RedirectToAction("Index", "Perfil", null);
                }
            }

            
        }

        // POST: Tipo_usuario/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_tipo,nombre_tipo")] Tipo_usuario tipo_usuario)
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
                        db.Entry(tipo_usuario).State = EntityState.Modified;
                        db.SaveChanges();
                        TempData["successMessage"] = "Tipo de usuario modificado de forma exitosa";
                        return RedirectToAction("Index");
                    }

                    foreach (ModelState modelState in ModelState.Values)
                    {
                        foreach (ModelError error in modelState.Errors)
                        {
                            TempData["errorMessage"] = error.ErrorMessage;
                        }
                    }

                    return View(tipo_usuario);
                }
                else
                {
                    ViewBag.Title = "Perfil";
                    return RedirectToAction("Index", "Perfil", null);
                }
            }

            
        }

        // GET: Tipo_usuario/Delete/5
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
                        TempData["errorMessage"] = "Debe seleccionar un tipo de usuario";
                        return RedirectToAction("Index");
                    }
                    Tipo_usuario tipo_usuario = db.Tipo_usuario.Find(id);
                    if (tipo_usuario == null)
                    {
                        TempData["errorMessage"] = "No se encontró el registro que selecciono";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        db.Tipo_usuario.Remove(tipo_usuario);
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
