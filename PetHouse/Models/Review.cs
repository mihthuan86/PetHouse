using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetHouse.Models
{
	public class Review
	{
		[Key]
		public int Id { get; set; }
		public int OrderId { get; set; }
		public int ProductId { get; set; }
		[MaxLength(250)]
        public string? Decription { get; set; }
		public int Rating { get; set; }
		public DateTime CreateDate { get; set; }
        public string? UserFeedbackId { get; set; }
		public string? Feedback { get; set; }
        public DateTime? FeedbackDate { get; set; }
        //relationship
        [ForeignKey("OrderId")]
		public Order Order { get; set; }		
		[ForeignKey("UserFeedbackId")]
		public User User { get; set; }
	}
}
