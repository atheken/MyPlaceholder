using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using Placeholder.Infrastructure;

namespace Placeholder.Controllers
{
	public class HomeController : Controller
	{
		private ImageGenerator _imgGenerator;
		public HomeController()
		{
			_imgGenerator = new ImageGenerator();
		}

		public ActionResult Index()
		{
			return View();
		}

		[OutputCache(Duration = 60 * 60 * 24, VaryByParam = "*")]
		public ActionResult Render(int width, int? height, String backgroundColor, String foregroundColor, String text)
		{
			var imageData = _imgGenerator.GetImage(width, height ?? width, backgroundColor, foregroundColor, text);
			return File(imageData, "image/png");
		}

	}
}
