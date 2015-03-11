using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Logical grid maze created using the growing tree algorithm
/// </summary>
[RequireComponent(typeof(MapGrid))]
public class Maze : AMapGenerationAlgorithm
{

	/// <summary>
	/// The state of a grid cell, used when carving out the maze
	/// </summary>
	enum CellState : byte
	{
		Wall,
		InMaze,
		Exposed,
		Unexposed }
	;

	/// <summary>
	/// Max number of neighbors allowed per cell.
	/// Lower numbers mean narrower passages
	/// Higher numbers fatter passages
	/// </summary>
	[Range(2.0f, 13.0f)]
	public int NeighborsAllowed;

	/// <summary>
	/// Number of map regions along the x axis
	/// </summary>
	private int columnCount;

	/// <summary>
	/// Number of map regions along the z axis
	/// </summary>
	private int rowCount;

	/// <summary>
	/// 2d array containing the state of each cell during carving
	/// </summary>
	private CellState[,] cells;

	/// <summary>
	/// Queue of maze regions to search through 
	/// </summary>
	private MazeRegion[] frontier;

	/// <summary>
	/// Index of last item in the frontier queue
	/// </summary>
	private int frontierMax;
	private MapGrid mapGrid;

	/// <summary>
	/// Initializes a new instance of the <see cref="Maze"/> class.
	/// </summary>
	void Awake ()
	{
		// get reference to map grid
		mapGrid = GetComponent<MapGrid> ();
	}

	void Init ()
	{
		columnCount = mapGrid.columnCount;
		rowCount = mapGrid.rowCount;
		
		// set all cells to unexposed
		cells = new CellState[columnCount, rowCount];
		for (int i=0; i<columnCount; i++) {
			for (int j=0; j<rowCount; j++) {
				cells [i, j] = CellState.Unexposed;
			}
		}
		
		frontier = new MazeRegion[columnCount * rowCount];
		frontierMax = 0;
	}



	/// <summary>
	/// Generates a logical grid maze using the growing tree algorithm
	/// </summary>
	/// <returns>The maze as 2d bool array. True if cell is in the maze, false if not.</returns>
	private bool[,] buildMaze ()
	{

		MazeRegion current;
		int currentIndex;
		bool[,] mazeCells = new bool[columnCount, rowCount];

		// initialize class variables
		Init ();

		// add initial cell to the maze
		carve (0, 0);

		// build maze
		while (frontierMax > 0) {
			currentIndex = Mathf.FloorToInt (Random.Range (0, frontierMax));

			current = frontier [currentIndex];
			frontierMax--;
			frontier [currentIndex] = frontier [frontierMax];


			if (check (current.X, current.Y)) {
				carve (current.X, current.Y);
			} else {
				harden (current.X, current.Y);
			}



		}

		// format return array
		for (int i=0; i<columnCount; i++) {
			for (int j=0; j<rowCount; j++) {
				mazeCells [i, j] = (cells [i, j] == CellState.InMaze);
			}
		}

		return mazeCells;
	}

	/// <summary>
	/// Counts the number of adjecent cell neighbors that are in the maze
	/// </summary>
	/// <param name="x">The x coordinate.</param>
	/// <param name="y">The y coordinate.</param>
	/// <returns>Returns true if this cell should be added to the maze</returns>
	private bool check (int x, int y)
	{
		short adjacentCellsInMaze = 0;

		if ((x > 0) && (cells [x - 1, y] == CellState.InMaze)) {
			++adjacentCellsInMaze;

			if (x < columnCount - 1) {
				if ((y > 0) && (cells [x + 1, y - 1] == CellState.InMaze)) {
					++adjacentCellsInMaze;
				}

				if ((y < rowCount - 1) && (cells [x + 1, y + 1] == CellState.InMaze)) {
					++adjacentCellsInMaze;
				}
			}
		}

		if ((x < columnCount - 1) && (cells [x + 1, y] == CellState.InMaze)) {
			++adjacentCellsInMaze;

			if (x > 0) {
				if ((y > 0) && (cells [x - 1, y - 1] == CellState.InMaze)) {
					++adjacentCellsInMaze;
				}
				
				if ((y < rowCount - 1) && (cells [x - 1, y + 1] == CellState.InMaze)) {
					++adjacentCellsInMaze;
				}
			}
		}

		if ((y > 0) && (cells [x, y - 1] == CellState.InMaze)) {
			++adjacentCellsInMaze;

			if (y < rowCount - 1) {
				if ((x > 0) && (cells [x - 1, y + 1] == CellState.InMaze)) {
					++adjacentCellsInMaze;
				}
				
				if ((x < columnCount - 1) && (cells [x + 1, y + 1] == CellState.InMaze)) {
					++adjacentCellsInMaze;
				}
			}
		}

		if ((y < rowCount - 1) && (cells [x, y + 1] == CellState.InMaze)) {
			++adjacentCellsInMaze;

			if (y > 0) {
				if ((x > 0) && (cells [x - 1, y - 1] == CellState.InMaze)) {
					++adjacentCellsInMaze;
				}
				
				if ((x < columnCount - 1) && (cells [x + 1, y - 1] == CellState.InMaze)) {
					++adjacentCellsInMaze;
				}
			}
		}

		return (adjacentCellsInMaze < NeighborsAllowed);
	}

	/// <summary>
	/// Marks a specified cell to the maze and adds the cell's neighbors
	/// to the cells to check
	/// </summary>
	/// <param name="x">The x coordinate.</param>
	/// <param name="y">The y coordinate.</param>
	private void carve (int x, int y)
	{
	
		// add cell to maze
		cells [x, y] = CellState.InMaze;

		// explore unexposed neighbors
		if ((x > 0) && (cells [x - 1, y] == CellState.Unexposed)) {
			expose (x - 1, y);
		}
		if ((x < columnCount - 1) && (cells [x + 1, y] == CellState.Unexposed)) {
			expose (x + 1, y);
		}
		if ((y > 0) && (cells [x, y - 1] == CellState.Unexposed)) {
			expose (x, y - 1);
		}
		if ((y < rowCount - 1) && (cells [x, y + 1] == CellState.Unexposed)) {
			expose (x, y + 1);
		}

	}

	/// <summary>
	/// Marks a cell as not in the maze
	/// </summary>
	/// <param name="x">The x coordinate.</param>
	/// <param name="y">The y coordinate.</param>
	private void harden (int x, int y)
	{
		cells [x, y] = CellState.Wall;
	}

	/// <summary>
	/// Marks a cell as exposed and adds to the queue to check
	/// </summary>
	/// <param name="x">The x coordinate.</param>
	/// <param name="y">The y coordinate.</param>
	private void expose (int x, int y)
	{
		cells [x, y] = CellState.Exposed;
		frontier [frontierMax] = new MazeRegion (x, y);
		frontierMax++;
	}

	override public Mesh[] Draw ()
	{

		bool[,] mazeRegions = buildMaze ();
		Mesh[] meshes = mapGrid.Draw (mazeRegions);
		return meshes;
	}

	override public List<Vector3> GetObjectPositions ()
	{
		return mapGrid.GetObjectPositions ();
	}

	override public void SetDifficulty (DifficultySetting difficulty)
	{
		columnCount = difficulty.columns;
		rowCount = difficulty.rows;

		mapGrid.SetDifficulty (difficulty);


	}

}
