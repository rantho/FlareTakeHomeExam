using FlareTakeHomeExam;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using System.Linq;
using System.Threading;

namespace UnitTestProject1
{
	[TestClass]
	public class UnitTest1
	{
		[TestMethod]
		public void CheckValidThreeRectangles()
		{
			var grid = new GridRectangle(20, 10, 14);
			//no overlapping rectangles.
			grid.Add(new MyRectangle("A", new Point(1, 1), new Point(4, 5)));
			grid.Add(new MyRectangle("B", new Point(6, 0), new Point(10, 4)));
			grid.Add(new MyRectangle("C", new Point(2, 5), new Point(8, 7)));

			grid.Validate();

			var actual = grid.Rectangles.Any(w => w.IsExtending || w.IsOverlap);
			Assert.IsFalse(actual);

		}
		[TestMethod]
		public void ValidateOverlappingRectangles()
		{
			var grid = new GridRectangle(20, 10, 14);

			//Invalid: Overlapping rectangles.
			grid.Add(new MyRectangle("A", new Point(1, 1), new Point(4, 5)));
			grid.Add(new MyRectangle("C", new Point(2, 4), new Point(8, 6)));

			grid.Validate();

			var actual = grid.Rectangles.Any(w => w.IsOverlap);
			Assert.IsTrue(actual);
		}
		[TestMethod]
		public void ValidateExtendinggRectangles()
		{
			var grid = new GridRectangle(20, 10, 14);

			//Invalid: Rectangle extending beyond grid
			grid.Add(new MyRectangle("B", new Point(9, 3), new Point(13, 7)));

			grid.Validate();

			var actual = grid.Rectangles.Any(w => w.IsExtending);
			Assert.IsTrue(actual);
		}
		[TestMethod]
		public void RemoveRectangleByPoints()
		{
			var grid = new GridRectangle(20, 10, 14);
	
			grid.Add(new MyRectangle("A", new Point(1, 1), new Point(4, 5)));
			grid.Add(new MyRectangle("B", new Point(6, 0), new Point(10, 4)));
			grid.Add(new MyRectangle("C", new Point(2, 5), new Point(8, 7)));

			grid.Remove(new Point(6, 0));
			grid.Remove(new Point(4, 5));

			var actual = grid.Rectangles.Count;
			Assert.AreEqual(actual,1);
		}
	}
}
