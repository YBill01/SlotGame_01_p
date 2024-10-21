using UnityEngine;

public class SFXButton : MonoBehaviour
{
	[SerializeField]
	private int m_sfxIndex;

	public void PlaySFX()
	{
		App.Instance.Services
			.Get<UIService>()
			.Get<UISoundService>()
			.PlaySFXOnceShot(m_sfxIndex);
	}
}