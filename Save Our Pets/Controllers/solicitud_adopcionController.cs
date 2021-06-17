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
    public class solicitud_adopcionController : Controller
    {
        private save_our_petsEntities db = new save_our_petsEntities();
        solicitudModel solicitud_model = new solicitudModel();

        // GET: solicitud_adopcion
        public ActionResult Index(string currentFilter, string nombreCandidato, string estadoSolicitud, string filtroTipo, int? page)
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


                    if (nombreCandidato != null)
                    {
                        page = 1;
                    }
                    else
                    {
                        nombreCandidato = currentFilter;
                    }

                    ViewBag.CurrentFilter = nombreCandidato;

                    if (estadoSolicitud != null)
                    {
                        page = 1;
                    }
                    else
                    {
                        estadoSolicitud = filtroTipo;
                    }
                    ViewBag.FiltroTipo = estadoSolicitud;

                    var solicitud_adopcion = solicitud_model.Listar();

                    if (!string.IsNullOrEmpty(nombreCandidato) && string.IsNullOrEmpty(estadoSolicitud))
                    {
                        solicitud_adopcion = solicitud_model.BuscarNombre(nombreCandidato);
                    }
                    /**/
                    if (!string.IsNullOrEmpty(nombreCandidato) && !string.IsNullOrEmpty(estadoSolicitud))
                    {
                        solicitud_adopcion = solicitud_model.BuscarNombreyEstado(nombreCandidato, estadoSolicitud);
                        
                    }


                    if (!string.IsNullOrEmpty(estadoSolicitud) && string.IsNullOrEmpty(nombreCandidato))
                    {
                        solicitud_adopcion = solicitud_model.solicitudesporEstado(estadoSolicitud);
                        
                    }
                    /**/

                    int pageSize = 10;
                    int pageNumber = (page ?? 1);

                    return View(solicitud_adopcion.ToPagedList(pageNumber, pageSize));

                }
                else
                {
                    ViewBag.Title = "Perfil";
                    return RedirectToAction("Index", "Perfil", null);
                }

                
            }

           
        }

        // GET: solicitud_adopcion/Details/5
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
                        TempData["errorMessage"] = "Debe seleccionar una solicitud";
                        return RedirectToAction("Index");
                    }
                    solicitud_adopcion solicitud_adopcion = db.solicitud_adopcion.Find(id);
                    if (solicitud_adopcion == null)
                    {
                        TempData["errorMessage"] = "No se ha encontrado la solicitud";
                        return RedirectToAction("Index");
                    }
                    return View(solicitud_adopcion);
                }
                else
                {
                    ViewBag.Title = "Perfil";
                    return RedirectToAction("Index", "Perfil", null);
                }

            }
                    
        }

        // GET: solicitud_adopcion/Create
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
                    ViewBag.id_usuario = new SelectList(db.Usuarios, "id_usuario", "nombres");
                    ViewBag.id_empleado = new SelectList(db.Usuarios, "id_usuario", "nombres");
                    return View();
                }
                else
                {
                    ViewBag.Title = "Perfil";
                    return RedirectToAction("Index", "Perfil", null);
                }
            }


            
        }

        // POST: solicitud_adopcion/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_solicitud,id_usuario,empleo_fijo,id_especie,requisitos_condiciones,estado_solicitud,id_empleado")] solicitud_adopcion solicitud_adopcion)
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
                    solicitud_adopcion.id_empleado = (int)System.Web.HttpContext.Current.Session["id_usuario"];

                    if (ModelState.IsValid)
                    {
                        db.solicitud_adopcion.Add(solicitud_adopcion);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }

                    ViewBag.id_especie = new SelectList(db.Especie, "id_especie", "nombre", solicitud_adopcion.id_especie);
                    ViewBag.id_usuario = new SelectList(db.Usuarios, "id_usuario", "nombres", solicitud_adopcion.id_usuario);
                    ViewBag.id_empleado = new SelectList(db.Usuarios, "id_usuario", "nombres", solicitud_adopcion.id_empleado);
                    return View(solicitud_adopcion);
                }
                else
                {
                    ViewBag.Title = "Perfil";
                    return RedirectToAction("Index", "Perfil", null);
                }
            }

           
        }

        // GET: solicitud_adopcion/Edit/5
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
                        TempData["errorMessage"] = "Debe seleccionar una solicitud";
                        return RedirectToAction("Index");
                    }
                    solicitud_adopcion solicitud_adopcion = db.solicitud_adopcion.Find(id);
                    if (solicitud_adopcion == null)
                    {
                        TempData["errorMessage"] = "No se ha encontrado la solicitud";
                        return RedirectToAction("Index");
                    }
                    ViewBag.id_especie = new SelectList(db.Especie, "id_especie", "nombre", solicitud_adopcion.id_especie);
                    ViewBag.id_usuario = new SelectList(db.Usuarios, "id_usuario", "nombres", solicitud_adopcion.id_usuario);
                    ViewBag.id_empleado = new SelectList(db.Usuarios.Where(c => c.id_tipo != 3), "id_usuario", "nombres", solicitud_adopcion.id_empleado);
                    return View(solicitud_adopcion);
                }
                else
                {
                    ViewBag.Title = "Perfil";
                    return RedirectToAction("Index", "Perfil", null);
                }
            }


            
        }

        // POST: solicitud_adopcion/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_solicitud,id_usuario,empleo_fijo,id_especie,requisitos_condiciones,estado_solicitud,id_empleado")] solicitud_adopcion solicitud_adopcion)
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
                    solicitud_adopcion.id_empleado = (int)System.Web.HttpContext.Current.Session["id_usuario"];

                    if (ModelState.IsValid)
                    {
                        db.Entry(solicitud_adopcion).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    ViewBag.id_especie = new SelectList(db.Especie, "id_especie", "nombre", solicitud_adopcion.id_especie);
                    ViewBag.id_usuario = new SelectList(db.Usuarios, "id_usuario", "nombres", solicitud_adopcion.id_usuario);
                    ViewBag.id_empleado = new SelectList(db.Usuarios.Where(c => c.id_tipo != 3), "id_usuario", "nombres", solicitud_adopcion.id_empleado);
                    return View(solicitud_adopcion);
                }
                else
                {
                    ViewBag.Title = "Perfil";
                    return RedirectToAction("Index", "Perfil", null);
                }
            }

            
        }

        // GET: solicitud_adopcion/Delete/5
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
                        TempData["errorMessage"] = "Debe seleccionar una solicitud";
                        return RedirectToAction("Index");
                    }
                    solicitud_adopcion solicitud_adopcion = db.solicitud_adopcion.Find(id);
                    if (solicitud_adopcion == null)
                    {
                        TempData["errorMessage"] = "No se ha encontrado la solicitud";
                        return RedirectToAction("Index");
                    }
                    return View(solicitud_adopcion);
                }
                else
                {
                    ViewBag.Title = "Perfil";
                    return RedirectToAction("Index", "Perfil", null);
                }
            }

            
        }

        // POST: solicitud_adopcion/Delete/5
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
                    solicitud_adopcion solicitud_adopcion = db.solicitud_adopcion.Find(id);
                    db.solicitud_adopcion.Remove(solicitud_adopcion);
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
