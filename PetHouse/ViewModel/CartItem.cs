using PetHouse.Models;

namespace PetHouse.ViewModel
{
    public class CartItem
    {
        public Product Product { get; set; }
        public int amount { get; set; }
        public double TotalMonay => amount * Product.OrderPrice;
    }
}
