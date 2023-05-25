
using System.ComponentModel.DataAnnotations;

namespace PetHouse.Models
{
	public class ImportDetail
	{
		[Key] 
		public int Id { get; set; }
		public int ImportId { get; set; }
		public int ProductId { get; set; }
		public int Quantity { get; set; }
		public double Price { get; set; }
		public double Total { get; set; }
		public Import Import { set; get; }
		public Product Product { set; get; }
	}
}
