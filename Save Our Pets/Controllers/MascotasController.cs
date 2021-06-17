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
    public class MascotasController : Controller
    {
        private save_our_petsEntities db = new save_our_petsEntities();

        //Modelos
        MascotaModel model_mascota = new MascotaModel();
        EstadoSaludModel saludModel = new EstadoSaludModel();
        ImagenModel imodel = new ImagenModel();
        reporteVacunasModel repvacunasModel = new reporteVacunasModel();
        reporteSeguimientoModel seguimientoModel = new reporteSeguimientoModel();

        //Clases
        DatosMascota datos_completos = new DatosMascota();
        Raza raza_mascota = new Raza();
        Mascota informacionMascota = new Mascota();
        Estado_salud informacion_salud = new Estado_salud();
        img_mascota imagenMascota = new img_mascota();
        Reporte_vacunas reporteVacunas = new Reporte_vacunas();
        reporte_seguimiento reporteSeguimiento = new reporte_seguimiento();

        // GET: Mascotas/Edit/5
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
                        TempData["errorMessage"] = "Debe seleccionar un registro de mascota";
                        return RedirectToAction("Index");
                    }
                    DatosMascota editarMascota = new DatosMascota();
                    editarMascota.infoMascota = db.Mascota.Find(id);

                    int id_est_saludo = saludModel.devolver_id_Salud(editarMascota.infoMascota.id_mascota);
                    int id_imagen_act = imodel.detectar_id_imagen(editarMascota.infoMascota.id_mascota);

                    editarMascota.saludMascota = db.Estado_salud.Find(id_est_saludo);
                    editarMascota.imgMascota = db.img_mascota.Find(id_imagen_act);

                    if (editarMascota == null)
                    {
                        TempData["errorMessage"] = "No se ha encontrado la mascota";
                        return RedirectToAction("Index");
                    }
                    ViewBag.id_especie = new SelectList(db.Especie, "id_especie", "nombre", editarMascota.infoMascota.id_especie);
                    ViewBag.id_estado = new SelectList(db.Estado_mascota, "id_estado", "nombre_estado", editarMascota.infoMascota.id_estado);
                    ViewBag.id_raza = new SelectList(db.Raza, "id_raza", "nombre", editarMascota.infoMascota.id_raza);
                    ViewBag.id_raza_gatos = new SelectList(db.Raza.Where(r => r.id_especie == 1), "id_raza", "nombre");
                    ViewBag.id_raza_perros = new SelectList(db.Raza.Where(r => r.id_especie == 2), "id_raza", "nombre");
                    ViewBag.id_raza_otros = new SelectList(db.Raza.Where(r => r.id_especie == 3), "id_raza", "nombre");
                    return View(editarMascota);
                }
                else
                {
                    ViewBag.Title = "Perfil";
                    return RedirectToAction("Index", "Perfil", null);
                }
            }


            
        }

        // POST: Mascotas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DatosMascota dataMascota)
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
                    //inicializamos el modelo de la imagen

                    HttpPostedFileBase file = Request.Files["ImageData"];

                    //llenamos los campos de la imagen
                    //dataMascota.imgMascota.imagen_mascota = imodel.Convertir_a_Bytes(file);


                    //Procedemos a la inserción 
                    int resultado1, resultado2, resultado3;

                    resultado1 = model_mascota.Update(dataMascota.infoMascota, dataMascota.infoMascota.id_mascota);

                    if (resultado1 > 0)
                    {
                        int id_est_saludo = saludModel.devolver_id_Salud(dataMascota.infoMascota.id_mascota);
                        int id_imagen_act = imodel.detectar_id_imagen(dataMascota.infoMascota.id_mascota);

                        dataMascota.saludMascota.id_estado_salud = id_est_saludo;
                        //  dataMascota.imgMascota.id_imagen = id_imagen_act;

                        resultado2 = saludModel.Update(dataMascota.saludMascota, id_est_saludo);
                        //  resultado3 = imodel.Update(dataMascota.imgMascota, id_imagen_act);


                        if (resultado2 > 0)
                        {
                            TempData["successMessage"] = "Se ha modificado exitosamente";
                            return RedirectToAction("ListaMascotas");
                        }

                    }
                    else
                    {
                        TempData["errorMessage"] = "No se ha podido modificar, intentelo de nuevo";
                    }

                    return RedirectToAction("Edit");
                }
                else
                {
                    ViewBag.Title = "Perfil";
                    return RedirectToAction("Index", "Perfil", null);
                }
            }


            

        }

        // GET: Mascotas/Delete/5
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
                        TempData["errorMessage"] = "Debe seleccionar un registro de mascota";
                        return RedirectToAction("Index");
                    }
                    Mascota mascota = db.Mascota.Find(id);
                    if (mascota == null)
                    {
                        TempData["errorMessage"] = "No se ha encontrado la mascota";
                        return RedirectToAction("Index");
                    }
                    return View(mascota);
                }
                else
                {
                    ViewBag.Title = "Perfil";
                    return RedirectToAction("Index", "Perfil", null);
                }
            }

           
        }

        // POST: Mascotas/Delete/5
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
                    Mascota mascota = db.Mascota.Find(id);
                    db.Mascota.Remove(mascota);
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

        // GET: Mascotas
        public ActionResult ListaMascotas(string currentFilter, string especie, string raza, string filtroRaza, int? page)
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
                    ViewBag.id_razas = (db.Raza.ToList());
                    ViewBag.id_especies = db.Especie.ToList();

                    if (especie != null)
                    {
                        page = 1;
                        if (!String.IsNullOrEmpty(especie))
                        {
                            int especieId = Convert.ToInt32(especie);
                            ViewBag.id_razas = (db.Raza.Where(s => s.id_especie == especieId).ToList());
                        }
                    }
                    else
                    {
                        especie = currentFilter;
                    }

                    ViewBag.CurrentFilter = especie;

                    if (raza != null)
                    {
                        page = 1;
                    }
                    else
                    {
                        raza = filtroRaza;
                    }
                    ViewBag.FiltroRaza = raza;

                    var mascota = model_mascota.Listar();

                    if (!string.IsNullOrEmpty(especie))
                    {
                        mascota = model_mascota.BuscarPorEspecie(especie);
                    }
                    /**/
                    if (!string.IsNullOrEmpty(especie) && !string.IsNullOrEmpty(raza))
                    {
                        mascota = model_mascota.BuscarPorRazayEspecie(raza, especie);

                    }


                    if (!string.IsNullOrEmpty(raza))
                    {
                        mascota = model_mascota.BuscarporRaza(raza);
                    }
                    /**/

                    int pageSize = 10;
                    int pageNumber = (page ?? 1);

                    return View(mascota.ToPagedList(pageNumber, pageSize));
                }
                else
                {
                    ViewBag.Title = "Perfil";
                    return RedirectToAction("Index", "Perfil", null);
                }
            }


            
        }


        //VER
        public ActionResult VER(int id)
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
                        TempData["errorMessage"] = "Debe seleccionar un registro de mascota";
                        return RedirectToAction("Index");
                    }

                   

                    List<int> lista_imagenes = imodel.detectar_imagenes_por_mascota(id);
                    int cantidad_imagenes = imodel.detectarImagen(id);
                    Mascota mascota = db.Mascota.Find(id);

                    if (mascota == null)
                    {
                        TempData["errorMessage"] = "No se ha encontrado la mascota";
                        return RedirectToAction("Index");
                    }

                    DatosMascota datos_de_mascota = new DatosMascota
                    {
                        infoMascota = db.Mascota.Find(id),
                        saludMascota = new Estado_salud
                        {
                            id_mascota = id,
                            id_estado_salud = saludModel.devolver_id_Salud(id),
                            descripcion_salud = saludModel.devolver_estado_salud(id)
                        },

                        imgMascota = new img_mascota
                        {
                            id_mascota = id
                        },

                        id_imagenes = lista_imagenes,
                        infoVacunas = repvacunasModel.obtenerlistadoVacunas(id),
                        listaSeguimiento = seguimientoModel.obtenerlistadodeVisitas(id)

                    };

                    return View(datos_de_mascota);
                }
                else
                {
                    ViewBag.Title = "Perfil";
                    return RedirectToAction("Index", "Perfil", null);
                }
            }

            
        }


        // GET: Mascotas
        public ActionResult Agregar()
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
                    ViewBag.id_estado = new SelectList(db.Estado_mascota, "id_estado", "nombre_estado");
                    ViewBag.id_raza = new SelectList(db.Raza, "id_raza", "nombre");
                    ViewBag.id_raza_gatos = new SelectList(db.Raza.Where(r => r.id_especie == 1), "id_raza", "nombre");
                    ViewBag.id_raza_perros = new SelectList(db.Raza.Where(r => r.id_especie == 2), "id_raza", "nombre");
                    ViewBag.id_raza_otros = new SelectList(db.Raza.Where(r => r.id_especie == 3), "id_raza", "nombre");
                    return View();
                }
                else
                {
                    ViewBag.Title = "Perfil";
                    return RedirectToAction("Index", "Perfil", null);
                }
            }

            
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Agregar(DatosMascota dataMascota)
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

                    //inicializamos el modelo de la imagen
                    //dataMascota.imgMascota.id_mascota = 0;
                    //dataMascota.imgMascota.imagen_mascota = null;

                    HttpPostedFileBase file = Request.Files["ImageData"];

                    //llenamos los campos de la imagen
                    //dataMascota.imgMascota.imagen_mascota = imodel.Convertir_a_Bytes(file);


                    //Procedemos a la insersión 
                    int resultado1, resultado2, resultado3;

                    resultado1 = model_mascota.Insert(dataMascota.infoMascota);

                    if (resultado1 > 0)
                    {
                        //Obtenemos el id de la mascota
                        int id_masc = dataMascota.infoMascota.id_mascota;

                        //Lo asignamos a los datos faltantes 
                        dataMascota.saludMascota.id_mascota = id_masc;
                        //dataMascota.imgMascota.id_mascota = id_masc;

                        resultado2 = saludModel.Insert(dataMascota.saludMascota);
                        //resultado3 = imodel.Insert(dataMascota.imgMascota);


                        if (resultado2 > 0)
                        {
                            TempData["successMessage"] = "Se ha agregado la mascota";
                            return RedirectToAction("ListaMascotas");
                        }

                    }
                    else
                    {
                        TempData["errorMessage"] = "No se ha podido agregar, intentelo de nuevo";
                    }

                    return RedirectToAction("Agregar");
                }
                else
                {
                    ViewBag.Title = "Perfil";
                    return RedirectToAction("Index", "Perfil", null);
                }
            }


            

        }


    }
}
