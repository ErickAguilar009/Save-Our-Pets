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
    public class reporte_seguimientoController : Controller
    {
        private save_our_petsEntities db = new save_our_petsEntities();
        reporteSeguimientoModel seguimientoModel = new reporteSeguimientoModel();
        UsuariosModel usuariosModel = new UsuariosModel();
        MascotaModel mascotaModel = new MascotaModel();
        

        // GET: reporte_seguimiento
        public ActionResult Index(string currentFilter, string nombreMascota, string nombreAdoptador, string filtroTipo, int? page)
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

                    if (nombreAdoptador != null)
                    {
                        page = 1;
                    }
                    else
                    {
                        nombreAdoptador = filtroTipo;
                    }
                    ViewBag.FiltroTipo = nombreAdoptador;


                    var reporte_seguimiento = seguimientoModel.Listar();

                    if (!string.IsNullOrEmpty(nombreMascota) && string.IsNullOrEmpty(nombreAdoptador))
                    {
                        reporte_seguimiento = seguimientoModel.BuscarporMascota(nombreMascota);
                    }
                    /**/
                    if (!string.IsNullOrEmpty(nombreMascota) && !string.IsNullOrEmpty(nombreAdoptador))
                    {
                        reporte_seguimiento = seguimientoModel.BuscarNombreyMascota(nombreAdoptador, nombreMascota);

                    }


                    if (!string.IsNullOrEmpty(nombreAdoptador) && string.IsNullOrEmpty(nombreMascota))
                    {
                        reporte_seguimiento = seguimientoModel.BuscarporAdoptador(nombreAdoptador);
                    }
                    /**/

                    int pageSize = 10;
                    int pageNumber = (page ?? 1);

                    return View(reporte_seguimiento.ToPagedList(pageNumber, pageSize));
                }
                else
                {
                    ViewBag.Title = "Perfil";
                    return RedirectToAction("Index", "Perfil", null);
                }
            }


            


        }

        // GET: reporte_seguimiento/Details/5
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
                        TempData["errorMessage"] = "Debe seleccionar un reporte de seguimiento";
                        return RedirectToAction("Index");
                    }
                    reporte_seguimiento reporte_seguimiento = db.reporte_seguimiento.Find(id);
                    if (reporte_seguimiento == null)
                    {
                        TempData["errorMessage"] = "No se ha encontrado el reporte de seguimiento";
                        return RedirectToAction("Index");
                    }
                    return View(reporte_seguimiento);
                }
                else
                {
                    ViewBag.Title = "Perfil";
                    return RedirectToAction("Index", "Perfil", null);
                }
            }

            
        }

        // GET: reporte_seguimiento/Create
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
                    Adopciones adopciones = new Adopciones();
                    reporte_seguimiento reporte = new reporte_seguimiento();
                    Usuarios usuarios = new Usuarios();
                    Mascota mascota = new Mascota();
                    ViewBag.mascota = "";
                    ViewBag.nombreResponsable = "";
                    ViewBag.id_adopcion = new SelectList(db.Adopciones, "id_adopcion", "id_adopcion");

                    if (id != null)
                    {
                        adopciones = seguimientoModel.obtenerIdAdopcion(Convert.ToInt32(id));
                        usuarios = usuariosModel.obtenerUsuario(adopciones.id_usuario);
                        mascota = mascotaModel.obtenerMascotaporID(adopciones.id_mascota);
                        reporte.id_adopcion = adopciones.id_adopcion;


                        ViewBag.id_adopcion = new SelectList(db.Adopciones.Where(a => a.id_adopcion == adopciones.id_adopcion), "id_adopcion", "Mascota.nombre_mascota");
                        ViewBag.mascota = mascota.nombre_mascota;
                        ViewBag.nombreResponsable = usuarios.nombres + " " + usuarios.apellidos;

                    }

                    return View(reporte);
                }
                else
                {
                    ViewBag.Title = "Perfil";
                    return RedirectToAction("Index", "Perfil", null);
                }
            }

            
        }

        // POST: reporte_seguimiento/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_seguimiento,id_adopcion,comentario,fecha_visita")] reporte_seguimiento reporte_seguimiento)
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
                    int contador = seguimientoModel.contarVisitas(reporte_seguimiento.id_seguimiento);
                    reporte_seguimiento.visitas_seguimiento = contador + 1;

                    if (ModelState.IsValid)
                    {
                        db.reporte_seguimiento.Add(reporte_seguimiento);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }

                    ViewBag.id_adopcion = new SelectList(db.Adopciones, "id_adopcion", "id_adopcion", reporte_seguimiento.id_adopcion);
                    return View(reporte_seguimiento);
                }
                else
                {
                    ViewBag.Title = "Perfil";
                    return RedirectToAction("Index", "Perfil", null);
                }
            }

            
        }

        // GET: reporte_seguimiento/Edit/5
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
                        TempData["errorMessage"] = "Debe seleccionar un reporte de seguimiento";
                        return RedirectToAction("Index");
                    }
                    reporte_seguimiento reporte_seguimiento = db.reporte_seguimiento.Find(id);
                    if (reporte_seguimiento == null)
                    {
                        TempData["errorMessage"] = "No se ha encontrado el reporte de seguimiento";
                        return RedirectToAction("Index");
                    }
                    ViewBag.id_adopcion = new SelectList(db.Adopciones, "id_adopcion", "id_adopcion", reporte_seguimiento.id_adopcion);
                    return View(reporte_seguimiento);
                }
                else
                {
                    ViewBag.Title = "Perfil";
                    return RedirectToAction("Index", "Perfil", null);
                }
            }

            
        }

        // POST: reporte_seguimiento/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_seguimiento,id_adopcion,visitas_seguimiento,comentario,fecha_visita")] reporte_seguimiento reporte_seguimiento)
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
                        db.Entry(reporte_seguimiento).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    ViewBag.id_adopcion = new SelectList(db.Adopciones, "id_adopcion", "id_adopcion", reporte_seguimiento.id_adopcion);
                    return View(reporte_seguimiento);
                }
                else
                {
                    ViewBag.Title = "Perfil";
                    return RedirectToAction("Index", "Perfil", null);
                }
            }

           
        }

        // GET: reporte_seguimiento/Delete/5
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
                        TempData["errorMessage"] = "Debe seleccionar un reporte de seguimiento";
                        return RedirectToAction("Index");
                    }
                    reporte_seguimiento reporte_seguimiento = db.reporte_seguimiento.Find(id);
                    if (reporte_seguimiento == null)
                    {
                        TempData["errorMessage"] = "No se ha encontrado el reporte de seguimiento";
                        return RedirectToAction("Index");
                    }
                    return View(reporte_seguimiento);
                }
                else
                {
                    ViewBag.Title = "Perfil";
                    return RedirectToAction("Index", "Perfil", null);
                }
            }

            
        }

        // POST: reporte_seguimiento/Delete/5
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
                    reporte_seguimiento reporte_seguimiento = db.reporte_seguimiento.Find(id);
                    db.reporte_seguimiento.Remove(reporte_seguimiento);
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
