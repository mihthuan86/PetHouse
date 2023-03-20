using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetHouse.Models
{
	public class Import
	{
		[Key]
		public int Id { get; set; }
        public int SupplierId { get; set; }
		public string UserId { get; set; }
		public DateTime ImportDate { get; set; }
		public int PaymentStatus { get; set; }

		[ForeignKey(nameof(SupplierId))]
		public Supplier Supplier { get; set; }
		[ForeignKey(nameof(UserId))]
		public User User { get; set; }
		public virtual ICollection<ImportDetail> ImportDetails { get; set; }

	}
}
