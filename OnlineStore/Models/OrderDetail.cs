using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OnlineStore.Models
{
	public class OrderDetail
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int OrderDetailId { get; set; }
		public int OrderId { get; set; }

		//public string Username { get; set; }

		public int ProductId { get; set; }
		public virtual Product Product { get; set; }

		public int Quantity { get; set; }

		public double? UnitPrice { get; set; }
	}
}