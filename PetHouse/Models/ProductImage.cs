using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetHouse.Models
{
	public class ProductImage
	{
		[Key]
		public int Id { get; set; }
		[Required]
		[StringLength(250)]
		public string Url { get; set; }

		public int ProductId { get; set; }
		public int DisplayOrder { get; set; }
		public DateTime CreateDate { get; set; }
		public DateTime? UpdateDate { get; set; }

		//relationship
		[ForeignKey(nameof(ProductId))]
		public Product Product { get; set; }
	}
}
