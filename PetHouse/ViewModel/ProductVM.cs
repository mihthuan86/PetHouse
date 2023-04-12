using PetHouse.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PetHouse.ViewModel
{
    public class ProductVM
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        [StringLength(250)]
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public int BrandId { get; set; }
        [NotMapped]
        public IFormFile ProductImg { get; set; }
        public double OrderPrice { get; set; }
        public double ImnportPrice { get; set; }
    }
}
