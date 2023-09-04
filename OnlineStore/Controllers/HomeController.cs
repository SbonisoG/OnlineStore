using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using OnlineStore.Models;

namespace OnlineStore.Controllers
{
	public class HomeController : Controller
	{
		private ApplicationDbContext db = new ApplicationDbContext();
		public ActionResult Index()
		{


			return View();
		}

		public ActionResult Shop(string searchString)
		{
			var products = (from item in db.Products select item);
			if (!String.IsNullOrEmpty(searchString))
			{
				products = products.Where(s => s.Name.Contains(searchString) || s.Category.Name.Contains(searchString));
			}
			return View(products.ToList());
		}

		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}
	}
}