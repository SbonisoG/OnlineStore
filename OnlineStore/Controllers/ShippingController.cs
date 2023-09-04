using Microsoft.AspNet.Identity;
using OnlineStore.Models;
using OnlineStore.Models.Users;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace OnlineStore.Controllers
{
	public class ShippingController : Controller
	{
		private ApplicationDbContext db = new ApplicationDbContext();

		// GET: Shipping
		public ActionResult Index()
		{
			var user = User.Identity.GetUserName();
			var customer = db.Drivers.FirstOrDefault(a => a.Email == user);

			if (User.IsInRole("Driver"))
			{
				List<Shipping> shippings = db.Shippings.Include(s => s.Order).Include(s => s.Driver).Where(s => s.DriverId == customer.DriverID && s.ShippingStatus == "Delivered").ToList();
				return View(shippings);
			}
			else
			{
				List<Shipping> shippings = db.Shippings.Include(s => s.Order).Include(s => s.Driver).ToList();
				return View(shippings);
			}
		}

		public ActionResult IndexI()
		{
			var user = User.Identity.GetUserName();
			var customer = db.Drivers.FirstOrDefault(a => a.Email == user);

			if (User.IsInRole("Driver"))
			{
				List<Shipping> shippings = db.Shippings.Include(s => s.Order).Include(s => s.Driver).Where(s => s.DriverId == customer.DriverID && s.ShippingStatus == "Delivered").ToList();
				return View(shippings);
			}
			else
			{
				List<Shipping> shippings = db.Shippings.Include(s => s.Order).Include(s => s.Driver).Where(s => s.ShippingStatus == "Delivered").ToList();
				return View(shippings);
			}
		}

		public ActionResult ActiveDeliveries()
		{
			var user = User.Identity.GetUserName();
			var customer = db.Drivers.FirstOrDefault(a => a.Email == user);

			if (User.IsInRole("Driver"))
			{
				List<Shipping> shippings = db.Shippings.Include(s => s.Order).Include(s => s.Driver).Where(s => s.DriverId == customer.DriverID && s.ShippingStatus != "Delivered").ToList();
				return View(shippings);
			}
			else
			{
				List<Shipping> shippings = db.Shippings.Include(s => s.Order).Include(s => s.Driver).Where(s => s.ShippingStatus != "Delivered").ToList();
				return View(shippings);
			}
		}

		public ActionResult Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			var shippings = db.Shippings.Include(s => s.Order).Include(s => s.Driver).FirstOrDefault(o => o.ShppngId == id);
			return View(shippings);
		}


		public ActionResult StartShipping(int? id, Shipping shipping)
		{


			var order = db.Orders.Find(id);

			//shipping.DriverId = 0;
			shipping.OrderId = order.OrderId;
			shipping.ShippingStatus = "In Warehouse";
			shipping.DateCreated = DateTime.Now; // Set the current date and time

			order.HasBeenShipped = "Accepted";
			db.Shippings.Add(shipping);

			db.Entry(order).State = EntityState.Modified;
			db.SaveChanges();


			return RedirectToAction("Index");
		}

		public ActionResult Assign(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var shipping = db.Shippings.Find(id);
			if (shipping == null)
			{
				return HttpNotFound();
			}

			var drivers = db.Drivers.ToList();
			ViewBag.DriverId = new SelectList(drivers, "DriverId", "FirstName");
			return View(shipping);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Assign(int? id, Shipping shipping)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var shippng = db.Shippings.Find(id);

			if (shipping != null)
			{
				shippng.DriverId = shipping.DriverId;
				db.Entry(shippng).State = EntityState.Modified;
				db.SaveChanges();
				TempData["Success123"] = "Updated Successfully.";
				return RedirectToAction("Index");
			}


			return View(shipping);
		}

		public static string GenerateCode()
		{
			Random random = new Random();
			const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
			string code = "";

			for (int i = 0; i < 6; i++)
			{
				int index = random.Next(characters.Length);
				code += characters[index];
			}

			return code;
		}

		public ActionResult SetDelivery(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var shipping = db.Shippings.Find(id);
			if (shipping == null)
			{
				return HttpNotFound();
			}


			return View(shipping);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult SetDelivery(int? id, Shipping shippings)
		{
			var shipping = db.Shippings.Find(id);
			var order = db.Orders.Find(shipping.OrderId);

			if (shippings.setToDeliveredOn.Value.DayOfWeek == DayOfWeek.Saturday || shippings.setToDeliveredOn.Value.DayOfWeek == DayOfWeek.Sunday)
			{
				ModelState.AddModelError("", "Weekend delivery is not available.");
				return View(shipping);
			}

			shipping.ShippingStatus = "Ready For Delivery";
			shipping.setToDeliveredOn = shippings.setToDeliveredOn.Value.Date;
			order.HasBeenShipped = "Ready For Delivery";

			db.Entry(shipping).State = EntityState.Modified;
			db.Entry(order).State = EntityState.Modified;

			db.SaveChanges();
			return RedirectToAction("Index");
		}

		public ActionResult OnTheWay(int? id)
		{
			var shipping = db.Shippings.Find(id);
			var order = db.Orders.Find(shipping.OrderId);

			shipping.ShippingStatus = "On The Way";
			shipping.ShippingCode = GenerateCode();
			order.HasBeenShipped = "On The Way";

			db.Entry(shipping).State = EntityState.Modified;
			db.Entry(order).State = EntityState.Modified;
			db.SaveChanges();

			return RedirectToAction("ActiveDeliveries");
		}

		public ActionResult Arrived(int? id)
		{
			var shipping = db.Shippings.Find(id);


			shipping.ShippingStatus = "Arrived";
			db.Entry(shipping).State = EntityState.Modified;
			db.SaveChanges();

			return RedirectToAction("ActiveDeliveries");
		}

		public ActionResult Delivered(int? id)
		{
			var shipping = db.Shippings.Find(id);
			var order = db.Orders.Find(shipping.OrderId);

			shipping.ShippingStatus = "Delivered";
			shipping.DateDelivered = DateTime.Now;

			order.HasBeenShipped = "Delivered";
			db.Entry(shipping).State = EntityState.Modified;
			db.Entry(order).State = EntityState.Modified;
			db.SaveChanges();


			return RedirectToAction("ActiveDeliveries");
		}

		public ActionResult Cancel(int? id)
		{
			var shipping = db.Shippings.Find(id);
			var order = db.Orders.Find(shipping.OrderId);

			shipping.ShippingStatus = "Not Delivered";
			shipping.DateDelivered = DateTime.Now;

			order.HasBeenShipped = "Not Delivered";
			db.Entry(shipping).State = EntityState.Modified;
			db.Entry(order).State = EntityState.Modified;
			db.SaveChanges();


			return RedirectToAction("ActiveDeliveries");
		}
	}
}