using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class UISlotReel : MonoBehaviour
{
	[SerializeField]
	private Vector2 m_itemSize;
	[SerializeField]
	private Vector2 m_itemSpacing;

	[Space]
	[SerializeField]
	private RollDirectionType m_rollDirectionType;
	public enum RollDirectionType
	{
		Up = 1,
		Down = -1
	}

	[Space]
	[SerializeField]
	private UISlotReelItem m_prefabItem;

	private int _position;
	public int Position => _position;

	private SlotReelData _data;

	private UISlotReelItem[] _items;

	public void SetData(SlotReelData data)
	{
		_data = data;
	}

	public void CreateItems(List<SlotMachine.ItemInfo> itemsInfo)
	{
		_items = new UISlotReelItem[itemsInfo.Count];

		foreach (SlotMachine.ItemInfo itemInfo in itemsInfo)
		{
			UISlotReelItem item = Instantiate(m_prefabItem, transform);

			item.SetData(_data.items[itemInfo.position]);

			item.SetPosition(
				m_itemSpacing.x,
				-itemInfo.position * m_itemSize.y - itemInfo.position * m_itemSpacing.y
			);

			_items[itemInfo.position] = item;
		}
	}
	public void ClearItems()
	{
		if (_items != null)
		{
			for (int i = 0; i < _items.Length; i++)
			{
				if (_items[i] != null)
				{
					Destroy(_items[i].gameObject);
				}

				_items[i] = null;
			}
		}
	}

	public UISlotReelItem GetItem(SlotMachine.ItemInfo itemInfo)
	{
		return _items[itemInfo.position];
	}

	public void SetPosition(int value)
	{
		_position = value;

		RollPosition(GetRollPosition(_position));
	}

	public void DoPosition(int value, float duration, int times)
	{
		float scrollPosition = GetRollPosition(_position);
		float endScrollPosition = GetRollPosition(value, (int)m_rollDirectionType, times);

		_position = value;

		//float doValue = 0.0f;
		DOTween.To(() => scrollPosition, x => scrollPosition = x, endScrollPosition, duration)
			.SetEase(Ease.InOutSine)
			//.SetEase(Ease.OutFlash)
			.OnUpdate(() => {
				RollPosition(scrollPosition);
			});
			/*.OnComplete(() => {
				
			});*/

		// animation...
		//RollPosition(GetRollPosition(_position, (int)m_rollDirectionType, times));
	}

	public void DoFlashItem(SlotMachine.ItemInfo itemInfo)
	{
		_items[itemInfo.position].Flash();
	}

	private void RollPosition(float value)
	{
		int len = _items.Length;
		float height = m_itemSize.y + m_itemSpacing.y;

		int shift = GetPosition(value);
		
		value = (-value % height);
		value = value > 0 ? value - height : value;

		for (int i = 0; i < len; i++)
		{
			int k = (i + shift) % len;

			_items[i].SetPosition(
				m_itemSpacing.x,
				-k * m_itemSize.y - k * m_itemSpacing.y - value
				);
		}
	}

	private float GetRollPosition(int position, int direction = 1, int times = 0)
	{
		int len = _items.Length;
		float height = m_itemSize.y + m_itemSpacing.y;
		position = direction < 0 ? len - position : position;

		return (height * position * direction) + ((height * (len * direction)) * times);
	}
	private int GetPosition(float rollPosition)
	{
		int len = _items.Length;
		int position = Mathf.CeilToInt(-rollPosition / (m_itemSize.y + m_itemSpacing.y)) % len;
		position = position < 0 ? len + position : position;

		return position;
	}
}