using TMPro;
using UnityEngine;

public class UIInfoPatternPanel : MonoBehaviour
{
	[SerializeField]
	private Array2D<GameObject> _tiles;
	[SerializeField]
	private TMP_Text m_multiplierText;

	public void SetData(SlotPatternData.RewardTable table)
	{
		m_multiplierText.text = $"<size=42>x</size>{table.multiplier}";

		SetTiles(table);
	}

	private void SetTiles(SlotPatternData.RewardTable table)
	{
		Vector2Int size = table.grid.Size;
		for (int x = 0; x < size.x; x++)
		{
			for (int y = 0; y < size.y; y++)
			{
				_tiles[x, y].SetActive(table.grid[x, y]);
			}
		}
	}
}