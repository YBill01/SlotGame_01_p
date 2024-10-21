using System.Collections.Generic;

public class SlotReel
{
	private SlotReelData _data;

	private SlotReelItem[] _items;
	public SlotReelItem[] Items => _items;

	private int _position;
	public int Position => _position;

	public SlotReel(SlotReelData data, List<SlotMachine.ItemInfo> itemsInfo)
	{
		_data = data;

		_items = new SlotReelItem[itemsInfo.Count];
		for (int i = 0; i < itemsInfo.Count; i++)
		{
			_items[itemsInfo[i].position] = new SlotReelItem(_data.items[itemsInfo[i].position]);
			_items[itemsInfo[i].position].index = itemsInfo[i].index;
		}
	}

	public void SetPosition(int value)
	{
		_position = value;
	}
}