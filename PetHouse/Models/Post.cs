using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetHouse.Models
{
	public class Post
	{
		[Key]
		public int Id { get; set; }
		[MaxLength(100)]
		public string Name { get; set; }
		public int MenuId { get; set; }
		[MaxLength(200)]
		public string Description { get; set; }		
		public string AuthorId { get; set; }
		[MaxLength(200)]
		public string ImgUrl { get; set;}
		public string Content { get; set; }
		public DateTime CreateDate { get; set; }
		public DateTime? UpdateDate { get; set; }

		//relationship
		[ForeignKey(nameof(MenuId))]
		public Menu Menu { get; set; }
		[ForeignKey(nameof(AuthorId))]
		public User User { get; set; }

	}
}
