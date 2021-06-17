using Save_Our_Pets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Save_Our_Pets.Views
{
    public class InicioController : Controller
    {
        MascotaModel mascotaModel = new MascotaModel();
        reporteRescateModel reporteRescate = new reporteRescateModel();
        ImagenModel imodel = new ImagenModel();
        // GET: Inicio
        public ActionResult Index()
        {

            ViewBag.Title = "Inicio";
            return View();
        }

        public ActionResult AcercaDe()
        {
            ViewBag.Title = "Acerca De";
            return View();
        }

        public ActionResult Adopciones()
        {
            ViewBag.Title = "Adopciones";

            List<img_mascota> img_Mascotas = imodel.imagenes_en_general();

            return View(img_Mascotas);
        }

        public ActionResult Contactanos()
        {
            ViewBag.Title = "Contáctanos";
            return View();
        }

        [HttpPost]
        public ActionResult Contactanos(string nombre, string apellido, string telefono, string mail, string asunto, string mensaje, string mascota, string colorpelo, string descripcion, string direccion)
        {
            ViewBag.Title = "Contáctanos";
            if (String.IsNullOrWhiteSpace(nombre))
            {
                ModelState.AddModelError("nombre", "Debe colocar su nombre");

                TempData["errorMessage"] = "Debe colocar su nombre";
            }
            else if (String.IsNullOrWhiteSpace(apellido))
            {

                ModelState.AddModelError("apellido", "Debe colocar su apellido");
                TempData["errorMessage"] = "Debe colocar su apellido";
            }
            else if (String.IsNullOrWhiteSpace(telefono))
            {

                ModelState.AddModelError("telefono", "Debe colocar su teléfono");
                TempData["errorMessage"] = "Debe colocar su teléfono";
            }
            else if (String.IsNullOrWhiteSpace(mail))
            {

                ModelState.AddModelError("email", "Debe colocar su e-mail");
                TempData["errorMessage"] = "Debe colocar su e-mail";
            }
            else
            {
                if (asunto != "Reporte de Mascota")
                {

                    if (String.IsNullOrWhiteSpace(mensaje))
                    {

                        ModelState.AddModelError("mensaje", "Debe colocar el mensaje a mandar");
                        TempData["errorMessage"] = "Debe colocar el mensaje";
                    }

                    string nombre_completo = nombre + " " + apellido;
                    CorreoContactanos correoEnviable = new CorreoContactanos(mail, nombre_completo, asunto, mensaje, telefono);
                    TempData["successMessage"] = "Se ha enviado su mensaje correctamente";
                    return RedirectToAction("Contactanos");
                }
                else
                {
                   
                    if (String.IsNullOrWhiteSpace(mascota))
                    {

                        ModelState.AddModelError("mascota", "Debe seleccionar una mascota");
                        TempData["errorMessage"] = "Debe colocar una mascota ";
                    }else if (String.IsNullOrWhiteSpace(descripcion))
                    {
                        ModelState.AddModelError("descripcion", "Debe escribir una descripcion de la mascota");
                        TempData["errorMessage"] = "Debe colocar una descripcion ";
                    }
                    else if (String.IsNullOrWhiteSpace(direccion))
                    {
                        ModelState.AddModelError("direccion", "Debe escribir la dirección");
                        TempData["errorMessage"] = "Debe colocar la dirección para ayudar a ubicarlo mejor ";
                    }
                    else
                    {
                        //creamos la mascota 
                        Mascota nuevaMascota = new Mascota();
                        nuevaMascota.nombre_mascota = "Sin nombre";
                        nuevaMascota.id_especie = Convert.ToInt32(mascota);
                        nuevaMascota.id_raza = mascotaModel.obtenerRazaDesconocida(Convert.ToInt32(mascota));
                        nuevaMascota.color_pelo = colorpelo;
                        nuevaMascota.peso = 0;
                        nuevaMascota.esterilizado = "No";
                        nuevaMascota.id_estado = 3;
                        //insertamos la mascota
                        int paso = mascotaModel.Insert(nuevaMascota);

                        if (paso > 0)
                        {
                            //creamos el reporte de rescate
                            int id_mascota_creada = nuevaMascota.id_mascota;
                            //creamos su estado de salud
                            Estado_salud estado_Salud = new Estado_salud();
                            estado_Salud.id_mascota = id_mascota_creada;
                            estado_Salud.descripcion_salud = "Desconocida";

                            reporte_rescate nuevo_repote = new reporte_rescate();
                            nuevo_repote.id_mascota = id_mascota_creada;
                            nuevo_repote.email = mail;
                            nuevo_repote.telefono = telefono;
                            nuevo_repote.direccion = direccion;
                            nuevo_repote.descripcion = descripcion;
                            nuevo_repote.fecha_reporte = DateTime.Now;

                            int paso2 = reporteRescate.Insert(nuevo_repote);
                            if(paso2 > 0)
                            {
                                TempData["successMessage"] = "Su reporte ha sido enviado exitosamente";
                            }
                            else
                            {
                                mascotaModel.Remove(id_mascota_creada);
                                TempData["errorMessage"] = "Ha ocurrido un error en nuestros servidores";
                                return View();
                            }
                        }
                        else
                        {
                            TempData["errorMessage"] = "Ha ocurrido un error en nuestros servidores.";
                            return View();
                        }
                    }



                }
               

            }
            return RedirectToAction("Contactanos");

        }
    }
}