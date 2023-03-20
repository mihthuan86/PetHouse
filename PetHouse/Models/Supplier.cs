using System.ComponentModel.DataAnnotations;

namespace PetHouse.Models
{
	public class Supplier
	{
		[Key]
        public int Id { get; set; }
		[Required]
		public string Name { get; set; }
		[Required]
		public string Address { get; set; }
		[Required]
		public string Email { get; set; }
		[Required]
		public string PhoneNumber { get; set; }
		public DateTime CreateDate { get; set; }
		public DateTime? UpdateDate { get; set; }
	}
}
