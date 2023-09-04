using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineStore.Models
{
	public class CartItem
	{
		public Product Product { get; set; }
		public int Qty { get; set; }
	}
}