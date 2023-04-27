using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace PetHouse.Models
{
	public class User:IdentityUser
	{
		[MaxLength(50)]
		public string FullName { get; set; }
		[MaxLength(250)]
		public string Address { get; set; }
        public int Status { get; set; }
		public DateTime CreateDate { get; set; }
		public DateTime? UpdateDate { get; set; }
		public virtual IEnumerable<Order> Orders { get; set; }	
	}
}
