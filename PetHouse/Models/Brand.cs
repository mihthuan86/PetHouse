using System.ComponentModel.DataAnnotations;

namespace PetHouse.Models
{
	public class Brand
	{
		[Key]
		public int Id { get; set; }
		[Required]
		[MaxLength(100)]
		public string Name { get; set; }		
		public int Status { get; set; }
        public DateTime CreateDate { get; set; }
		public DateTime? UpdateDate { get; set; }
    }
}
