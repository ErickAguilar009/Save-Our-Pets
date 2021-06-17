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
    public class reporte_rescateController : Controller
    {
        private save_our_petsEntities db = new save_our_petsEntities();
        DatosReporte informacionReporte = new DatosReporte();
        MascotaModel mascotaModel = new MascotaModel();


        // GET: reporte_rescate
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


                    var reporte_rescate = db.reporte_rescate.Include(r => r.Mascota);
                    return View(reporte_rescate.ToList());
                }
                else
                {
                    ViewBag.Title = "Perfil";
                    return RedirectToAction("Index", "Perfil", null);
                }
            }
        }

        // GET: reporte_rescate/Details/5
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
                    DatosReporte datosReporte = new DatosReporte();
                    if (id == null)
                    {
                        TempData["errorMessage"] = "Debe seleccionar un reporte de rescate";
                        return RedirectToAction("Index");
                    }
                    reporte_rescate reporte_rescate = db.reporte_rescate.Find(id);

                    datosReporte.infoRescate = reporte_rescate;
                    datosReporte.infoMascota = db.Mascota.Find(reporte_rescate.id_mascota);
                    if (reporte_rescate == null)
                    {
                        TempData["errorMessage"] = "No se ha encontrado el reporte de rescate";
                        return RedirectToAction("Index");
                    }
                    return View(datosReporte);
                }
                else
                {
                    ViewBag.Title = "Perfil";
                    return RedirectToAction("Index", "Perfil", null);
                }
            }


           
        }

        // GET: reporte_rescate/Create
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

        // POST: reporte_rescate/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_solicitud,id_mascota,email,telefono,direccion,descripcion,fecha_reporte,fecha_rescate")] reporte_rescate reporte_rescate)
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
                        db.reporte_rescate.Add(reporte_rescate);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }

                    ViewBag.id_mascota = new SelectList(db.Mascota, "id_mascota", "nombre_mascota", reporte_rescate.id_mascota);
                    return View(reporte_rescate);
                }
                else
                {
                    ViewBag.Title = "Perfil";
                    return RedirectToAction("Index", "Perfil", null);
                }
            }

            
        }

        // GET: reporte_rescate/Edit/5
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
                        TempData["errorMessage"] = "Debe seleccionar un reporte de rescate";
                        return RedirectToAction("Index");
                    }
                    reporte_rescate reporte_rescate = db.reporte_rescate.Find(id);
                    if (reporte_rescate == null)
                    {
                        TempData["errorMessage"] = "No se ha encontrado el reporte de rescate";
                        return RedirectToAction("Index");
                    }
                    ViewBag.id_mascota = new SelectList(db.Mascota, "id_mascota", "nombre_mascota", reporte_rescate.id_mascota);
                    return View(reporte_rescate);
                }
                else
                {
                    ViewBag.Title = "Perfil";
                    return RedirectToAction("Index", "Perfil", null);
                }
            }

           
        }

        // POST: reporte_rescate/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_solicitud,id_mascota,email,telefono,direccion,descripcion,fecha_reporte,fecha_rescate")] reporte_rescate reporte_rescate, string estadoRescate)
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
                        db.Entry(reporte_rescate).State = EntityState.Modified;
                        db.SaveChanges();
                        if (!String.IsNullOrEmpty(estadoRescate))
                        {
                            Mascota mascota = db.Mascota.Find(reporte_rescate.id_mascota);

                            if (Convert.ToInt32(estadoRescate) == 1)
                            {
                                //Si ya lo rescataron
                                mascota.id_estado = 1;
                                mascotaModel.Update(mascota, reporte_rescate.id_mascota);
                            }
                            else
                            {
                                mascota.id_estado = 3;
                                mascotaModel.Update(mascota, reporte_rescate.id_mascota);
                            }
                        }

                        return RedirectToAction("Index");
                    }
                    ViewBag.id_mascota = new SelectList(db.Mascota, "id_mascota", "nombre_mascota", reporte_rescate.id_mascota);
                    return View(reporte_rescate);
                }
                else
                {
                    ViewBag.Title = "Perfil";
                    return RedirectToAction("Index", "Perfil", null);
                }
            }

            
        }

        // GET: reporte_rescate/Delete/5
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
                        TempData["errorMessage"] = "Debe seleccionar un reporte de rescate";
                        return RedirectToAction("Index");
                    }
                    reporte_rescate reporte_rescate = db.reporte_rescate.Find(id);
                    if (reporte_rescate == null)
                    {
                        TempData["errorMessage"] = "No se ha encontrado el reporte de rescate";
                        return RedirectToAction("Index");
                    }
                    return View(reporte_rescate);
                }
                else
                {
                    ViewBag.Title = "Perfil";
                    return RedirectToAction("Index", "Perfil", null);
                }
            }
            
        }


        // POST: reporte_rescate/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
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
                    reporte_rescate reporte_rescate = db.reporte_rescate.Find(id);
                    db.reporte_rescate.Remove(reporte_rescate);
                    db.SaveChanges();
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
