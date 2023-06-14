using project1LA.Models.DAO;
using project1LA.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace project1LA.Controllers
{
    public class UserController : Controller
    {

        private UserDAO userRepository = new UserDAO(); //variable que se usa para poder llamar a las funciones del modelo


        // GET: User
        public ActionResult Index()
        {
            return View(userRepository.ReadUsers());
        }
        //Form Crear usuario
        public ActionResult Create()
        {
            return View();
        }


        //Store guardar usuario
        [HttpPost]

        public ActionResult Create(UserDTO user)
        {
            string result = userRepository.InsertUser(user);
            return RedirectToAction("index");

        }

        //Form editar usuario
        public ActionResult EditForm(int id)
        { 
            string userEdit = id.ToString();

            var userUpdate = userRepository.UpdateUserForm(userEdit);

            if (userUpdate != null)
            {
                return View(userUpdate);
            }
            else
            {
                return Content("El usuario no se editó correctamente");
            }
        }

        //Eliminar usuario
        public ActionResult Delete(int id)
        {

            string idUserDelete = id.ToString();

            string result = userRepository.DeleteUser(idUserDelete);

            if (result == "Success")
            {
                // Elimina el usuario correctamente
                return RedirectToAction("Index"); 
            }
            else
            {
                // Ocurrió un error al eliminar el usuario
                return Content("El usuario no se elimino correctamente");
            }
        }


        //Store editar usuario
        [HttpPost]

        public ActionResult storeUpdate(FormCollection form)
        {
            string name = form["name"];
            string email = form["email"];
            string id = form["id"];

            string result = userRepository.StoreUpdate( name, email, id);

            if (result == "Success")
            {
                // Elimina el usuario correctamente
                return RedirectToAction("Index");
            }
            else
            {
                // Ocurrió un error al eliminar el usuario
                return Content("El usuario no se elimino correctamente");
            }

        }
    }
}