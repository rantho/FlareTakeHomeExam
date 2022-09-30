using System.Drawing;

namespace FlareTakeHomeExam
{
	internal static class TestScenario
	{
		internal static void TestCase(int scenario)
		{
			var grid = new GridRectangle(20,10,14);

			if (scenario == 1)
			{
				//no overlapping rectangles.
				grid.Add(new MyRectangle("A", new Point(1, 1), new Point(4, 5)));
				grid.Add(new MyRectangle("B", new Point(6, 0), new Point(10, 4)));
				grid.Add(new MyRectangle("C", new Point(2, 5), new Point(8, 7)));
			}
			else if (scenario == 2)
			{
				//Invalid: Overlapping rectangles.
				grid.Add(new MyRectangle("A", new Point(1, 1), new Point(4, 5)));
				grid.Add(new MyRectangle("C", new Point(2, 4), new Point(8, 6)));

			}
			else if (scenario == 3)
			{
				//Invalid: Rectangle extending beyond grid
				grid.Add(new MyRectangle("B", new Point(9, 3), new Point(13, 7)));
			}
			else if(scenario == 4)
			{
				//Invalid: Overlapping rectangles.
				grid.Add(new MyRectangle("A", new Point(1, 1), new Point(4, 5)));
				grid.Add(new MyRectangle("B", new Point(2, 4), new Point(8, 6)));
				//Invalid: Rectangle extending beyond grid
				grid.Add(new MyRectangle("C", new Point(9, 3), new Point(13, 7)));
			}
			else if (scenario == 5)
			{
				//Invalid: Overlapping rectangles.
				grid.Add(new MyRectangle("A", new Point(1, 1), new Point(4, 5)));
				grid.Add(new MyRectangle("B", new Point(2, 4), new Point(8, 6)));
				//Invalid: Rectangle extending beyond grid
				grid.Add(new MyRectangle("C", new Point(6, 0), new Point(10, 4)));
				grid.Add(new MyRectangle("D", new Point(10, 3), new Point(13, 7)));

			}
			grid.DrawRectangles();


		}
	}
}
