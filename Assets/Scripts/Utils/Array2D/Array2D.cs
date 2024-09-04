using System;
using System.Linq;
using UnityEngine;

[Serializable]
public struct Array2D<T>
{
	[SerializeField]
	private Vector2Int m_size;

	[SerializeField, HideInInspector]
	private T[] m_data;

	public Vector2Int Size
	{
		get => m_size;
		set
		{
			m_size = value;

			Resize();
		}
	}

	public static implicit operator Array2D<T>(T[,] array) => new Array2D<T>() { m_data = array.Cast<T>().ToArray(), m_size = new Vector2Int(array.GetLength(0), array.GetLength(1)) };

	public Array2D(int x, int y)
	{
		m_size = new Vector2Int(x, y);
		m_data = new T[m_size.x * m_size.y];
	}

	public T this[int x, int y]
	{
		get => m_data[(m_size.x * y) + x];
		set => m_data[(m_size.x * y) + x] = value;
	}

	private void Resize()
	{
		T[] backup = m_data;

		m_data = new T[m_size.x * m_size.y];
		
		for (int i = 0; i < Mathf.Min(m_data.Length, backup.Length); i++)
		{
			m_data[i] = backup[i];
		}
	}
}