using Save_Our_Pets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Save_Our_Pets.Controllers
{
    public class Adopciones2Controller : Controller
    {
        private save_our_petsEntities db = new save_our_petsEntities();
        UsuariosModel userModel = new UsuariosModel();
        solicitudModel solModel = new solicitudModel();
        DireccionModel dirModel = new DireccionModel();


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
            }else if(adopcion.infoUsuario.contra != pass2)
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

                if(seinserto > 0)
                {
                    id_usuario_asig = userModel.obtenerIDUsuario(adopcion.infoUsuario.email, adopcion.infoUsuario.nombres, adopcion.infoUsuario.apellidos);
                    //Estado solicitud = 0 = Enviado y en espera de aprobación
                    adopcion.datosSolicitud.estado_solicitud = 0;
                    //insertamos los datos a la solicitud
                    adopcion.datosSolicitud.id_usuario = id_usuario_asig;
                    adopcion.datosSolicitud.id_empleado = id_usuario_asig;
                    
                    int sol_adopcion = solModel.Insert(adopcion.datosSolicitud);

                    if (sol_adopcion > 0) {
                        int dataPersonal = 0;

                        //insertamos los datos a la dirección
                        adopcion.datosPersonalesUsuario.id_usuario = id_usuario_asig;
                        dataPersonal = dirModel.Insert(adopcion.datosPersonalesUsuario);

                        if (dataPersonal > 0)
                        {
                            string nombre_completo = adopcion.infoUsuario.nombres + " " + adopcion.infoUsuario.apellidos;
                            CorreoContactanos correoEnviable = new CorreoContactanos(adopcion.infoUsuario.email,nombre_completo, adopcion.infoUsuario.telefono);
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

    }
}