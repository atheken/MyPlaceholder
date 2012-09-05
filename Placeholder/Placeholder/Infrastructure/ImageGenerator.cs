using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Globalization;
using System.IO;
using System.Drawing.Text;
using System.Drawing.Imaging;
using System.Web.Caching;

namespace Placeholder.Infrastructure
{
	/// <summary>
	/// Provides a mechanism for generating placeholder images.
	/// </summary>
	public class ImageGenerator
	{

		public String CacheKey(int width, int height, Color backgroundColor, Color foregroundColor, String text)
		{
			return String.Format("Placeholder.Cache.{0}x{1}:{2}:{3}:{4}", width, height, backgroundColor, foregroundColor, text);
		}

		public Byte[] GetImage(int width, int height, String backgroundColor, String foregroundColor, String text)
		{
			// Size
			width = Math.Max(Math.Min(3000, width), 1);
			height = Math.Max(Math.Min(3000, height), 1);

			// Color
			var bgColor = Color.FromArgb(204, 204, 204);
			var textColor = Color.FromArgb(150, 150, 150);
			if (Regex.IsMatch(backgroundColor ?? "", "^[0-9a-f]{3,6}$"))
			{
				string rStr = "", gStr = "", bStr = "";
				if (backgroundColor.Length == 3)
				{
					rStr = backgroundColor[0] + "" + backgroundColor[0];
					gStr = backgroundColor[1] + "" + backgroundColor[1];
					bStr = backgroundColor[2] + "" + backgroundColor[2];
				}
				else if (backgroundColor.Length == 6)
				{
					rStr = backgroundColor.Substring(0, 2);
					gStr = backgroundColor.Substring(2, 2);
					bStr = backgroundColor.Substring(4, 2);
				}

				bgColor = Color.FromArgb(Int32.Parse(rStr, NumberStyles.HexNumber), 
					Int32.Parse(gStr, NumberStyles.HexNumber), Int32.Parse(bStr, NumberStyles.HexNumber));
			}
			if (Regex.IsMatch(foregroundColor ?? "", "^[0-9a-f]{3,6}$"))
			{
				string rStr = "", gStr = "", bStr = "";
				if (foregroundColor.Length == 3)
				{
					rStr = foregroundColor[0] + "" + foregroundColor[0];
					gStr = foregroundColor[1] + "" + foregroundColor[1];
					bStr = foregroundColor[2] + "" + foregroundColor[2];
				}
				else if (foregroundColor.Length == 6)
				{
					rStr = foregroundColor.Substring(0, 2);
					gStr = foregroundColor.Substring(2, 2);
					bStr = foregroundColor.Substring(4, 2);
				}

				textColor = Color.FromArgb(Int32.Parse(rStr, NumberStyles.HexNumber), Int32.Parse(gStr, NumberStyles.HexNumber), Int32.Parse(bStr, NumberStyles.HexNumber));
			}

			// Label
			String label = text ?? String.Format("{0}x{1}", width, height, bgColor);

			byte[] retval = null;
			using (MemoryStream memStream = new MemoryStream())
			{
				using (Font font = new Font("Verdana", Math.Min(height / 3, width / label.Length), FontStyle.Bold, GraphicsUnit.Pixel))
				using (Pen pen = new Pen(textColor))
				using (Brush brush = new SolidBrush(textColor))
				using (Image image = new Bitmap(width, height))
				using (Graphics g = Graphics.FromImage(image))
				{
					g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
					g.Clear(bgColor);
					var size = g.MeasureString(label, font);
					//g.DrawRectangle(pen, 0, 0, width-1, height-1);
					g.DrawString(label, font, brush, width / 2 - size.Width / 2, height / 2 - size.Height / 2);
					image.Save(memStream, ImageFormat.Png);
				}
				retval = memStream.ToArray();
			}
			return retval;
		}

	}
}