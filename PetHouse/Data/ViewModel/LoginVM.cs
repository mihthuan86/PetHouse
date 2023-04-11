

using System.ComponentModel.DataAnnotations;

namespace PetHouse.Data.ViewModel
{
	public class LoginVM
	{
		[Required(ErrorMessage ="Vui lòng nhập Email hoặc UserName")]
		public string UserNameOrEmail { get; set; }

		[Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
		[DataType(DataType.Password)]
		public string Password { get; set; }
	}
}
