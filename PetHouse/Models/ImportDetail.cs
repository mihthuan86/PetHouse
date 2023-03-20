using System.ComponentModel.DataAnnotations.Schema;

namespace PetHouse.Models
{
	public class ImportDetail
	{
		public int ImportId { get; set; }
		public int ProductId { get; set; }
		public int Quantity { get; set; }
		public decimal Price { get;}
		public Import Import { set; get; }
		public Product Product { set; get; }
	}
}
