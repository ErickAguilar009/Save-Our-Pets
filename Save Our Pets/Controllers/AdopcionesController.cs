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
    public class AdopcionesController : Controller
    {
        private save_our_petsEntities db = new save_our_petsEntities();
        UsuariosModel userModel = new UsuariosModel();
        solicitudModel solModel = new solicitudModel();
        DireccionModel dirModel = new DireccionModel();
        MascotaModel mascotModel = new MascotaModel();

        // GET: Adopciones
        public ActionResult Inicio()
        {
            ViewBag.Title = "Solicitar Adopción";
            ViewBag.id_especie = new SelectList(db.Especie, "id_especie", "nombre");
            return View();
        }

        [HttpPost]
        public ActionResult Inicio(string pass2, DatosAdopcion adopcion)
        {
            int id_usuario_asig = 0;

            ViewBag.Title = "Solicitar Adopción";
            //retornamos la lista de especies
            ViewBag.id_especie = new SelectList(db.Especie, "id_especie", "nombre");

            if (String.IsNullOrWhiteSpace(adopcion.infoUsuario.nombres))
            {
                ModelState.AddModelError("nombre", "Debe colocar su nombre");

                TempData["errorMessage"] = "Debe colocar su nombre";
            }
            else if (String.IsNullOrWhiteSpace(adopcion.infoUsuario.apellidos))
            {

                ModelState.AddModelError("apellido", "Debe colocar su apellido");
                TempData["errorMessage"] = "Debe colocar su apellido";
            }
            else if (String.IsNullOrWhiteSpace(adopcion.infoUsuario.telefono))
            {

                ModelState.AddModelError("telefono", "Debe colocar su telefono");
                TempData["errorMessage"] = "Debe colocar su telefono";
            }
            else if (String.IsNullOrWhiteSpace(adopcion.infoUsuario.email))
            {

                ModelState.AddModelError("email", "Debe colocar su e-mail");
                TempData["errorMessage"] = "Debe colocar su e-mail";
            }
            else if (String.IsNullOrWhiteSpace(adopcion.infoUsuario.DUI))
            {

                ModelState.AddModelError("mensaje", "Debe colocar su n° de DUI:");
                TempData["errorMessage"] = "Debe colocar su DUI";
            }
            else if (String.IsNullOrWhiteSpace(adopcion.infoUsuario.fecha_nacimiento.ToString()))
            {

                ModelState.AddModelError("mensaje", "Debe colocar su fecha de nacimiento");
                TempData["errorMessage"] = "Debe colocar su fecha de nacimiento";
            }
            else if (String.IsNullOrWhiteSpace(adopcion.infoUsuario.contra))
            {

                ModelState.AddModelError("mensaje", "Debe colocar una contraseña");
                TempData["errorMessage"] = "Debe colocar una contraseña";
            }
            else if (String.IsNullOrEmpty(pass2))
            {
                ModelState.AddModelError("mensaje", "Debe confirmar su contraseña");
                TempData["errorMessage"] = "Debe confirmar su contraseña";
            }
            else if (adopcion.infoUsuario.contra != pass2)
            {
                ModelState.AddModelError("mensaje", "Las contraseñas no son iguales");
                TempData["errorMessage"] = "Las contraseñas ingresadas no son iguales";
            }
            else
            {
                //asignamos tipo cliente
                adopcion.infoUsuario.id_tipo = 3;
                //encriptamos la contraseña
                adopcion.infoUsuario.contra = SecurityUtils.EncriptarSHA2(adopcion.infoUsuario.contra);

                int seinserto = userModel.Insert(adopcion.infoUsuario);

                if (seinserto > 0)
                {
                    id_usuario_asig = userModel.obtenerIDUsuario(adopcion.infoUsuario.email, adopcion.infoUsuario.nombres, adopcion.infoUsuario.apellidos);
                    //Estado solicitud = 0 = Enviado y en espera de aprobación
                    adopcion.datosSolicitud.estado_solicitud = 0;
                    //insertamos los datos a la solicitud
                    adopcion.datosSolicitud.id_usuario = id_usuario_asig;
                    adopcion.datosSolicitud.id_empleado = id_usuario_asig;

                    int sol_adopcion = solModel.Insert(adopcion.datosSolicitud);

                    if (sol_adopcion > 0)
                    {
                        int dataPersonal = 0;

                        //insertamos los datos a la dirección
                        adopcion.datosPersonalesUsuario.id_usuario = id_usuario_asig;
                        dataPersonal = dirModel.Insert(adopcion.datosPersonalesUsuario);

                        if (dataPersonal > 0)
                        {
                            string nombre_completo = adopcion.infoUsuario.nombres + " " + adopcion.infoUsuario.apellidos;
                            CorreoContactanos correoEnviable = new CorreoContactanos(adopcion.infoUsuario.email, nombre_completo, adopcion.infoUsuario.telefono);
                            TempData["successMessage"] = "Se ha enviado su solicitud correctamente";
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            //error
                            TempData["errorMessage"] = "3 Ha ocurrido un error en nuestros servidores,\npor favor intentelo más tarde";
                        }
                    }
                    else
                    {
                        //error
                        TempData["errorMessage"] = "2 Ha ocurrido un error en nuestros servidores,\npor favor intentelo más tarde";
                    }

                }
                else
                {
                    //error
                    TempData["errorMessage"] = "Ha ocurrido un error en nuestros servidores,\npor favor intentelo más tarde";
                }


            }

            return View();
        }




        // GET: Adopciones
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
                    var adopciones = db.Adopciones.Include(a => a.Usuarios).Include(a => a.Mascota);
                    return View(adopciones.ToList());
                }
                else
                {
                    ViewBag.Title = "Perfil";
                    return RedirectToAction("Index", "Perfil", null);
                }
            }

            
        }

        // GET: Adopciones/Details/5
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
                    Adopciones adopciones = db.Adopciones.Find(id);
                    if (adopciones == null)
                    {
                        //return HttpNotFound();
                        TempData["errorMessage"] = "No se ha encontrado ese registro de adopción";
                        return RedirectToAction("Index");
                    }
                    return View(adopciones);
                }
                else
                {
                    ViewBag.Title = "Perfil";
                    return RedirectToAction("Index", "Perfil", null);
                }
            }


        }

        // GET: Adopciones/Create
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
                    ViewBag.id_usuario = new SelectList((from u in db.Usuarios
                                                         join s in db.solicitud_adopcion on u.id_usuario equals s.id_usuario
                                                         where u.id_tipo == 3 && s.estado_solicitud == 1
                                                         select u
                                                 ).ToList(), "id_usuario", "nombres");

                    //ViewBag.id_usuario = new SelectList(db.Usuarios, "id_usuario", "nombres");
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

        // POST: Adopciones/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_adopcion,id_usuario,id_mascota")] Adopciones adopciones)
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
                        db.Adopciones.Add(adopciones);
                        db.SaveChanges();
                        //Obtenemos los datos de la mascota seleccionada
                        Mascota nuevaMascota = new Mascota();
                        nuevaMascota = mascotModel.obtenerMascotaporID(adopciones.id_mascota);
                        //Actualizamos el estado
                        nuevaMascota.id_estado = 2;
                        mascotModel.Update(nuevaMascota, adopciones.id_mascota);

                        return RedirectToAction("Index");
                    }

                    //ViewBag.id_usuario = new SelectList(db.Usuarios.Where(u => u.id_tipo == 3), "id_usuario", "nombres", adopciones.id_usuario);
                    ViewBag.id_usuario = new SelectList((from u in db.Usuarios
                                                         join s in db.solicitud_adopcion on u.id_usuario equals s.id_usuario
                                                         where u.id_tipo == 3 && s.estado_solicitud == 1
                                                         select u
                                                         ).ToList(), "id_usuario", "nombres");
                    ViewBag.id_mascota = new SelectList(db.Mascota, "id_mascota", "nombre_mascota", adopciones.id_mascota);
                    return View(adopciones);
                }
                else
                {
                    ViewBag.Title = "Perfil";
                    return RedirectToAction("Index", "Perfil", null);
                }
            }



           
        }

        // GET: Adopciones/Edit/5
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
                    string url_anterior = "Adopciones";

                    if (id == null)
                    {
                        TempData["errorMessage"] = "Debe seleccionar un registro";
                        return RedirectToAction("Index");
                    }
                    Adopciones adopciones = db.Adopciones.Find(id);
                    if (adopciones == null)
                    {
                        //return HttpNotFound();
                        TempData["errorMessage"] = "No se ha encontrado ese registro de adopción";
                        return RedirectToAction("Index");
                    }
                    //ViewBag.id_usuario = new SelectList(db.Usuarios.Where(u => u.id_tipo == 3), "id_usuario", "nombres", adopciones.id_usuario);
                    ViewBag.id_usuario = new SelectList((from u in db.Usuarios
                                                         join s in db.solicitud_adopcion on u.id_usuario equals s.id_usuario
                                                         where u.id_tipo == 3 && s.estado_solicitud == 1
                                                         select u
                                                         ).ToList(), "id_usuario", "nombres");
                    ViewBag.id_mascota = new SelectList(db.Mascota, "id_mascota", "nombre_mascota", adopciones.id_mascota);
                    return View(adopciones);
                }
                else
                {
                    ViewBag.Title = "Perfil";
                    return RedirectToAction("Index", "Perfil", null);
                }
            }

            
        }

        // POST: Adopciones/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_adopcion,id_usuario,id_mascota")] Adopciones adopciones)
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
                        db.Entry(adopciones).State = EntityState.Modified;
                        db.SaveChanges();
                        //Obtenemos los datos de la mascota seleccionada
                        Mascota nuevaMascota = new Mascota();
                        nuevaMascota = mascotModel.obtenerMascotaporID(adopciones.id_mascota);
                        //Actualizamos el estado
                        nuevaMascota.id_estado = 2;
                        mascotModel.Update(nuevaMascota, adopciones.id_mascota);

                        return RedirectToAction("Index");
                    }
                    //ViewBag.id_usuario = new SelectList(db.Usuarios.Where(u => u.id_tipo == 3), "id_usuario", "nombres", adopciones.id_usuario);
                    ViewBag.id_usuario = new SelectList((from u in db.Usuarios
                                                         join s in db.solicitud_adopcion on u.id_usuario equals s.id_usuario
                                                         where u.id_tipo == 3 && s.estado_solicitud == 1
                                                         select u
                                                         ).ToList(), "id_usuario", "nombres");
                    ViewBag.id_mascota = new SelectList(db.Mascota, "id_mascota", "nombre_mascota", adopciones.id_mascota);
                    return View(adopciones);
                }
                else
                {
                    ViewBag.Title = "Perfil";
                    return RedirectToAction("Index", "Perfil", null);
                }
            }

            
        }

        // GET: Adopciones/Delete/5
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
                        TempData["errorMessage"] = "Debe seleccionar un registro";
                        return RedirectToAction("Index");
                    }
                    Adopciones adopciones = db.Adopciones.Find(id);
                    if (adopciones == null)
                    {
                        //return HttpNotFound();
                        TempData["errorMessage"] = "No se ha encontrado ese registro de adopción";
                        return RedirectToAction("Index");
                    }
                    return View(adopciones);
                }
                else
                {
                    ViewBag.Title = "Perfil";
                    return RedirectToAction("Index", "Perfil", null);
                }
            }

            
        }

        // POST: Adopciones/Delete/5
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
                    Adopciones adopciones = db.Adopciones.Find(id);
                    db.Adopciones.Remove(adopciones);
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
