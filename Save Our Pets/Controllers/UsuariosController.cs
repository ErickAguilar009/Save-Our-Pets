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
    public class UsuariosController : Controller
    {
        private save_our_petsEntities db = new save_our_petsEntities();
        DatosUsuario datoUsuario = new DatosUsuario();

        UsuariosModel userModel = new UsuariosModel();
        DireccionModel dirModel = new DireccionModel();

        // GET: Usuarios
        public ActionResult Index(string currentFilter, string nombreUsuario, string tipoUsuario, string filtroTipo,  int? page)
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
                    if (nombreUsuario != null)
                    {
                        page = 1;
                    }
                    else
                    {
                        nombreUsuario = currentFilter;
                    }

                    ViewBag.CurrentFilter = nombreUsuario;

                    if (tipoUsuario != null)
                    {
                        page = 1;
                    }
                    else
                    {
                        tipoUsuario = filtroTipo;
                    }
                    ViewBag.FiltroTipo = tipoUsuario;


                    var usuarios = userModel.Listar();

                    if (!string.IsNullOrEmpty(nombreUsuario))
                    {
                        usuarios = userModel.BuscarPorNombre(nombreUsuario);
                    }
                    /**/
                    if (!string.IsNullOrEmpty(nombreUsuario) && !string.IsNullOrEmpty(tipoUsuario))
                    {
                        usuarios = userModel.BuscarPorNombreyTipo(nombreUsuario, tipoUsuario);

                    }


                    if (!string.IsNullOrEmpty(tipoUsuario))
                    {
                        usuarios = userModel.BuscarPorTipo(tipoUsuario);
                    }
                    /**/

                    int pageSize = 10;
                    int pageNumber = (page ?? 1);

                    return View(usuarios.ToPagedList(pageNumber, pageSize));
                }
                else
                {
                    ViewBag.Title = "Perfil";
                    return RedirectToAction("Index", "Perfil", null);
                }
            }

            
        }

        // GET: Usuarios/Details/5
        public ActionResult Details(int? id)
        {
            DatosUsuario datosUsuario = new DatosUsuario();

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
                        TempData["errorMessage"] = "Debe seleccionar un Usuario";
                        return RedirectToAction("Index");
                    }

                    direccion_usuario dirUsuario = new direccion_usuario();

                    dirUsuario = dirModel.buscarDireccionporID((int)id);

                    Usuarios usuarios = db.Usuarios.Find(id);

                    datosUsuario.datos_usuario = usuarios;
                    datosUsuario.direcc_usuario = dirUsuario;
                    if (usuarios == null)
                    {
                        TempData["errorMessage"] = "No se ha encontrado el Usuario";
                        return RedirectToAction("Index");
                    }
                    return View(datosUsuario);
                }
                else
                {
                    ViewBag.Title = "Perfil";
                    return RedirectToAction("Index", "Perfil", null);
                }
            }

            
        }

        // GET: Usuarios/Create
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

                    ViewBag.id_tipo = new SelectList(db.Tipo_usuario, "id_tipo", "nombre_tipo");
                    return View();
                }
                else
                {
                    ViewBag.Title = "Perfil";
                    return RedirectToAction("Index", "Perfil", null);
                }
            }

        }

        // POST: Usuarios/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_usuario,id_tipo,nombres,apellidos,email,telefono,DUI,fecha_nacimiento,contra")] Usuarios usuarios)
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
                    //verificamos si el correo ya esta registrado
                    Usuarios usuario_existe = userModel.VerificarCorreo(usuarios.email);

                    if (usuario_existe == null)
                    {
                        if (ModelState.IsValid)
                        {
                            //Encriptamos la password
                            usuarios.contra = SecurityUtils.EncriptarSHA2(usuarios.contra);
                            db.Usuarios.Add(usuarios);
                            db.SaveChanges();
                            TempData["successMessage"] = "Usuario agregado de forma exitosa";
                            return RedirectToAction("Index");
                        }

                        foreach (ModelState modelState in ModelState.Values)
                        {
                            foreach (ModelError error in modelState.Errors)
                            {
                                TempData["errorMessage"] = error.ErrorMessage;
                            }
                        }

                        ViewBag.id_tipo = new SelectList(db.Tipo_usuario, "id_tipo", "nombre_tipo", usuarios.id_tipo);
                        return View(usuarios);

                    }
                    else
                    {
                        TempData["errorMessage"] = "El correo ingresado ya esta registrado!";
                        ViewBag.id_tipo = new SelectList(db.Tipo_usuario, "id_tipo", "nombre_tipo", usuarios.id_tipo);
                        return View(usuarios);
                    }
                }
                else
                {
                    ViewBag.Title = "Perfil";
                    return RedirectToAction("Index", "Perfil", null);
                }
            }

            
        }

        // GET: Usuarios/Edit/5
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
                        TempData["errorMessage"] = "Debe seleccionar un Usuario";
                        return RedirectToAction("Index");
                    }
                    Usuarios usuarios = db.Usuarios.Find(id);
                    if (usuarios == null)
                    {
                        TempData["errorMessage"] = "No se ha encontrado el Usuario";
                        return RedirectToAction("Index");
                    }
                    ViewBag.id_tipo = new SelectList(db.Tipo_usuario, "id_tipo", "nombre_tipo", usuarios.id_tipo);
                    return View(usuarios);
                }
                else
                {
                    ViewBag.Title = "Perfil";
                    return RedirectToAction("Index", "Perfil", null);
                }
            }
            
        }

        // POST: Usuarios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_usuario,id_tipo,nombres,apellidos,email,telefono,DUI,fecha_nacimiento,contra")] Usuarios usuarios, bool samePassword)
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
                    Usuarios user1 = new Usuarios();
                    user1 = userModel.obtenerUsuario(usuarios.id_usuario);

                    if (samePassword == true)
                    {
                        usuarios.contra = user1.contra;
                    }
                    else
                    {
                        //Encriptamos la password
                        usuarios.contra = SecurityUtils.EncriptarSHA2(usuarios.contra);
                    }

                    if (ModelState.IsValid)
                    {

                        db.Entry(usuarios).State = EntityState.Modified;
                        db.SaveChanges();
                        TempData["successMessage"] = "Usuario modificado de forma exitosa";
                        return RedirectToAction("Index");
                    }

                    foreach (ModelState modelState in ModelState.Values)
                    {
                        foreach (ModelError error in modelState.Errors)
                        {
                            TempData["errorMessage"] = error.ErrorMessage;
                        }
                    }

                    ViewBag.id_tipo = new SelectList(db.Tipo_usuario, "id_tipo", "nombre_tipo", usuarios.id_tipo);
                    return View(usuarios);
                }
                else
                {
                    ViewBag.Title = "Perfil";
                    return RedirectToAction("Index", "Perfil", null);
                }
            }

            
        }

        // GET: Usuarios/Delete/5
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
                        TempData["errorMessage"] = "Debe seleccionar un Usuario";
                        return RedirectToAction("Index");
                    }
                    Usuarios usuarios = db.Usuarios.Find(id);
                    if (usuarios == null)
                    {
                        TempData["errorMessage"] = "No se encontró el registro que selecciono";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        db.Usuarios.Remove(usuarios);
                        if (db.SaveChanges() > 0)
                        {
                            TempData["successMessage"] = "Usuario eliminado de forma exitosa";
                        }
                        else
                        {
                            TempData["errorMessage"] = "No se pudo eliminar el usuario que seleccionó";
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
