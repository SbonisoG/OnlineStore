using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using OnlineStore.Models;
using OnlineStore.Models.Users;
using Stripe.Checkout;
using static System.Collections.Specialized.BitVector32;


namespace OnlineStore.Controllers
{
	public class CartController : Controller
	{
		private ApplicationDbContext db = new ApplicationDbContext();

		// GET: Cart
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult AddToCart(int id)
		{
			if (Session["cart"] == null)
			{
				List<CartItem> cart = new List<CartItem>();
				//CartItem cartItem = new CartItem { Product = db.Products.Find(id),Qty = 1};
				//CartItem cartItem = new CartItem();
				//cartItem.Product = db.Products.Find(id);
				//cartItem.Qty = 1;
				//cart.Add(cartItem);

				cart.Add(new CartItem() { Product = db.Products.Find(id), Qty = 1 }); //(The above commented lines of code into 1 line of code)
				Session["cart"] = cart;
			}
			else
			{
				List<CartItem> carts = (List<CartItem>)Session["cart"];
				int index = IsInCart(id);
				if (index != -1)
				{
					carts[index].Qty++;
				}
				else
				{
					carts.Add(new CartItem() { Product = db.Products.Find(id), Qty = 1 });

				}
				Session["cart"] = carts;
			}
			return RedirectToAction("Index");
		}

		public ActionResult RemoveFromCart(int id)
		{
			List<CartItem> cart = (List<CartItem>)Session["cart"];
			int index = IsInCart(id);
			cart[index].Qty--;
			if (cart[index].Qty == 0)
			{
				cart.RemoveAt(index);
			}
			//cart.RemoveAt(index);
			if (Session == null)
			{
				Session["cart"] = null;
			}
			else
			{
				Session["cart"] = cart;
			}

			return RedirectToAction("Index");
		}

		public int IsInCart(int id)
		{
			List<CartItem> carts = (List<CartItem>)Session["cart"];
			for (int i = 0; i < carts.Count; i++)
			{
				if (carts[i].Product.ProductId == id)
				{
					return i;
				}
			}

			return -1;
		}

		public double getTotal()
		{
			List<CartItem> cart = (List<CartItem>)Session["cart"];
			var total = (from data in cart select data.Product.Price * data.Qty).Sum();
			return total;
		}


		public ActionResult GetOrder()
		{
			//List<CartItem> carts = (List<CartItem>)Session["cart"];

			//List<OrderDetail> orderDetailList = new List<OrderDetail>();
			//         OrderDetail orderDetail = new OrderDetail();

			//var user = User.Identity.GetUserName();
			//var customer = db.Customers.Where(a => a.Email == user).FirstOrDefault();

			//         if (customer == null)
			//         {
			//             RedirectToAction("Login", "Account");
			//         }
			//         else if(customer != null)
			//         {
			//             order.dateTime = DateTime.Now;
			//             order.Email = customer.Email;
			//             order.FistName = customer.FirstName;
			//             order.LastName = customer.LastName;
			//             order.Address = customer.Address;
			//             order.ContactNo = customer.ContactNo;
			//             order.Total = getTotal();
			//             order.HasBeenShipped = false;
			//             order.PaymentTransactionId = "";


			//	foreach (CartItem cartItem in carts)
			//	{
			//		orderDetail.OrderId = order.OrderId;
			//		orderDetail.ProductId = cartItem.Product.ProductId;
			//		orderDetail.Quantity = cartItem.Qty;
			//                 orderDetail.UnitPrice = cartItem.Product.Price;


			//		orderDetailList.Add(orderDetail);

			//	}

			//	db.OrderDetails.AddRange(orderDetailList);
			//	db.Orders.Add(order);
			//             db.SaveChanges();

			//	Session.Remove("cart");
			//}

			//return RedirectToAction("Index");

			List<CartItem> carts = (List<CartItem>)Session["cart"];

			var user = User.Identity.GetUserName();
			var customer = db.Customers.FirstOrDefault(a => a.Email == user);

			if (customer == null)
			{
				return RedirectToAction("Login", "Account");
			}

			Order order = new Order
			{
				dateTime = DateTime.Now,
				Email = customer.Email,
				FistName = customer.FirstName,
				LastName = customer.LastName,
				ItemQty = carts.Sum(cartItem => cartItem.Qty),
				//Address = customer.Address,
				ContactNo = customer.ContactNo,
				Total = getTotal(),
				HasBeenShipped = "Requested",
				PaymentTransactionId = "PROCESSING"
			};

			List<OrderDetail> orderDetailList = new List<OrderDetail>();

			foreach (CartItem cartItem in carts)
			{
				OrderDetail orderDetail = new OrderDetail
				{
					OrderId = order.OrderId,
					ProductId = cartItem.Product.ProductId,
					Quantity = cartItem.Qty,
					UnitPrice = cartItem.Product.Price
				};

				orderDetailList.Add(orderDetail);
			}

			using (var dbContextTransaction = db.Database.BeginTransaction())
			{
				try
				{
					db.Orders.Add(order);
					db.SaveChanges();

					foreach (var orderDetail in orderDetailList)
					{
						orderDetail.OrderId = order.OrderId;
						db.OrderDetails.Add(orderDetail);
					}

					db.SaveChanges();
					dbContextTransaction.Commit();

					Session.Remove("cart");

					//return RedirectToAction("Update","Order");
				}
				catch (Exception)
				{
					dbContextTransaction.Rollback();
					throw;
				}
			}

			var options = new Stripe.Checkout.SessionCreateOptions
			{
				LineItems = new List<SessionLineItemOptions>
				{
					new SessionLineItemOptions
					{
						PriceData = new SessionLineItemPriceDataOptions
						{
							UnitAmount = Convert.ToInt32(order.Total)*100,
							Currency = "zar",
							ProductData = new SessionLineItemPriceDataProductDataOptions
							{
								Name = order.FistName,
							},

						},
						Quantity= 1,
					},
				},
				Mode = "payment",
				SuccessUrl = "https://localhost:44373/Order/Update/" + order.OrderId,
				CancelUrl = "https://localhost:44373/Home",
			};

			var service = new Stripe.Checkout.SessionService();
			Stripe.Checkout.Session session = service.Create(options);

			Response.Headers.Add("Location", session.Url);
			return new HttpStatusCodeResult(303);
		}
	}
}