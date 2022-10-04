using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace FlareTakeHomeExam
{
	public class GridRectangle: IBounds
	{
		[DllImport("kernel32.dll", EntryPoint = "GetConsoleWindow", SetLastError = true)] private static extern IntPtr GetConsoleHandle();
		[DllImport("user32.dll")] private static extern int GetWindowRect(IntPtr hwnd, out Rectangle rect);

		public Collection<MyRectangle> Rectangles = new Collection<MyRectangle>();

		#region : Properties
		public int Left { get; set; }
		public int Right { get; set; }
		public int Top { get; set; }
		public int Bottom { get; set; }
		public int Width { get; set; }
		public int Height { get; set; }
		private int CellSize { get; set; }
		private int NumberOfColumnCells { get; set; }
		private int NumberOfRowCells { get; set; }
		#endregion

		/// <param name="cellsize">set size of grid cell</param>
		/// <param name="numOfRowCells">set number rows</param>
		/// <param name="numOfColCells">st number columns</param>
		public GridRectangle(int cellsize, int numOfRowCells, int numOfColCells)
		{
			if(cellsize <= 0)
			{
				throw new ArgumentOutOfRangeException("Cell size should be greater than 0");
			}
			if(numOfRowCells < 5 || numOfRowCells > 25 || numOfColCells < 5 || numOfColCells > 25)
			{
				throw new ArgumentOutOfRangeException("A grid must have a width and height of no less than 5 and no greater than 25");
			}

			CellSize = cellsize;
			NumberOfColumnCells = numOfColCells;
			NumberOfRowCells = numOfRowCells;

			var borderRec = new Rectangle(CellSize, CellSize, (numOfColCells * CellSize) - (CellSize * 2), (numOfRowCells * CellSize) - (CellSize * 2));
			borderRec.CopyBounds(this);
		}

		/// <summary>
		/// Place rectangles on the grid
		/// </summary>
		/// <param name="rec">MyRectangle</param>
		public void Add(MyRectangle rec)
		{
			if(rec.Point1.X < 0 || rec.Point1.Y < 0 || rec.Point2.X < 0 || rec.Point2.Y < 0)
			{
				throw new ArgumentException("Positions on the grid are non-negative integer coordinates starting at 0");
			}

			Rectangles.Add(rec);
			var x = rec.Point1.X * (CellSize);
			var y = rec.Point1.Y * (CellSize);
			var width = Math.Abs(rec.Point2.X - rec.Point1.X) * CellSize;
			var height = Math.Abs(rec.Point2.Y - rec.Point1.Y) * CellSize;

			x += CellSize;
			y += CellSize;

			Rectangle strucRect = new Rectangle(x, y, width, height);
			strucRect.CopyBounds(rec);
		}

		/// <summary>
		/// Remove a rectangle from the grid by specifying any point within the rectangle
		/// </summary>
		/// <param name="point"></param>
		public void Remove(Point point)
		{
			var tempRec = new MyRectangle(String.Empty, point, new Point(point.X + 1, point.Y + 1));

			var x = tempRec.Point1.X * (CellSize);
			var y = tempRec.Point1.Y * (CellSize);
			var width = Math.Abs(tempRec.Point2.X - tempRec.Point1.X) * CellSize;
			var height = Math.Abs(tempRec.Point2.Y - tempRec.Point1.Y) * CellSize;

			x += CellSize;
			y += CellSize;

			Rectangle strucRect = new Rectangle(x, y, width, height);
			strucRect.CopyBounds(tempRec);


			var myRectangles = Rectangles.ToList();

			myRectangles.ForEach(r2 =>
			{
				var isOverLap = !(tempRec.Left >= r2.Left + r2.Width) &&
						 !(tempRec.Left + tempRec.Width <= r2.Left) &&
						 !(tempRec.Top >= r2.Top + r2.Height) &&
						 !(tempRec.Top + tempRec.Height <= r2.Top);

				if (isOverLap)
				{
					Rectangles.Remove(r2);
				}
			});
		}

		/// <summary>
		/// Find a rectangle based on a given position
		/// </summary>
		/// <param name="point"></param>
		/// <returns></returns>
		public MyRectangle Find(Point point)
		{
			var rectangle = Rectangles.FirstOrDefault(s => (s.Point1.X == point.X && s.Point1.Y == point.Y) ||
			(s.Point2.X == point.X && s.Point2.Y == point.Y));

			return rectangle;
		}
		public void ClearScreen()
		{
			Console.WindowHeight = CellSize;
			Console.WindowWidth = CellSize * 2;
			Console.CursorVisible = false;
			Console.Clear();
		}

		/// <summary>
		/// Renders all the rectangles to console screen.
		/// </summary>
		public void DrawRectangles()
		{
			ClearScreen();

			IntPtr handle = GetConsoleHandle();

			Rectangle rect;
			GetWindowRect(handle, out rect);
			
			Random rand = new Random(0);
			Pen pen = new Pen(Color.White);

			using (var gfx = Graphics.FromHwnd(handle))
			{
				gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
				gfx.Clear(Color.White);
				CreateGrid(gfx);

				Brush brush = new SolidBrush(Color.White);
				foreach (var rec in Rectangles)
				{
					pen.Color = Color.FromArgb(rand.Next());
					brush = new SolidBrush(pen.Color);

					using (Font font2 = new Font("Arial", 16, FontStyle.Regular, GraphicsUnit.Point))
					{
						var rect2 = rec.ToRectangle();
						TextFormatFlags flags = TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter;
						TextRenderer.DrawText(gfx, rec.Name, font2, rect2, pen.Color, flags);
						pen.Color = Color.Black;
						gfx.DrawRectangle(pen, Rectangle.Round(rect2));
						gfx.FillRectangle(brush, rect2);	
					}
				}
				Validate();
				PrintInvalidRectanglePositions(gfx);
			}
		}

		/// <summary>
		/// Prints the invalid rectangles below the grid.
		/// </summary>
		/// <param name="gfx"></param>
		private void PrintInvalidRectanglePositions(Graphics gfx)
		{
			var overLapRectangles = Rectangles.Where(w => w.IsOverlap).Select(s => s.Name).ToList();
			var extendingRectangles = Rectangles.Where(w => w.IsExtending).Select(s => s.Name).ToList();

			if (overLapRectangles.Any())
			{
				using (Font font2 = new Font("Arial", 12, FontStyle.Regular, GraphicsUnit.Point))
				{
					var errorMessage = $"Overlapping: {string.Join(",", overLapRectangles)}";
					Rectangle rect2 = new Rectangle(CellSize, Bottom + CellSize * 2, Width, CellSize);
					TextFormatFlags flags = TextFormatFlags.VerticalCenter | TextFormatFlags.WordBreak;
					TextRenderer.DrawText(gfx, errorMessage, font2, rect2, Color.Black, flags);
				}
			}
			if (extendingRectangles.Any())
			{
				using (Font font2 = new Font("Arial", 12, FontStyle.Regular, GraphicsUnit.Point))
				{
					var errorMessage = $"Extending: {string.Join(",", extendingRectangles)}";

					var y = Bottom + CellSize * 2;
					if (overLapRectangles.Any()) y += CellSize;

					Rectangle rect2 = new Rectangle(CellSize, y, Width, CellSize);
					TextFormatFlags flags = TextFormatFlags.VerticalCenter | TextFormatFlags.WordBreak;
					TextRenderer.DrawText(gfx, errorMessage, font2, rect2, Color.Black, flags);
				}
			}			
		}

		/// <summary>
		/// Create graphical view of Grid
		/// </summary>
		/// <param name="gfx"></param>
		private void CreateGrid(Graphics gfx)
		{
			Pen pen = new Pen(Color.White);
			pen.Color = Color.LightGray;

			//Row
			for (int y = 0; y < NumberOfRowCells; ++y)
			{
				gfx.DrawLine(pen, 0, y * CellSize, NumberOfColumnCells * CellSize, y * CellSize);
			}
			for (int y = 0; y < NumberOfRowCells - 2; ++y)
			{
				using (Font font2 = new Font("Arial", 10, FontStyle.Regular, GraphicsUnit.Point))
				{
					Rectangle rect2 = new Rectangle(0, (y + 1) * CellSize, CellSize, CellSize);
					TextFormatFlags flags = TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter;
					TextRenderer.DrawText(gfx, y.ToString(), font2, rect2, Color.Black, flags);
				}
			}

			//Column
			for (int x = 0; x < NumberOfColumnCells; ++x)
			{
				gfx.DrawLine(pen, x * CellSize, 0, x * CellSize, NumberOfRowCells * CellSize);
			}
			gfx.DrawRectangle(pen, 0, 0, NumberOfColumnCells * CellSize, NumberOfRowCells * CellSize);

			for (int x = 0; x < NumberOfColumnCells - 2; ++x)
			{
				using (Font font2 = new Font("Arial", 10, FontStyle.Regular, GraphicsUnit.Point))
				{
					Rectangle rect2 = new Rectangle((x + 1) * CellSize, 0, CellSize, CellSize);
					TextFormatFlags flags = TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter;
					TextRenderer.DrawText(gfx, x.ToString(), font2, rect2, Color.Black, flags);
				}
			}

			pen.Color = Color.Black;
			var borderRec =  new Rectangle(Left, Top, Width, Height);
			gfx.DrawRectangle(pen,borderRec);		
		}

		/// <summary>
		/// Marks the rectangles that overlaps and extending beyond the grid.
		/// </summary>
		public void Validate()
		{
			foreach (var r1 in Rectangles)
			{
				var checkThsRectangle = Rectangles.Where(w => w.Name != r1.Name && (!w.IsOverlap || !w.IsExtending)).ToList();
				if(checkThsRectangle.Any())
				{
					checkThsRectangle.ForEach(r2 =>
					{
						var isOverLap = !(r1.Left >= r2.Left + r2.Width) &&
								 !(r1.Left + r1.Width <= r2.Left) &&
								 !(r1.Top >= r2.Top + r2.Height) &&
								 !(r1.Top + r1.Height <= r2.Top);

						if(!r1.IsOverlap) r1.IsOverlap = isOverLap;
						if(!r2.IsOverlap) r2.IsOverlap = isOverLap;
					});
				}
				var isExtending = r1.Left< Left || r1.Height < Top || r1.Right > Right || r1.Bottom > Bottom;
				r1.IsExtending = isExtending;
			}
		}
	}
}
