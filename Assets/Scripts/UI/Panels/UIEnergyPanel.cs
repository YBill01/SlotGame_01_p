using N.Fridman.FormatNums.Scripts.Helpers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIEnergyPanel : MonoBehaviour/*, IPointerDownHandler, IPointerUpHandler*/
{
	[SerializeField]
	private UIFlashEffect m_icon;
	[SerializeField]
	private Image m_indicatorLine;

	[SerializeField]
	private Scrollbar m_scrollbar;
	[SerializeField]
	private UIFlashEffect m_scrollbarLine;

	[SerializeField]
	private TMP_Text m_amountText;

	[SerializeField]
	private UIFloatingStringEffect m_floatingString;

	

	public void SetAmount(int amount, float value)
	{
		m_amountText.text = FormatNumsHelper.FormatNum((float)amount);
		m_scrollbar.size = value;
	}

	public void SetRecoveryProgress(float value)
	{
		m_indicatorLine.fillAmount = value;
	}

	public void Add(int count)
	{
		m_icon.FlashEffect();
		m_scrollbarLine.FlashEffect();

		m_floatingString.FloatingStringEffect($"+{FormatNumsHelper.FormatNum((float)count)}", Color.green);
	}
	public void Take(int count)
	{
		m_floatingString.FloatingStringEffect($"-{FormatNumsHelper.FormatNum((float)count)}", Color.red);
	}





	/*public void OnPointerDown(PointerEventData eventData)
	{
		Add(1232123);
	}
	public void OnPointerUp(PointerEventData eventData)
	{
		Take(123);
	}*/
}