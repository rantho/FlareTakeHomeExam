using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FlareTakeHomeExam
{
	internal static class BoundsHelper
	{
		internal static void CopyBounds(this Rectangle rectangle, IBounds bounds)
		{
			bounds.Top = rectangle.Top;
			bounds.Bottom = rectangle.Bottom;
			bounds.Left = rectangle.Left;
			bounds.Right = rectangle.Right;
			bounds.Width = rectangle.Width;
			bounds.Height = rectangle.Height;
		}
	}
}
