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
    public class GroupsController : Controller
    {
        IGroupService _groupService;
        IPersonService _personService;

        public GroupsController(IGroupService groupService, IPersonService personService)
        {
            _groupService = groupService;
            _personService = personService;
        }

        // GET: Groups
        public ActionResult Index()
        {

            var group = _groupService.GetAll();

            var model = group.Select(g => new GroupModel
            {
                Name = g.Name
            }).ToList();



            return View(group);
        }

        // GET: Groups/Details/5
        public ActionResult Details(int id)
        {
            var group = _groupService.GetById(id);

            var model = Mapper.Map<GroupModel>(group);

            return View(model);
        }

        public ActionResult Create()
        {
            return View();
        }

        // GET: Groups/Create
        [HttpPost]
        public ActionResult Create(GroupModel model)
        {

            try
            {
                var group = Mapper.Map<Group>(model);

                _groupService.Insert(group);

                return RedirectToAction("Index");

            }
            catch
            {

                return View();
            }

        }


        // GET: Groups/Edit/5
        public ActionResult Edit(int id)
        {
            var group = _groupService.GetById(id);

            var model = Mapper.Map<GroupModel>(group);

            return View(model);
        }

        // POST: Groups/Edit/5.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(GroupModel model)
        {
            try
            {
                var group = _groupService.GetById(model.GroupId);
                group.Name = model.Name;

                _groupService.Update(group);

                return RedirectToAction("Index");
            }
            catch
            {

                return View();
            }
           
        }

        // GET: Groups/Delete/5
        public ActionResult Delete(int id)
        {
            var group = _groupService.GetById(id);

            var model = Mapper.Map<GroupModel>(group);

            return View(model);
        }

        // POST: Groups/Delete/5
        [HttpPost]
        public ActionResult Delete(GroupModel model)
        {
            try
            {
                var group = _groupService.GetById(model.GroupId);
                _groupService.Delete(group);

                return RedirectToAction("Index");
            }
            catch
            {

                return View();
            }
        }

        
            public ActionResult List()
            {
            var people = _personService.GetAll();

            var model = people.Select(p => new PersonModel
            {
                Name = p.Name,
                Email = p.Email
            }).ToList();

            return PartialView("~/Views/Member/_List.cshtml", people);
            }
        
        }
}
