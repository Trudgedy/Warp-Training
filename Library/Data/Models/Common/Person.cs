using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data.Models.Common
{
	public class Person : BaseEntity
	{
		public Person(){
			this.Members = new HashSet<Member>();
		}

		public int PersonId { get; set; }
		[Required]
		public String Name { get; set; }
		[Required]
		public String Email { get; set; }

		#region Navigational Properties

		public ICollection<Member> Members { get; set; }


		#endregion

	}
}
