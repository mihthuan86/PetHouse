using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetHouse.Models
{
	public class Order
	{
		[Key]
		public int Id { get; set; }
        public string UserId { get; set; }		
		[MaxLength(100)]
		public string FullName { get; set; }
		[MaxLength(200)]
		public string Address { get; set; }
		[MaxLength(10)]
		public string PhoneNumber { get; set; }
		[MaxLength(50)]
		public string Email { get; set; }
		public DateTime OrderDate { get; set;}
        public int OrderStatus { get; set; }
		public int PaymentMethod { get; set; }

		//relationship
		[ForeignKey("UserId")]
		public User User { get; set; }		
		public virtual IEnumerable<OrderDetail> OrderDetails { get; set; }
	}
}
