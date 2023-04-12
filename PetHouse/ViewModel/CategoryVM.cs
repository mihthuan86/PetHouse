using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PetHouse.ViewModel
{
    public class CategoryVM
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        [StringLength(250)]
        public string Description { get; set; }
        public string? CategoryIcon { get; set; }
        [NotMapped]
        public IFormFile? CategoryImg { get; set; }
        public int? ParentId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        bool isDelete { get; set; }
    }
}
