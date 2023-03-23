using PetHouse.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PetHouse.Data.ViewModel
{
	public class ProductVM
	{
		public int Id { get; set; }
		[Required]
		[StringLength(100)]
		public string Name { get; set; }
		[Required]
		[StringLength(250)]
		public string Description { get; set; }
		public int CategoryId { get; set; }
		
		public int BrandId { get; set; }       
		[NotMapped]
		public IFormFile ProductImg { get; set; }
        public IEnumerable<IFormFile> ProductImgOrther { get; set; }

        public double OrderPrice { get; set; }
		public double ImnportPrice { get; set; }
		public int Quantity { get; set; }
		public bool Status { get; set; }
		public DateTime CreateDate { get; set; }
		public DateTime? UpdateDate { get; set; }
	}
}
