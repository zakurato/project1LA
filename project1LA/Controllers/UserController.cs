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

        private UserDAO userRepository = new UserDAO(); //variable que se usa para poder llamar a la funcion del modelo


        // GET: User
        public ActionResult Index()
        {
            return View(userRepository.ReadUsers());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]

        public ActionResult Create(UserDTO user)
        {
            string result = userRepository.InsertUser(user);
            return RedirectToAction("index");

        }

        public ActionResult EditForm(int id)
        {
            var userUpdate = userRepository.UpdateUserForm(new UserDTO { Id = id });

            if (userUpdate != null)
            {
                // Pasar el objeto UserDTO a la vista
                return View(userUpdate);
            }
            else
            {
                return Content("El usuario no se editó correctamente");
            }
        }


        public ActionResult Delete(int id)
        {
            var userDelete = new UserDTO { Id = id };
            string result = userRepository.DeleteUser(userDelete);

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