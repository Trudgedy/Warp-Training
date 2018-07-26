using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Library.Data.Models.Common
{
	public class PersonListModel 
	{
		
        [Display(Name = "User Name:")]
		public int PersonId { get; set; }

        [Required]
        public IEnumerable<SelectListItem> Name { get; set; }

	}
}
