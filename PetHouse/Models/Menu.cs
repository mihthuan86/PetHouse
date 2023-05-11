﻿using System.ComponentModel.DataAnnotations;

namespace PetHouse.Models
{
	public class Menu
	{
		[Key]
		public int Id { get; set; }		
		[MaxLength(100)]
		public string Name { get; set; }
		public DateTime CreateDate { get; set; }
		public DateTime? UpdateDate { get; set; }
		public int Status { get; set; }
	}
}
