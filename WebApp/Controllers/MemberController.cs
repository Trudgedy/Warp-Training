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

        [HttpPost]
        public void LinkMember(int id, int PersonId)
        {

            

            var member = new Member();
            member.GroupId = id;
            member.PersonId = PersonId;


            _memberService.Insert(member);

            
        }
    }
}