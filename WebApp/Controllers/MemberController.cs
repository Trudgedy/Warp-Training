using Library.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Library.Data;
using Library.Data.Models.Common;
using AutoMapper;

namespace WebApp.Controllers
{
    public class MemberController : Controller
    {

        IPersonService _personService;

        public MemberController(IPersonService personService)
        {
            _personService = personService;
        }

        public ActionResult Add(int id)
        {

            var person = _personService.GetAll();

            ViewBag.PersonId = person;
            

            return View();
        }

        public ActionResult LinkMember(int GroupId)
        {
            String PersonId = Request["PersonID"];

            return RedirectToAction("Edit", "Groups", new { id = GroupId });
            
        }
    }
}