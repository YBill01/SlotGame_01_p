using N.Fridman.FormatNums.Scripts.Helpers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIItemPanel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	[SerializeField]
	private UIFlashEffect m_icon;

	[SerializeField]
	private TMP_Text m_amountText;

	[SerializeField]
	private UIFloatingStringEffect m_floatingString;


	public void SetAmount(int amount)
	{
		m_amountText.text = FormatNumsHelper.FormatNum((float)amount);
	}

	public void Add(int count)
	{
		m_icon.FlashScaleEffect();
		m_floatingString.FloatingStringEffect($"+{FormatNumsHelper.FormatNum((float)count)}", Color.green);
	}
	public void Take(int count)
	{
		m_floatingString.FloatingStringEffect($"-{FormatNumsHelper.FormatNum((float)count)}", Color.red);
	}





	public void OnPointerDown(PointerEventData eventData)
	{
		Add(1234);
	}
	public void OnPointerUp(PointerEventData eventData)
	{
		Take(123);
	}
}