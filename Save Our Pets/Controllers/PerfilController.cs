using Save_Our_Pets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Save_Our_Pets.Controllers
{
    public class PerfilController : Controller
    {
        private save_our_petsEntities db = new save_our_petsEntities();
        UsuariosModel userModel = new UsuariosModel();
        solicitudModel solModel = new solicitudModel();
        DireccionModel dirModel = new DireccionModel();

        // GET: Perfil
        public ActionResult Index()
        {
            DatosAdopcion adopcion = new DatosAdopcion();
            Usuarios usuario = new Usuarios();
            direccion_usuario direccion_u = new direccion_usuario();
            solicitud_adopcion solic_u = new solicitud_adopcion();

            if (System.Web.HttpContext.Current.Session["usuario"] == null)
            {
                TempData["errorMessage"] = "Debe iniciar sesión antes de continuar";
                return RedirectToAction("IniciarSesion", "Login", null);
            }
            else
            {
                ViewBag.Title = "Perfil";

                if ((int)System.Web.HttpContext.Current.Session["tipo_usuario"] != 3)
                {
                    ViewBag.Title = "Administración";
                    return RedirectToAction("Index", "Administracion", null);
                }
                else
                {
                    int id_usuario_actual = (int)System.Web.HttpContext.Current.Session["id_usuario"];
                    usuario = db.Usuarios.Find(id_usuario_actual);
                    direccion_u = dirModel.buscarDireccionporID(id_usuario_actual);
                    solic_u = solModel.buscarsolicitudporID(id_usuario_actual);

                    adopcion.infoUsuario = usuario;
                    adopcion.datosPersonalesUsuario = direccion_u;
                    adopcion.datosSolicitud = solic_u;
                    ViewBag.Direccion = "";
                    if (adopcion.datosPersonalesUsuario.direccion != null)
                    {
                        ViewBag.Direccion = adopcion.datosPersonalesUsuario.direccion;
                    }
                    return View(adopcion);
                }
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(DatosAdopcion datosdeAdopcion)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] == null)
            {
                TempData["errorMessage"] = "Debe iniciar sesión antes de continuar";
                return RedirectToAction("IniciarSesion", "Login", null);
            }
            else
            {
                if ((int)System.Web.HttpContext.Current.Session["tipo_usuario"] == 3)
                {
                    int id_usuario_actual = (int)System.Web.HttpContext.Current.Session["id_usuario"];

                    if (String.IsNullOrEmpty(datosdeAdopcion.datosPersonalesUsuario.departamento))
                    {
                        ModelState.AddModelError("nombre", "Debe seleccionar un departamento");

                        TempData["errorMessage"] = "Debe seleccionar un departamento";
                    }
                    else if (String.IsNullOrEmpty(datosdeAdopcion.datosPersonalesUsuario.municipio))
                    {
                        ModelState.AddModelError("nombre", "Debe seleccionar un municipio");

                        TempData["errorMessage"] = "Debe seleccionar un municipio";
                    }
                    else if (String.IsNullOrEmpty(datosdeAdopcion.datosPersonalesUsuario.direccion))
                    {
                        ModelState.AddModelError("nombre", "Debe escribir su dirección");

                        TempData["errorMessage"] = "Debe escribir su dirección";
                    }
                    else if (String.IsNullOrEmpty(datosdeAdopcion.datosSolicitud.empleo_fijo.ToString()))
                    {
                        ModelState.AddModelError("nombre", "Debe seleccionar una opción en empleo fijo");

                        TempData["errorMessage"] = "Debe seleccionar una opción en empleo fijo";
                    }
                    else if (String.IsNullOrEmpty(datosdeAdopcion.datosSolicitud.requisitos_condiciones.ToString()))
                    {
                        ModelState.AddModelError("nombre", "Debe seleccionar una opción en términos y condiciones");

                        TempData["errorMessage"] = "Debe seleccionar una opción en términos y condiciones";
                    }
                    else
                    {
                        //obtenemos una versión antes de actualizar
                        solicitud_adopcion solicitud_u = new solicitud_adopcion();
                        direccion_usuario direcc_u = new direccion_usuario();
                        solicitud_u = solModel.buscarsolicitudporID(id_usuario_actual);
                        direcc_u = dirModel.buscarDireccionporID(id_usuario_actual);
                        //colocamos de manera fija el id usuario actual
                        datosdeAdopcion.datosPersonalesUsuario.id_usuario = id_usuario_actual;
                        datosdeAdopcion.datosSolicitud.id_usuario = id_usuario_actual;

                        //definimos el id de cada tabla con respecto al usuario actual
                        datosdeAdopcion.datosPersonalesUsuario.id_direccion = direcc_u.id_direccion;
                        datosdeAdopcion.datosSolicitud.id_solicitud = solicitud_u.id_solicitud;

                        //Datos a no modificar
                        datosdeAdopcion.datosSolicitud.id_especie = solicitud_u.id_especie;
                        datosdeAdopcion.datosSolicitud.id_empleado = solicitud_u.id_empleado;
                        datosdeAdopcion.datosSolicitud.estado_solicitud = solicitud_u.estado_solicitud;

                        int up_dir = dirModel.Update(datosdeAdopcion.datosPersonalesUsuario, direcc_u.id_direccion);
                        if (up_dir > 0)
                        {
                            int up_sol = solModel.Update(datosdeAdopcion.datosSolicitud, solicitud_u.id_solicitud);

                            if (up_sol > 0)
                            {

                                TempData["successMessage"] = "Se ha actualizado correctamente";
                                return RedirectToAction("Index");
                            }
                        }

                    }

                    DatosAdopcion adopcion = new DatosAdopcion();
                    Usuarios usuario = new Usuarios();
                    direccion_usuario direccion_u = new direccion_usuario();
                    solicitud_adopcion solic_u = new solicitud_adopcion();
                    ViewBag.Title = "Perfil";

                    usuario = db.Usuarios.Find(id_usuario_actual);
                    direccion_u = dirModel.buscarDireccionporID(id_usuario_actual);
                    solic_u = solModel.buscarsolicitudporID(id_usuario_actual);

                    adopcion.infoUsuario = usuario;
                    adopcion.datosPersonalesUsuario = direccion_u;
                    adopcion.datosSolicitud = solic_u;
                    ViewBag.Direccion = adopcion.datosPersonalesUsuario.direccion;
                    return View(adopcion);
                }
                else
                {
                    ViewBag.Title = "Admin";
                    return RedirectToAction("Index", "Administracion", null);
                }
            }




        }
    }
}