using System.Collections.Generic;
using System;
using UnityEngine;
using System.Drawing;

[CreateAssetMenu(menuName = "Game/Slot/SlotPatternData", fileName = "SlotPattern", order = 20)]
public class SlotPatternData : ScriptableObject
{
	public int numReels = 0;
	public int numLines = 0;

	[Array2DSimple]
	public Array2D<bool> playGrid;

	//[Array2DSimple]
	//public Array2D<bool>[] rewardTables;
	//[Array2DSimple]
	//public Array2D<bool>[] rewardTables;

	public RewardTable[] rewardTables;

	//public RewardTable rewardTable;

	//public string[] rewardTablesStr;


	[Serializable]
	public struct RewardTable
	{
		public string name;

		[Array2DSimple]
		public Array2D<bool> grid;

		public float multiplier;
	}



	//////////////////////////////

	//public Array2D<bool> grid;

	//public Array2<int> grid2/* = new int[1,2]*/;


	/*[Serializable]
	public class Array<T>
	{
		public List<T> Y = new List<T>();
		public T this[int index] => Y[index];
	}

	[Serializable]
	public class Array2<T>
	{
		public static implicit operator T[](Array2<T> array) => null;
		public static implicit operator T[,](Array2<T> array) => null;
		*//*public static implicit operator Array2<T>(T[] array) => new Array2<T>() {  };
		public static implicit operator Array2<T>(T[,] array) => new Array2<T>() {  };
		public static implicit operator Array2<T>(Array array) => new Array2<T>() {  };*//*


		public List<Array<T>> X = new List<Array<T>>();
		public T this[int x, int y] => X[x][y];

		*//*public T this[int x, int y]
		{
			get => X[x][y];
			//get => data[(size.x * y) + x];
			set => X[x][y] = value;
			//set => data[(size.x * y) + x] = value;
		}*//*
		public Array2(int x, int y)
		{
			
		}
		public Array2(T[,] matrix)
		{
			
		}
	}*/

	//////////////////////////////

	//public Array2d grid;

#if UNITY_EDITOR
	private int _numReelsPrev = -1;
	private int _numLinesPrev = -1;

	private int _rewardTablesSizePrev = -1;

	private void OnValidate()
	{
		if (_numReelsPrev != numReels || _numLinesPrev != numLines || _rewardTablesSizePrev != rewardTables.Length)
		{
			_numReelsPrev = numReels;
			_numLinesPrev = numLines;

			_rewardTablesSizePrev = rewardTables.Length;

			Vector2Int size = new Vector2Int(numReels, numLines);

			playGrid.Size = size;

			for (int i = 0; i < rewardTables.Length; i++)
			{
				rewardTables[i].grid.Size = size;
			}
		}

		for (int i = 0; i < rewardTables.Length; i++)
		{
			for (int x = 0; x < numReels; x++)
			{
				for (int y = 0; y < numLines; y++)
				{
					if (!playGrid[x, y])
					{
						rewardTables[i].grid[x, y] = false;
					}
				}
			}
		}

		//Debug.Log("asd");
		//var c = grid[1, 2];
		//var c2 = grid2[0, 1];

		//grid3[1, 1] = 123;
		//var c3 = grid3[1, 1];

		/*var c4arr = new Array2D<int>();
		c4arr.Size = new Vector2Int(3, 3);
		c4arr[1, 2] = 123;
		var c4 = c4arr[1, 2];




		Debug.Log(c4);*/
	}
#endif


	/*[System.Serializable]
	public class Cell
	{
		public bool value;
	}*/

	/*[System.Serializable]
	public class Array
	{
		public List<bool> cells = new List<bool>();
		public bool this[int index] => cells[index];
	}

	[System.Serializable]
	public class Array2d
	{
		public List<Array> arrays = new List<Array>();
		public bool this[int x, int y] => arrays[x][y];
	}*/
}