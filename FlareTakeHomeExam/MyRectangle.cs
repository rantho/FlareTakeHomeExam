using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlareTakeHomeExam
{
	public class MyRectangle : IBounds
	{
		public MyRectangle()
		{
		}

		public MyRectangle(string name, Point point1, Point point2)
		{
			Point1 = point1;
			Point2 = point2;
			Name = name;
		}
		public string Name { get; set; }
		public Point Point1 { get; set; }
		public Point Point2 { get; set; }
		public bool IsOverlap { get; set; }
		public bool IsExtending { get;set; }
		public int Left { get; set; }
		public int Right { get; set; }	
		public int Top { get; set; }
		public int Bottom { get; set; }
		public int Width { get; set; }
		public int Height { get; set; }

		public Rectangle ToRectangle()
		{
			return new Rectangle(Left, Top,  Width, Height);
		}
	}
}
