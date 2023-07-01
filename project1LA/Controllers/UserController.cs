using project1LA.Models;
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



        private AuthorizationConfig _db = new AuthorizationConfig();



        // GET: User
        public ActionResult Index()
        {

            //get an specific user (5)
            List<UserDTO> users = userRepository.ReadUsers();
            UserDTO user = (from u in users where u.Id == 25 select u).First();
            RolesUsuario ru = _db.RolesUsuario.Where(x=> x.IdUser == user.Id).FirstOrDefault();
            //get role // name
            Session["role"] = _db.Roles.Where(x=> x.Id == ru.IdRole).FirstOrDefault().Description;
            //asing a session variable
            var roles = _db.Roles.ToList();
            return View(userRepository.ReadUsers());
        }
        //Form Crear usuario
        public ActionResult Create()
        {
            var roles = _db.Roles.ToList();//envio los roles por la vista   
            return View(roles);
        }


        //Store guardar usuario
        [HttpPost]

        public ActionResult Create(FormCollection form)
        {

            string name = form["name"];
            string email = form["email"];
            string rol = form["rol"];

            if(rol == "")
            {
                TempData["rolRequerido"] = "Debe seleccionar un roll";
                return RedirectToAction("Create");

            }
            else
            {

                List<UserDTO> users = userRepository.ReadUsers();//tabla de usuarios
                var UsersLast = users.LastOrDefault();//busco el ultimo usuario 
                var idUserIdLast = UsersLast.Id;// obtengo el id del ultimo usuario

                string result = userRepository.InsertUser(name, email);

                var idUserRoleCreate = _db.Roles.Where(x => x.Description == rol).First();//busco el id del roll que selecciono

                TempData["creado"] = "El usuario se creo correctamente";
                RolesUsuario ru = new RolesUsuario(); //creo un nuevo objeto RolesUsuario
                ru.IdUser = idUserIdLast + 1;//le sumo 1 ya que va ser el nuevo id usuario
                ru.IdRole = idUserRoleCreate.Id; //le guardo el id del roll

                _db.RolesUsuario.Add(ru);
                _db.SaveChanges();

                return RedirectToAction("index");
            }

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
                TempData["Eliminado"] = "El usuario se elimino correctamente";

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
                TempData["actualizado"] = "El usuario se edito correctamente";

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