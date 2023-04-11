namespace PetHouse.Models
{
	public class Payment
	{
        public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public int Status { get; set; }
        public double Surcharge { get; set; }
    }
}
