using UnityEngine;
using UnityEngine.UI;

public class UIInfoPatternsPanel : MonoBehaviour
{
	[SerializeField]
	private UIInfoPatternPanel m_prefabItem;
	[SerializeField]
	private RectTransform m_infoPatternPanelContainer;
	[SerializeField]
	private ScrollRect m_infoPatternPanelScrollRect;

	private SlotPatternData _data;

	private UIInfoPatternPanel[] _items;

	public void SetData(SlotPatternData patternData)
	{
		_data = patternData;
		_items = new UIInfoPatternPanel[_data.rewardTables.Length];
	}

	public void CreateItems()
	{
		for (int i = 0; i < _data.rewardTables.Length; i++)
		{
			UIInfoPatternPanel item = Instantiate(m_prefabItem, m_infoPatternPanelContainer);

			item.SetData(_data.rewardTables[i]);
			
			_items[i] = item;
		}

		m_infoPatternPanelScrollRect.Rebuild(CanvasUpdate.PostLayout);
	}
	public void ClearItems()
	{
		for (int i = 0; i < _items.Length; i++)
		{
			Destroy(_items[i].gameObject);
			_items[i] = null;
		}
	}
}