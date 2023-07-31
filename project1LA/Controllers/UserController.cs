using MySqlX.XDevAPI;
using project1LA.Models;
using project1LA.Models.DAO;
using project1LA.Models.DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
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
            //Todos las facturas
            var invoices = _db.Invoices.ToList();

            //todos las facturas donde el cliente sea A
            List<Invoice> invoicesCustomerA = invoices.Where(invoice => invoice.Customer == "Cliente A").ToList();

            //Cuenta todos los Clientes 
            var totalInvoicesRecords = invoices.Count();

            //El valor mas alto de las facturas
            var upIvoice = invoices.Max(i=> i.Total);

            //Existen facturas mayores a 500
            var greaterThan500 = invoices.Any(i => i.Total > 500);

            //Suma del contenido de cada total de la factura
            var totalSum = invoices.Sum(i => i.Total);

            //Facturas donde el total sea mayor a 100 y que el cliente diferente a C
            var greaterThan100AndDiffentCCustomer = invoices.Where(i => i.Total > 100 && i.Customer != "Cliente C").ToList();

            //Existen facturas mayores a 100
            var allGreaterThan50 = invoices.All(i => i.Total > 100);






            //Tarea Lambda y LINQ

            //¿Cuál es el total de todas las facturas registradas en la base de datos?
 
            var allInvoicesLambda = _db.Invoices.ToList();
            var allInvoicesLINQ = (from invoice in invoices select invoice).ToList();


            //¿Cuántas facturas tiene el cliente "Cliente B" ?
            var allInvoicesClientBLambda = _db.Invoices.Where(i => i.Customer == "Cliente B").ToList();
            var allInvoicesClientBLINQ = (from invoce in invoices
                                          where invoce.Customer == "Cliente B"
                                          select invoce).ToList();

            //¿Cuál es la factura con el monto más bajo?
            var lowAmountBLambda = _db.Invoices.Min(i => i.Total);
            var lowAmountLINQ = (from invoice in invoices
                                 select invoice.Total).Min();




            //Obtener una lista con los nombres de los clientes que tienen facturas con monto mayor a $200.
            var greaterThan200Lambda = _db.Invoices.Where(i => i.Total > 200).ToList();
            var greaterThan200NameLambda = greaterThan200Lambda.Select(i => i.Customer).ToList();

            var greaterThan200LINQ = (from invoice in invoices
                                      where invoice.Total > 200
                                      select invoice.Customer).ToList();

            //¿Cuál es la fecha más antigua registrada en las facturas?

            var invoiceOldLambda = _db.Invoices.Min(i => i.Date);
            var invoiceOldLINQ = (from invoice in invoices
                                  select invoice.Date
                                  ).Min();

            //Obtener una lista de las 5 facturas más recientes.

            var invoicesTop5RecentlyLambda = _db.Invoices.OrderByDescending(i => i.Date).Take(5).ToList();
            var invoicesTop5RecentlyLINQ = (from invoice in invoices
                                        orderby invoice.Date descending
                                        select invoice).Take(5).ToList();

            //¿Hay alguna factura registrada el día de hoy?

            //fecha actual
            DateTime fechaActual = DateTime.Now.Date;
            var invoicesTodayLambda = _db.Invoices.Where(i => DbFunctions.TruncateTime(i.Date) == fechaActual).ToList();
            var invoicesTodayLINQ = (from invoice in invoices
                                     where invoice.Date.Date == fechaActual
                                     select invoice).ToList();


            //Obtener una lista de las facturas que tienen un monto menor o igual a $100 y un cliente distinto de "Cliente A".
            var lessThan100OrEqualAndDiffentACustomerLambda = _db.Invoices.Where(i => i.Total <= 100 && i.Customer != "Cliente A").ToList();
            var lessThan100OrEqualAndDiffentACustomerLINQ = (from invoice in invoices
                                                             where invoice.Total <= 100 && invoice.Customer != "Cliente A"
                                                             select invoice).ToList();

            //¿Cuál es el promedio del monto de las facturas?
            var invoicesAverageLambda = _db.Invoices.Average(i => i.Total);
            var invoicesAverageLINQ = (from invoice in invoices
                                       select invoice.Total).Average();


            //Obtener una lista con los números de factura de las 10 facturas con montos más altos
            var invoicesTop10MoreAmountLambda = _db.Invoices.OrderByDescending(i => i.Total).Take(10).ToList();
            var invoicesTop10MoreAmountLambda2 = invoicesTop10MoreAmountLambda.Select(i => i.Total).ToList();


            var invoicesTop10MoreAmountLINQ = invoices
                .OrderByDescending(invoice => invoice.Total) 
                .Take(10) 
                .Select(invoice => invoice.Total) 
                .ToList(); 














            //get an specific user (5)
            List<UserDTO> users = userRepository.ReadUsers();
            UserDTO user = (from u in users where u.Id == 5 select u).First();
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

                string result = userRepository.InsertUser(name, email);//guardo el usuario

                List<UserDTO> users = userRepository.ReadUsers();//tabla de usuarios
                var UsersLast = users.LastOrDefault();//busco el ultimo usuario 
                var idUserLast = UsersLast.Id;// obtengo el id del ultimo usuario

                var roleSelected = _db.Roles.Where(x => x.Description == rol).First();//obtengo la coleccion del roll que selecciono

                RolesUsuario ru = new RolesUsuario(); //creo un nuevo objeto RolesUsuario
                ru.IdUser = idUserLast;//guardo el id del ultimo usuario en el campo de la tabla IdUser
                ru.IdRole = roleSelected.Id; //le guardo el id del roll seleccionado en el campo de la tabla IdRole

                _db.RolesUsuario.Add(ru);
                _db.SaveChanges();

                TempData["creado"] = "El usuario se creo correctamente";
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

        public ActionResult getRole(int id)
        {

            int idRole= _db.RolesUsuario.Where(x => x.IdUser == id).FirstOrDefault().IdRole;

            return Content(_db.Roles.Where(x => x.Id == idRole).FirstOrDefault().Description);
        }
    }
}