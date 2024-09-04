using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISlotReelItem : MonoBehaviour
{
	[SerializeField]
	private Image m_image;

	
	public SlotReelItemData ItemData { get; private set; }

	//private SpriteRenderer _renderer;
	

	
	

	private void Start()
	{
		//_renderer = GetComponent<SpriteRenderer>();
		//m_image = GetComponent<Image>();
	}


	public void SetData(SlotReelItemData itemData)
	{
		ItemData = itemData;

		m_image.sprite = ItemData.data.view.icon;
	}

	public void SetPosition(float positionY)
	{
		transform.localPosition = new Vector3(transform.localPosition.x, positionY, transform.localPosition.z);
	}


}