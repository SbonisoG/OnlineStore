using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using OnlineStore.Models.Users;

namespace OnlineStore.Models
{
	public class Shipping
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ShppngId { get; set; }
		public int OrderId { get; set; }
		public virtual Order Order { get; set; }
		public int? DriverId { get; set; }
		public virtual Driver Driver { get; set; }
		public string ShippingStatus { get; set; }
		public string ShippingCode { get; set; }
		public DateTime? DateCreated { get; set; }
		public DateTime? DateDelivered { get; set; }
		public DateTime? setToDeliveredOn { get; set; }
	}
}