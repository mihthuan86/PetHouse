using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetHouse.Models
{
	public class OrderDetail
	{
		
		public int OrderId { set; get; }		
		public int ProductId { set; get; }
		public int Quantity { set; get; }
		public double Price { set; get; }		
		public  Order Order { set; get; }		
		public  Product Product { set; get; }
	}
}

