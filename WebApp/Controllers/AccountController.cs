using Library.Core.Crypto;
using Library.Data.Models.Common;
using Library.Service.Common;
using System;
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
        
        public RedirectToRouteResult LogOff(int id)
        {

            var person = _personService.GetById(id);
            person.PersonGuid = new Guid();

            _personService.Update(person);

            return RedirectToAction("Index", "Person", null);
        }

        public ActionResult LoginPartial()
        {
            HttpCookie cookie = Request.Cookies["TrainingAuth"];

            if (cookie != null)
            {
                var SessionGuid = new Guid(cookie.Value);

                    ViewBag.User = _personService.GetByGuid(SessionGuid);

                if (ViewBag.User != null)
                {
                    ViewBag.IsAuthenticated = true;
                }else
                {
                    ViewBag.IsAuthenticated = false;
                }

            }else
            {
                ViewBag.IsAuthenticated = false;
            }
            return PartialView("~/Views/Shared/_LoginPartial.cshtml");
        }

        public ActionResult Login()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Login(Person Model)
        {
            var person = _personService.GetByEmail(Model.Email);
            if (person != null)
            {


                var InputPassword = Hash.GetHash(Model.Password, person.Salt);

                if (InputPassword == person.Password)
                {
                    person.PersonGuid = Guid.NewGuid();
                    _personService.Update(person);

                    Response.Cookies.Add(new HttpCookie("TrainingAuth", person.PersonGuid.ToString()));
                    return Redirect(Request.QueryString["redirectUrl"]);
                }
            }else
            {
                return Redirect("/Person/Create/");
            }
            return View();
        }
    }
}