using Library.Service.Common;
using System.Linq;
using System.Web.Mvc;
using Library.Data.Models.Common;
using AutoMapper;
using Library.Core.Crypto;

namespace WebApp.Controllers
{
    public class PersonController : Controller
	{
		IPersonService _personService;

		public PersonController(IPersonService personService)
		{
			_personService = personService;
		}

		// GET: Person
		public ActionResult Index()
		{
			var people = _personService.GetAll();

			
			var model = people.Select(p=> new PersonModel{
				Name = p.Name,
				Email = p.Email
			}).ToList();

			

			return View(people);
		}

	  
		// GET: Person/Details/5
		public ActionResult Details(int id)
		{
			var person = _personService.GetById(id);

			var model = Mapper.Map<PersonModel>(person);

			return View(model);
		}
		

		// GET: Person/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: Person/Create
		[HttpPost]
		public ActionResult Create(PersonModel model)
		{
			try
			{
				// Add insert logic here

				var person = Mapper.Map<Person>(model);

                person.Salt = Hash.GetSalt();
                person.Password = Hash.GetHash(model.Password, person.Salt);
				_personService.Insert(person);

				return RedirectToAction("Index");
			}
			catch
			{
				return View();
			}
		}

		// GET: Person/Edit/5
		public ActionResult Edit(int id)
		{
			// Find person by Id from database , and send to view after using automapper
			var person = _personService.GetById(id);

			var model = Mapper.Map<PersonModel>(person);

			return View(model);
		}

		// POST: Person/Edit/5
		[HttpPost]
		public ActionResult Edit(PersonModel model)
		{
			try
			{
				// Add update logic here
				var person = _personService.GetById(model.PersonId);
				person.Name = model.Name;
				person.Email = model.Email;
                person.Salt = Hash.GetSalt();
                person.Password = Hash.GetHash(model.Password, person.Salt);

                _personService.Update(person);

				return RedirectToAction("Index");
			}
			catch
			{
				return View();
			}
		}

		// GET: Person/Delete/5
		public ActionResult Delete(int id)
		{
			var person = _personService.GetById(id);
			var model = Mapper.Map<PersonModel>(person);

			return View(model);
		}

		// POST: Person/Delete/5
		[HttpPost]
		public ActionResult Delete(PersonModel model)
		{
			try
			{
				var person = _personService.GetById(model.PersonId);
				_personService.Delete(person);

				return RedirectToAction("Index");
			}
			catch
			{
				return View();
			}
		}

        public ActionResult DownloadPerson()
        {
            // Create a new workbook.
            SpreadsheetGear.IWorkbook workbook = SpreadsheetGear.Factory.GetWorkbook();
            SpreadsheetGear.IWorksheet worksheet = workbook.Worksheets["Sheet1"];
            SpreadsheetGear.IRange cells = worksheet.Cells;

            // Set the worksheet name.
            worksheet.Name = "List of People";

            // Load column titles and center.
            cells["B1"].Formula = "Name";
            cells["C1"].Formula = "Email";
            cells["D1"].Formula = "Password";



           
            var people = _personService.GetAll();
            var count = 1;
            foreach (var item in people)
            {
                cells["B" + count].Formula = item.Name;
                cells["C" + count].Formula = item.Email;
                cells["D" + count].Formula = item.Password;
                count++;
            }
            
            // Save workbook to an Open XML (XLSX) workbook stream.
            System.IO.Stream stream = workbook.SaveToStream(
                SpreadsheetGear.FileFormat.OpenXMLWorkbook);

            // Reset stream's current position back to the beginning.
            stream.Seek(0, System.IO.SeekOrigin.Begin);

            // Stream the Excel spreadsheet to the client in a format
            // compatible with Excel 97/2000/XP/2003/2007/2010/2013/2016.
            return new FileStreamResult(stream,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }
    }
}
