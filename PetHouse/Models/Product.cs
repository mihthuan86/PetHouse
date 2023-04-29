using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetHouse.Models
{
	public class Product
	{
		[Key]
		public int Id { get; set; }
		[Required]
		[StringLength(100)]
		public string Name { get; set; }
		[Required]
		[StringLength(250)]
		public string Description { get; set; }
        public int CategoryId { get; set; }
		public int BrandId { get; set; }
        public double OrderPrice { get; set; }
        public string ProductImgAvt { get; set; }
		[NotMapped]
		public IFormFile ImgFile { get; set; }
		[Column(TypeName ="xml")]
		public string ImgDetails { get; set; }
		public int Quantity { get; set; }
        public int Status { get; set; }
		public DateTime CreateDate { get; set; }
		public DateTime? UpdateDate { get; set; }

		//relationship
		[ForeignKey("CategoryId")]
		public Category Category { get; set; }
		[ForeignKey("BrandId")]
		public Brand Brand { get; set; }			
		public virtual IEnumerable<OrderDetail> OrderDetails { get; set; }
		public virtual IEnumerable<ImportDetail> ImportDetails { get; set; }		




	}
}
