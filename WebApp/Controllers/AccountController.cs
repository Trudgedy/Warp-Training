using Library.Core.Crypto;
using Library.Data.Models.Common;
using Library.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace WebApp.Controllers
{
    
    public class AccountController : Controller
    {
        IPersonService _personService;
        public AccountController(IPersonService personService)
        {
            _personService = personService;
        }

        // GET: Account
        public ActionResult Register()
        {

            return View("~/Views/Person/Create.cshtml");
        }

        public ActionResult Login()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Login(Person Model)
        {
            var person = _personService.GetByEmail(Model.Email);
            var InputPassword = Hash.GetHash(Model.Password, person.Salt);

            if (InputPassword == person.Password)
            {
                //TODO update person guid
                //Guid.NewGuid()
                person.PersonGuid = Guid.NewGuid();
                _personService.Update(person);

                Response.Cookies.Add(new HttpCookie("TrainingAuth", person.PersonGuid.ToString()));
                return Redirect("/Person/Index");
            }

            return View();
        }
    }
}