using UnityEngine;
using UnityEngine.UI;

public class UISmoothCollectingItemEffect : MonoBehaviour
{
	[SerializeField]
	private UIImageEffect m_image;

	//public int amount;



	public void SetIcon(Sprite icon)
	{
		m_image.GetComponent<Image>().sprite = icon;
	}

	public void Appear()
	{
		m_image.FlashScaleInEffect();
		m_image.AlphaInEffect();
	}
	/*public void Disappear(*//*Action action*//*)
	{
		m_image.FlashScaleUpEffect();
		m_image.AlphaOutEffect(() =>
		{
			*//*action?.Invoke();*//*
		});
	}*/

}