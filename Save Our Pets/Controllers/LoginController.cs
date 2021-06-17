using Save_Our_Pets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Save_Our_Pets.Controllers
{
    public class LoginController : Controller
    {
        UsuariosModel model = new UsuariosModel();

        // GET: Login
        public ActionResult IniciarSesion()
        {
            ViewBag.Title = "Iniciar Sesión";
            return View();
        }

        [HttpPost]
        public ActionResult IniciarSesion(string usuario, string password, string ReturnUrl)
        {

            ViewBag.Title = "Iniciar Sesión";

            if (String.IsNullOrWhiteSpace(usuario))
            {
                ModelState.AddModelError("codigo", "Debe ingresar su usuario");

                TempData["errorMessage"] = "Debe ingresar su usuario";
            }
            else if (String.IsNullOrWhiteSpace(password))
            {
                ViewBag.userAccess = usuario;
                ModelState.AddModelError("password", "Debe ingresar la contraseña");
                TempData["errorMessage"] = "Debe ingresar su contraseña";
            }

            if (!ModelState.IsValid)
            {
                //return View(ModelState);
                return View();
            }
            Usuarios usuario_valido = model.VerificarCuentaporEmail(usuario, password);

            if (usuario_valido == null)
            {
                ViewBag.userAccess = usuario;

                ModelState.AddModelError("codigo", "Usuario y/o password incorrecto");
                TempData["errorMessage"] = "Usuario y/o password incorrecto";

            }

            else
            {
                FormsAuthentication.SetAuthCookie(usuario, false);

                if (usuario_valido != null)
                {
                    //Session["user"] = usuario_valido;
                    System.Web.HttpContext.Current.Session["usuario"] = usuario_valido;
                    System.Web.HttpContext.Current.Session["id_usuario"] = usuario_valido.id_usuario;
                    System.Web.HttpContext.Current.Session["nombre_apellidoUsuario"] = usuario_valido.nombres + " " + usuario_valido.apellidos;
                    System.Web.HttpContext.Current.Session["tipo_usuario"] = usuario_valido.id_tipo;
                }

                if (String.IsNullOrEmpty(ReturnUrl))
                {
                    TempData["successMessage"] = "Bienvenido!!";

                    if (usuario_valido.id_tipo != 3) {
                        return RedirectToAction("Index", "Administracion", null); 
                    }
                    else
                    {
                        return RedirectToAction("Index", "Perfil", null);
                    }
                    

                }
                
                return Redirect(ReturnUrl);

            }

            return View();
        }




        public ActionResult Cerrar()
        {
            FormsAuthentication.SignOut();

            //Session["user"] = null;
            System.Web.HttpContext.Current.Session["usuario"] = null;
            System.Web.HttpContext.Current.Session["nombre_apellidoUsuario"] = null;
            System.Web.HttpContext.Current.Session["tipo_usuario"] = null;

            TempData["errorMessage"] = "Ha cerrado Sesión 😓";

            return RedirectToAction("iniciarSesion");

        }


        public ActionResult Reestablecer()
        {
            ViewBag.Title = "Reestablecer Contraseña";
            return View();
        }

        [HttpPost]
        public ActionResult Reestablecer(string usuario)
        {
            int se_actualizo = 0;
            ViewBag.Title = "Reestablecer Contraseña";
            Usuarios usuario_existe = model.VerificarCorreo(usuario);

            if (usuario_existe != null)
            {
                string crandom = SecurityUtils.generarLlave();//generamos una llave al azar que reemplazara su clave por el momento
                //lo que haremos es que no la encriptaremos para que no pueda acceder hasta que de verdad cambie su clave
                usuario_existe.contra = crandom;

                //actualizamos el usuario
                se_actualizo = model.Update(usuario_existe, usuario_existe.id_usuario);

                if (se_actualizo > 0)
                {
                    //enviamos el correo
                    CorreoContactanos correoreset = new CorreoContactanos(usuario_existe.email, usuario_existe.nombres, usuario_existe.apellidos, crandom);
                    TempData["successMessage"] = "Se ha mandado el enlace a su correo para reestablecer la contraseña 😉";
                    return View();
                }
                else
                {
                    TempData["errorMessage"] = "Ha ocurrido un error, intente de nuevo";
                    return View();
                }

            }
            else
            {
                TempData["errorMessage"] = "El correo indicado no existe en nuestros registros!";
                return View();
            }
        }

        public ActionResult Confirmación(string nk)
        {
            if (!String.IsNullOrEmpty(nk))
            {
                return View();
            }
            else
            {
                TempData["errorMessage"] = "Redireccionando";
                return RedirectToAction("iniciarSesion");
            }


        }

        [HttpPost]
        public ActionResult Confirmación(string nk, string password)
        {
            int actualizar_usuario = 0;
            
            if (String.IsNullOrEmpty(nk))//Por si eliminan el nk de la url
            {
                TempData["errorMessage"] = "No se ha encontrado el nk";
                return View();
            }else if (String.IsNullOrWhiteSpace(password))//verificamos que haya ingresado una contraseña
            {
                ModelState.AddModelError("password", "Debe ingresar una contraseña");
                TempData["errorMessage"] = "Debe ingresar una contraseña";
                return View();
            }
            else
            {//actualizamos el usuario
                Usuarios user = model.Usuario_por_Key(nk);
                //encriptamos
                user.contra = SecurityUtils.EncriptarSHA2(password);

                //actualizamos
                actualizar_usuario = model.Update(user, user.id_usuario);
                //verificamos que se haya actualizado

                if (actualizar_usuario > 0)
                {
                    //enviamos el correo
                    CorreoContactanos correoreset = new CorreoContactanos(user.email);
                    TempData["successMessage"] = "Se ha cambiado la contraseña 😉";
                    return View();
                }
                else
                {
                    TempData["errorMessage"] = "Ha ocurrido un error, intente de nuevo";
                    return View();
                }

            }


        }

    }
}