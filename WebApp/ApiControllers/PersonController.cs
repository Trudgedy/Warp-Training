using Library.Data.Models.Common;
using Library.Service.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using System.Web.Mvc;

namespace WebApp.ApiControllers
{
    public class personController : ApiController
    {
        IPersonService _personService;

        public personController(IPersonService personService)
        {
            _personService = personService;
        }

        //GET: api/Person
        public string GetPeople()
        {
            var People = _personService.GetAll();
            string Response = JsonConvert.SerializeObject(People);

            return Response;
        }

        //POST: api/person/create
        [System.Web.Http.HttpPost]
        public HttpStatusCodeResult create([FromBody] Person model)
        {
            try {
                if (model != null)
                {

                    _personService.Insert(model);

                    return new HttpStatusCodeResult(HttpStatusCode.Created, "Person has been created");
                }
                else
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }
            }  catch(Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
        }

        //PUT: api/person/update
        [System.Web.Http.HttpPut]
        public HttpStatusCodeResult update([FromBody] Person model, int id)
        {

            try
            {
                var CurrentPerson = _personService.GetById(id);

                if (model.Name != null)
                    CurrentPerson.Name = model.Name;

                if (model.Email != null)
                    CurrentPerson.Email = model.Email;

                _personService.Update(CurrentPerson);


                return new HttpStatusCodeResult(HttpStatusCode.Accepted, model.Email + model.Name);
            }
            catch (Exception)
            {

                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
        }

        //DELETE: api/person/delete/1
        [System.Web.Http.HttpDelete]
        public HttpStatusCodeResult delete(int id)
        {

            var Person = _personService.GetById(id);

            if (Person != null)
            {
                _personService.Delete(Person);
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }

            return new HttpStatusCodeResult(HttpStatusCode.NotFound);
        }





    }
}
