using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace OnlineStore.Models
{
	public class Product
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ProductId { get; set; }

		[Required]
		public string Name { get; set; }

		public string Description { get; set; }
		public int Quantity { get; set; }
		public int CatId { get; set; }
		public virtual Category Category { get; set; }

		public double Price { get; set; }
		[DisplayName("Product Image")]
		public byte[] PrdctImage { get; set; }
	}
}