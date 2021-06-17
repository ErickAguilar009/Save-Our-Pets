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
    public class Reporte_vacunasController : Controller
    {
        private save_our_petsEntities db = new save_our_petsEntities();
        reporteVacunasModel repVacunas = new reporteVacunasModel();
        MascotaModel model_mascota = new MascotaModel();
        

        // GET: Reporte_vacunas
        public ActionResult Index(string currentFilter, string nombreMascota, int? page)
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
                    if (nombreMascota != null)
                    {
                        page = 1;
                    }
                    else
                    {
                        nombreMascota = currentFilter;
                    }

                    ViewBag.CurrentFilter = nombreMascota;


                    var reporte_vacunas = repVacunas.Listar();

                    if (!string.IsNullOrEmpty(nombreMascota))
                    {
                        reporte_vacunas = repVacunas.BuscarporNombreMascota(nombreMascota);
                    }
                    /**/

                    int pageSize = 10;
                    int pageNumber = (page ?? 1);

                    return View(reporte_vacunas.ToPagedList(pageNumber, pageSize));
                }
                else
                {
                    ViewBag.Title = "Perfil";
                    return RedirectToAction("Index", "Perfil", null);
                }
            }


            

        }

        // GET: Reporte_vacunas/Details/5
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
                        TempData["errorMessage"] = "Debe seleccionar un reporte de vacunas";
                        return RedirectToAction("Index");
                    }
                    Reporte_vacunas reporte_vacunas = db.Reporte_vacunas.Find(id);
                    if (reporte_vacunas == null)
                    {
                        TempData["errorMessage"] = "No se ha encontrado el reporte de vacunas";
                        return RedirectToAction("Index");
                    }
                    return View(reporte_vacunas);
                }
                else
                {
                    ViewBag.Title = "Perfil";
                    return RedirectToAction("Index", "Perfil", null);
                }
            }

            
        }

        // GET: Reporte_vacunas/Create
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
                    ViewBag.id_mascota = new SelectList(db.Mascota, "id_mascota", "nombre_mascota");
                    //filtrar vacunas por especie
                    ViewBag.id_vacuna = new SelectList(db.Vacunas, "id_vacuna", "vacuna");
                    ViewBag.id_vacuna_perros = new SelectList(db.Vacunas.Where(v => v.descripcion.Contains("perros")), "id_vacuna", "vacuna");
                    ViewBag.id_vacuna_gatos = new SelectList(db.Vacunas.Where(v => v.descripcion.Contains("gatos")), "id_vacuna", "vacuna");
                    ViewBag.id_vacuna_otros = new SelectList(db.Vacunas.Where(v => v.descripcion.Contains("otros")), "id_vacuna", "vacuna");
                    if (id != null)
                    {
                        Mascota nMascota = new Mascota();
                        nMascota = db.Mascota.Find(id);
                        int id_especie = nMascota.id_especie;
                        ViewBag.id_mascota = new SelectList(db.Mascota.Where(u => u.id_mascota == id), "id_mascota", "nombre_mascota");

                        if (id_especie == 1)
                        {
                            ViewBag.id_vacuna = new SelectList(db.Vacunas.Where(v => v.descripcion.Contains("gatos")), "id_vacuna", "vacuna");

                        }
                        else if (id_especie == 2)
                        {
                            ViewBag.id_vacuna = new SelectList(db.Vacunas.Where(v => v.descripcion.Contains("perros")), "id_vacuna", "vacuna");
                        }
                        else
                        {
                            ViewBag.id_vacuna = new SelectList(db.Vacunas.Where(v => v.descripcion.Contains("otros")), "id_vacuna", "vacuna");
                        }

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

        // POST: Reporte_vacunas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_vacuna_aplicada,id_mascota,id_vacuna,fecha_aplicada,fecha_a_aplicar_nueva")] Reporte_vacunas reporte_vacunas)
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
                        db.Reporte_vacunas.Add(reporte_vacunas);
                        db.SaveChanges();
                        TempData["successMessage"] = "Vacunación agregada exitosamente";
                        return RedirectToAction("Ver", "Mascotas", new { id = reporte_vacunas.id_mascota });
                    }
                    else
                    {
                        TempData["errorMessage"] = "Ha ocurrido un error";
                    }

                    ViewBag.id_mascota = new SelectList(db.Mascota, "id_mascota", "nombre_mascota", reporte_vacunas.id_mascota);
                    ViewBag.id_vacuna = new SelectList(db.Vacunas, "id_vacuna", "vacuna", reporte_vacunas.id_vacuna);
                    return View(reporte_vacunas);
                }
                else
                {
                    ViewBag.Title = "Perfil";
                    return RedirectToAction("Index", "Perfil", null);
                }
            }

            
        }

        // GET: Reporte_vacunas/Edit/5
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
                        TempData["errorMessage"] = "Debe seleccionar un reporte de vacunas";
                        return RedirectToAction("Index");
                    }
                    Reporte_vacunas reporte_vacunas = db.Reporte_vacunas.Find(id);
                    if (reporte_vacunas == null)
                    {
                        TempData["errorMessage"] = "No se ha encontrado el reporte de vacunas";
                        return RedirectToAction("Index");
                    }
                    ViewBag.id_mascota = new SelectList(db.Mascota, "id_mascota", "nombre_mascota", reporte_vacunas.id_mascota);
                    ViewBag.id_vacuna = new SelectList(db.Vacunas, "id_vacuna", "vacuna", reporte_vacunas.id_vacuna);
                    return View(reporte_vacunas);
                }
                else
                {
                    ViewBag.Title = "Perfil";
                    return RedirectToAction("Index", "Perfil", null);
                }
            }

            
        }

        // POST: Reporte_vacunas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_vacuna_aplicada,id_mascota,id_vacuna,fecha_aplicada,fecha_a_aplicar_nueva")] Reporte_vacunas reporte_vacunas)
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
                        db.Entry(reporte_vacunas).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    ViewBag.id_mascota = new SelectList(db.Mascota, "id_mascota", "nombre_mascota", reporte_vacunas.id_mascota);
                    ViewBag.id_vacuna = new SelectList(db.Vacunas, "id_vacuna", "vacuna", reporte_vacunas.id_vacuna);
                    return View(reporte_vacunas);
                }
                else
                {
                    ViewBag.Title = "Perfil";
                    return RedirectToAction("Index", "Perfil", null);
                }
            }

           
        }

        // GET: Reporte_vacunas/Delete/5
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
                        TempData["errorMessage"] = "Debe seleccionar un reporte de vacunas";
                        return RedirectToAction("Index");
                    }
                    Reporte_vacunas reporte_vacunas = db.Reporte_vacunas.Find(id);
                    if (reporte_vacunas == null)
                    {
                        TempData["errorMessage"] = "No se ha encontrado el reporte de vacunas";
                        return RedirectToAction("Index");
                    }
                    return View(reporte_vacunas);
                }
                else
                {
                    ViewBag.Title = "Perfil";
                    return RedirectToAction("Index", "Perfil", null);
                }
            }

            
        }

        // POST: Reporte_vacunas/Delete/5
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
                    Reporte_vacunas reporte_vacunas = db.Reporte_vacunas.Find(id);
                    db.Reporte_vacunas.Remove(reporte_vacunas);
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
