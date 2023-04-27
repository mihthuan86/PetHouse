using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetHouse.Models
{
	public class Cart
	{
		[Key]
        public int Id { get; set; }
		public string UserId { get; set; }
		public int ProductId { get; set; }
		public int Quatity { get; set; }

		[ForeignKey(nameof(UserId))]
		public User User { get; set; }
		[ForeignKey(nameof(ProductId))]
		public Product Product { get; set; }
    }
}
