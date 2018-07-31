using Library.Core.Crypto;
using Library.Data.Models.Common;
using Library.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
            var CurrentPerson = _personService.GetByEmail(Model.Email);
            var InputPassword = Hash.GetHash(Model.Password, CurrentPerson.Salt);
            var result = (InputPassword == CurrentPerson.Password);
            return View();
        }
    }
}