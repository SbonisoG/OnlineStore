using Microsoft.AspNet.Identity;
using OnlineStore.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace OnlineStore.Controllers
{
	public class OrderController : Controller
	{
		private ApplicationDbContext db = new ApplicationDbContext();
		// GET: Order
		public ActionResult Index()
		{
			List<Order> orders = db.Orders.Include("OrderDetails").ToList();
			return View(orders);
		}

		public ActionResult Update(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var order = db.Orders.Find(id);
			if (order == null)
			{
				return HttpNotFound();
			}
			return View(order);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Update(int? id, Order order)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var orders = db.Orders.Find(id);

			if (order != null)
			{
				orders.PaymentTransactionId = "PAID";
				orders.Address = order.Address;
				db.Entry(orders).State = EntityState.Modified;
				db.SaveChanges();
				TempData["Success123"] = "Updated Successfully.";
				return RedirectToAction("ActiveOrder");
			}


			return View(order);
		}

		public ActionResult MyOrderHistory()
		{
			var user = User.Identity.GetUserName();
			var customer = db.Customers.FirstOrDefault(a => a.Email == user);

			List<Order> orders = db.Orders.Include("OrderDetails").Where(o => o.Email == customer.Email && (o.HasBeenShipped == "Delivered" || o.HasBeenShipped == "Not Delivered")).ToList();
			return View(orders);
		}

		public ActionResult ActiveOrder()
		{
			var user = User.Identity.GetUserName();
			var customer = db.Customers.FirstOrDefault(a => a.Email == user);

			if (User.IsInRole("Assistant"))
			{
				List<Order> orders = db.Orders.Include("OrderDetails").Where(o => o.HasBeenShipped != "Delivered").ToList();
				return View(orders);
			}
			else
			{
				List<Order> orders = db.Orders.Include("OrderDetails").Where(o => o.Email == customer.Email && o.HasBeenShipped != "Delivered").ToList();
				return View(orders);
			}
		}

		public ActionResult ViewOrder(int? id)
		{
			var user = User.Identity.GetUserName();
			var customer = db.Customers.FirstOrDefault(a => a.Email == user);

			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			//var order = db.Orders.Find(id);

			if (User.IsInRole("Assistant"))
			{
				var orders = db.Orders.Include("OrderDetails").FirstOrDefault(o => o.OrderId == id);
				return View(orders);
			}
			else
			{
				var orders = db.Orders.Include("OrderDetails").FirstOrDefault(o => o.Email == customer.Email && o.OrderId == id);
				return View(orders);
			}
		}

	}
}