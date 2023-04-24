using System.ComponentModel.DataAnnotations;

namespace PetHouse.Areas.Admin.ViewModel
{
	public class UpdateUserVM
	{
		public string Id { get; set; }	
		[Display(Name = "Họ và Tên")]
		[Required(ErrorMessage = "Vui lòng nhập Họ Tên")]
		public string FullName { get; set; }

		[Display(Name = "UserName")]
		[Required(ErrorMessage = "Vui lòng nhập User Name")]
		public string UserName { get; set; }

		[MaxLength(150)]
		[Required(ErrorMessage = "Vui lòng nhập Email")]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }

		[MaxLength(11)]
		[Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
		[Display(Name = "Điện thoại")]
		[DataType(DataType.PhoneNumber)]
		public string Phone { get; set; }

		[MaxLength(100)]
		[Required(ErrorMessage = "Vui lòng nhập địa chỉ")]
		[Display(Name = "Địa chỉ")]
		public string Address { get; set; }

		public DateTime UpdateDate { get {
				return DateTime.Now;
			} }
	}
}
