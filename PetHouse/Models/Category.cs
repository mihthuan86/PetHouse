using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetHouse.Models
{
	public class Category
	{
		[Key]
		public int Id { get; set; }
		[Required]
		[StringLength(50)]
		public string Name { get; set; }
		[Required]
		[StringLength(250)]
		public string Description { get; set; }
        public string? CategoryIcon { get; set; }
		[NotMapped]
		public IFormFile? CategoryImg { get; set; }
		public string? ParentId { get; set; }		
        public DateTime CreateDate { get; set; }
		public DateTime? UpdateDate { get; set; }
		public bool isDelete { get; set; }
		public bool isParent { get; set; }
		public virtual IEnumerable<Product> Products { get; set; }

    }
}
