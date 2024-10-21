using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class UISlotReelItem : MonoBehaviour, IUIStatsDrop
{
	[SerializeField]
	private UIImageEffect m_image;

	[Space]
	[SerializeField]
	private RectTransform m_dropRectTransform;

	private SlotReelItemData _data;

	//private SpriteRenderer _renderer;

	private RectTransform _rectTransform;


	private void Awake()
	{
		_rectTransform = GetComponent<RectTransform>();
	}
	


	public void SetData(SlotReelItemData data)
	{
		_data = data;

		m_image.GetComponent<Image>().sprite = _data.view.icon;
	}

	public void SetPosition(float positionX, float positionY)
	{
		_rectTransform.anchoredPosition = new Vector2(positionX, positionY);
	}

	public RectTransform GetRectTransformCollecting()
	{
		return m_dropRectTransform;
	}

	public void Flash()
	{
		m_image.FlashScaleUpEffect();
	}

}