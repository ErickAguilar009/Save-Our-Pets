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
    public class RazasController : Controller
    {
        private save_our_petsEntities db = new save_our_petsEntities();
        MascotaModel model_mascota = new MascotaModel();
        

        // GET: Razas
        public ActionResult Index(string currentFilter, string nombreRaza, string especie, string filtroTipo, int? page)
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

                    if (nombreRaza != null)
                    {
                        page = 1;
                    }
                    else
                    {
                        nombreRaza = currentFilter;
                    }

                    ViewBag.CurrentFilter = nombreRaza;

                    if (especie != null)
                    {
                        page = 1;
                    }
                    else
                    {
                        especie = filtroTipo;
                    }
                    ViewBag.FiltroTipo = especie;

                    var raza = model_mascota.ListarRazas();

                    if (!string.IsNullOrEmpty(nombreRaza) && string.IsNullOrEmpty(especie))
                    {
                        raza = model_mascota.BuscarNombreRaza(nombreRaza);
                    }
                    /**/
                    if (!string.IsNullOrEmpty(nombreRaza) && !string.IsNullOrEmpty(especie))
                    {

                        raza = model_mascota.BuscarNombreRazayEspecie(nombreRaza, especie);

                    }


                    if (!string.IsNullOrEmpty(especie) && string.IsNullOrEmpty(nombreRaza))
                    {
                        raza = model_mascota.RazasporEspecie(especie);
                    }
                    /**/

                    int pageSize = 10;
                    int pageNumber = (page ?? 1);

                    return View(raza.ToPagedList(pageNumber, pageSize));
                }
                else
                {
                    ViewBag.Title = "Perfil";
                    return RedirectToAction("Index", "Perfil", null);
                }
            }

            


            
        }

        // GET: Razas/Details/5
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
                        TempData["errorMessage"] = "Debe seleccionar un registro de raza";
                        return RedirectToAction("Index");
                    }
                    Raza raza = db.Raza.Find(id);
                    if (raza == null)
                    {
                        TempData["errorMessage"] = "No se ha encontrado la raza";
                        return RedirectToAction("Index");
                    }
                    return View(raza);
                }
                else
                {
                    ViewBag.Title = "Perfil";
                    return RedirectToAction("Index", "Perfil", null);
                }
            }

            
        }

        // GET: Razas/Create
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

                    ViewBag.id_especie = new SelectList(db.Especie, "id_especie", "nombre");
                    return View();
                }
                else
                {
                    ViewBag.Title = "Perfil";
                    return RedirectToAction("Index", "Perfil", null);
                }
            }

        }

        // POST: Razas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_raza,nombre,id_especie")] Raza raza)
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
                        db.Raza.Add(raza);
                        db.SaveChanges();
                        TempData["successMessage"] = "Raza agregada de forma exitosa";
                        return RedirectToAction("Index");
                    }


                    foreach (ModelState modelState in ModelState.Values)
                    {
                        foreach (ModelError error in modelState.Errors)
                        {
                            TempData["errorMessage"] = error.ErrorMessage;
                        }
                    }

                    ViewBag.id_especie = new SelectList(db.Especie, "id_especie", "nombre", raza.id_especie);
                    return View(raza);
                }
                else
                {
                    ViewBag.Title = "Perfil";
                    return RedirectToAction("Index", "Perfil", null);
                }
            }

            
        }

        // GET: Razas/Edit/5
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
                        TempData["errorMessage"] = "Debe seleccionar un registro de raza";
                        return RedirectToAction("Index");
                    }
                    Raza raza = db.Raza.Find(id);
                    if (raza == null)
                    {
                        TempData["errorMessage"] = "No se ha encontrado la raza";
                        return RedirectToAction("Index");
                    }
                    ViewBag.id_especie = new SelectList(db.Especie, "id_especie", "nombre", raza.id_especie);
                    return View(raza);
                }
                else
                {
                    ViewBag.Title = "Perfil";
                    return RedirectToAction("Index", "Perfil", null);
                }
            }

            
        }

        // POST: Razas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_raza,nombre,id_especie")] Raza raza)
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
                        db.Entry(raza).State = EntityState.Modified;
                        db.SaveChanges();
                        TempData["successMessage"] = "Raza modificada de forma exitosa";
                        return RedirectToAction("Index");
                    }

                    foreach (ModelState modelState in ModelState.Values)
                    {
                        foreach (ModelError error in modelState.Errors)
                        {
                            TempData["errorMessage"] = error.ErrorMessage;
                        }
                    }

                    ViewBag.id_especie = new SelectList(db.Especie, "id_especie", "nombre", raza.id_especie);
                    return View(raza);
                }
                else
                {
                    ViewBag.Title = "Perfil";
                    return RedirectToAction("Index", "Perfil", null);
                }
            }

            
        }

        // GET: Razas/Delete/5
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
                        TempData["errorMessage"] = "Debe seleccionar un registro de raza";
                        return RedirectToAction("Index");
                    }
                    Raza raza = db.Raza.Find(id);
                    if (raza == null)
                    {
                        TempData["errorMessage"] = "No se ha encontrado la raza";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        db.Raza.Remove(raza);
                        if (db.SaveChanges() > 0)
                        {
                            TempData["successMessage"] = "Raza eliminada de forma exitosa";
                        }
                        else
                        {
                            TempData["errorMessage"] = "No se pudo eliminar la raza que seleccionó";
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
