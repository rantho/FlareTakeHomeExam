
UI Use Test Scenario
To check sample scenarios in UI.

TestScenario.TestCaseUI(scenario);
where scenario values: 
	1. No overlapping rectangles.
	2. Invalid: Overlapping rectangles.
	3. Invalid: Rectangle extending beyond grid
	4. Invalid: Overlapping and extending beyond grid
	5. Invalid: 4 Rectangles, 1 valid 3 invalid.

The task

● Create a grid
  - GridRectangle.CreateGrid();
● Place rectangles on the grid
  - GridRectangle.Add(new MyRectangle("A", new Point(1, 1), new Point(4, 5)));
● Find a rectangle based on a given position
  - GridRectangle.Find(new Point(4, 5));
● Remove a rectangle from the grid by specifying any point within the rectangle
  - GridRectangle.Remove(new Point(4, 5));
● [Optional] Display the grid and rectangles as ASCII art - this might help you with
testing!
  - Uses the console window and system.Drawing to test Grid and Rectangles.

Constraints
● A grid must have a width and height of no less than 5 and no greater than 25
  - validated on constructor of GridRectangle, GridRectangle(int cellsize, int numOfRowCells, int numOfColCells).

  if(numOfRowCells < 5 || numOfRowCells > 25 || numOfColCells < 5 || numOfColCells > 25)
			{
				throw new ArgumentOutOfRangeException("A grid must have a width and height of no less than 5 and no greater than 25");
			}

● Positions on the grid are non-negative integer coordinates starting at 0
  - check on adding rectangle. Add(MyRectangle rec).

	if(rec.Point1.X < 0 || rec.Point1.Y < 0 || rec.Point2.X < 0 || rec.Point2.Y < 0)
			{
				throw new ArgumentException("Positions on the grid are non-negative integer coordinates starting at 0");
			}
● Rectangles must not extend beyond the edge of the grid
  - check the list of rectangles GridRectangle.Validate and Mark rectangles  extending within the 
	  the GridRectangle.Left, GridRectangle.Top, GridRectangle.Right, GridRectangle.Bottom.

	var isExtending = r1.Left< Left || r1.Height < Top || r1.Right > Right || r1.Bottom > Bottom;

● Rectangles must not overlap
  - check the list of rectangles GridRectangle.Validate and Mark rectangles that overlaps.

 var isOverLap = !(r1.Left >= r2.Left + r2.Width) &&
								 !(r1.Left + r1.Width <= r2.Left) &&
								 !(r1.Top >= r2.Top + r2.Height) &&
								 !(r1.Top + r1.Height <= r2.Top);
