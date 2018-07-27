using Library.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace WebApp.ApiControllers
{
    
    public class PersonController : ApiController
    {
        IPersonService _personService;

        public PersonController(IPersonService personService)
        {
            _personService = personService;
        }
        //GET: api/Person
        [HttpGet]
        public String Get()
        {
            var People = _personService.GetAll();
            var json = new JavaScriptSerializer().Serialize(People);
            return json;
        }


    }
}
