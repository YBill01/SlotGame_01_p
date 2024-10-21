using UnityEngine;

public class UISlotTiles : MonoBehaviour
{
	[SerializeField]
	private Array2D<GameObject> _tilesClose;
	[SerializeField]
	private Array2D<GameObject> _tilesShadow;

	public void SetTiles(SlotPatternData slotPattern)
	{
		for (int x = 0; x < slotPattern.numLines; x++)
		{
			for (int y = 0; y < slotPattern.numReels; y++)
			{
				_tilesClose[x, y].SetActive(!slotPattern.playGrid[x, y]);
				_tilesShadow[x, y].SetActive(!slotPattern.playGrid[x, y]);
			}
		}
	}
}