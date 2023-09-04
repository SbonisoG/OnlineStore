using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace OnlineStore.Models.Users
{
	public class Driver
	{
		[Key]
		[Display(Name = "Driver ID")]
		public int DriverID { get; set; }

		[Required]
		[StringLength(60, MinimumLength = 3)]
		[Display(Name = "Name")]
		public string FirstName { get; set; }

		[Required]
		[StringLength(60, MinimumLength = 3)]
		[Display(Name = "Surname")]
		public string LastName { get; set; }

		[Required]
		[EmailAddress]
		[Display(Name = "Email")]
		public string Email { get; set; }

		[Display(Name = "Gender")]
		public string Gender { get; set; }

		[Display(Name = "Vehicle")]
		public string Vehicle { get; set; }

		[Display(Name = "Number Plate")]
		public string CarRegNo { get; set; }

		[Display(Name = "Role")]
		public string role { get; set; }

		[Required(ErrorMessage = "Enter Phone Nubmer")]
		[Display(Name = "Phone Number")]
		[DataType(DataType.PhoneNumber)]
		public string ContactNo { get; set; }

		[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "Confirm password")]
		[Compare("Password", ErrorMessage = "Password and confirm password do not match.")]
		public string ConfirmPassword { get; set; }

		public byte[] DriverImage { get; set; }

		[Display(Name = "Trip count")]
		public int tripCount { get; set; }

		public string DriverStatus { get; set; }

		public string Id { get; set; }
		public ApplicationUser ApplicationUser { get; set; }
	}
}