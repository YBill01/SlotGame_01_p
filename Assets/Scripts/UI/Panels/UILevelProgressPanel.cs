using N.Fridman.FormatNums.Scripts.Helpers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UILevelProgressPanel : MonoBehaviour, IPointerDownHandler
{

	[SerializeField]
	private Scrollbar m_scrollbar;
	[SerializeField]
	private UIFlashEffect m_scrollbarLine;
	
	[SerializeField]
	private TMP_Text m_levelText;

	[SerializeField]
	private UIFloatingStringEffect m_floatingString;


	public void SetLevel(int value)
	{
		m_levelText.text = $"level {value}";;
	}
	public void SetProgress(float value)
	{
		m_scrollbar.size = value;
	}
	public void Add(int count)
	{
		m_scrollbarLine.FlashEffect();
		m_floatingString.FloatingStringEffect($"+{FormatNumsHelper.FormatNum((float)count)}", new Color(0.92f, 0.64f, 1.0f));
		//SetProgress(value);
	}





	public void OnPointerDown(PointerEventData eventData)
	{
		Add(123);
	}
	
}