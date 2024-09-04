using UnityEngine;

[CreateAssetMenu(menuName = "Game/Slot/SlotReelData", fileName = "SlotReel", order = 31)]
public class SlotReelData : ScriptableObject
{
	public SlotReelItemData[] items;


	
	/*public Sequence[] sequence;

	public struct Sequence
	{
		public SlotReelItemData itemData;

		public float percent;
	}*/

}