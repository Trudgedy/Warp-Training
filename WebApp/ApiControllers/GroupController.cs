using Library.Data.Models.Common;
using Library.Service.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace WebApp.ApiControllers
{
    public class groupController : ApiController
    {

        IGroupService _groupService;
        IMemberService _memberService;

        public groupController(IGroupService groupService, IMemberService memberService)
        {
            _groupService = groupService;
            _memberService = memberService;
        }

        //GET: api/group/
        [System.Web.Http.HttpGet]
        public string get()
        {

            var Groups = _groupService.GetAll();
            string Response = JsonConvert.SerializeObject(Groups);

            return Response;
        }

        //POST: api/group/create
        [System.Web.Http.HttpPost]
        public HttpStatusCodeResult create(Group model)
        {
            try
            {

                if (model != null)
                {
                    _groupService.Insert(model);
                    return new HttpStatusCodeResult(HttpStatusCode.Created);
                }
                else
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
        }
        
        //PUT: api/group/update/1
        [System.Web.Http.HttpPut]
        public HttpStatusCodeResult update(Group model, int id)
        {

            try
            {
                var CurrentGroup = _groupService.GetById(id);

                if (model.Name != null)
                    CurrentGroup.Name = model.Name;

                _groupService.Update(CurrentGroup);


                return new HttpStatusCodeResult(HttpStatusCode.Accepted, model.Name);
            }
            catch (Exception)
            {

                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

        }

        //DELETE: api/group/delete/1
        [System.Web.Http.HttpDelete]
        public HttpStatusCodeResult delete(int id)
        {
            var Group = _groupService.GetById(id);

            if (Group != null)
            {
                _groupService.Delete(Group);
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }

            return new HttpStatusCodeResult(HttpStatusCode.NotFound);

        }

        //GET: api/group/1/members
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/group/{id}/members")]
        public string members(int id)
        {
            var Members = _memberService.GetPeople(id);
            string Response = JsonConvert.SerializeObject(Members);

            return Response;
        }

        //PUT: api/group/1/add/2
        [System.Web.Http.HttpPut]
        [System.Web.Http.Route("api/group/{id}/add/{PersonId}")]
        public HttpStatusCodeResult add(int id, int PersonId)
        {
            try
            {
                var member = new Member();
                member.GroupId = id;
                member.PersonId = PersonId;

                _memberService.Insert(member);
                return new HttpStatusCodeResult(HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden, ex.ToString());
            }

            
        }


    }
}
