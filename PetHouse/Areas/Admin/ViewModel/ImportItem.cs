using PetHouse.Models;

namespace PetHouse.Areas.Admin.ViewModel
{
	public class ImportItem
	{
		public Product Product { get; set; }
		public int amount { get; set; }
		public int price { get; set; }
		public double TotalMonay => amount * price;
	}
}
