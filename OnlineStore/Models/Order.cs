using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineStore.Models
{
	public class Order
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int OrderId { get; set; }

		public DateTime dateTime { get; set; }

		public string Email { get; set; }
		public string FistName { get; set; }
		public string LastName { get; set; }
		public string Address { get; set; }
		public string ContactNo { get; set; }
		public int ItemQty { get; set; }
		public double Total { get; set; }
		public string PaymentTransactionId { get; set; }
		public string HasBeenShipped { get; set; }

		public virtual ICollection<OrderDetail> OrderDetails { get; set; }

		public Order()
		{
			OrderDetails = new List<OrderDetail>();
		}
	}
}