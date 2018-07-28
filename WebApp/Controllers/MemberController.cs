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
        IGroupService _groupService;
        IMemberService _memberService;

        public MemberController(IPersonService personService, IGroupService groupService, IMemberService memberService)
        {
            _groupService = groupService;
            _personService = personService;
            _memberService = memberService;
        }

        public ActionResult Add(int id)
        {

            var person = _personService.GetAll();

            ViewBag.PersonId = person;

            var model = _groupService.GetById(id);

            return View(model);
        }

        [HttpPost]
        public ActionResult LinkMember(int id, Person model)
        {
            var url = Url.RequestContext.RouteData.Values["id"];
            var selectedPerson = model.PersonId;

            var member = new Member();
            member.GroupId = id;
            member.PersonId = selectedPerson;


            _memberService.Insert(member);


            return RedirectToAction("Edit", "Groups", new { id = 1 });
            
        }
    }
}