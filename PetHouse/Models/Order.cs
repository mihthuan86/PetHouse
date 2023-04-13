using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetHouse.Models
{
	public class Order
	{
		[Key]
		public int Id { get; set; }
        public string CustomerId { get; set; }		
		[MaxLength(100)]
		public string Receiver_FullName { get; set; }
		[MaxLength(200)]
		public string Receiver_Address { get; set; }
		[MaxLength(10)]
		public string Receiver_PhoneNumber { get; set; }
		[MaxLength(70)]
		public string? Receiver_Email { get; set; }
		[MaxLength(100)]
		public string? Note { get; set; }
		public DateTime OrderDate { get; set;}
        public int OrderStatus { get; set; }
		public int PaymentId { get; set; }
        public DateTime ShipDate { get; set; }
        public double TotalPrice { get; set; }
        //relationship
        [ForeignKey("CustomerId")]
		public User User { get; set; }
		[ForeignKey("PaymentId")]
		public Payment Payment { get; set; }
		public virtual IEnumerable<OrderDetail> OrderDetails { get; set; }
	}
}
