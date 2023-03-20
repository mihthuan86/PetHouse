using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetHouse.Models
{
	public class Feedback
	{
		[Key]
		public int Id { get; set; }
		public int OrderId { get; set; }
		public int? ProductId { get; set; }
		[MaxLength(250)]
        public string Decription { get; set; }
		public int? Rating { get; set; }
		public DateTime CreateDate { get; set; }

		//relationship
		[ForeignKey("OrderId")]
		public Order Order { get; set; }
    }
}
