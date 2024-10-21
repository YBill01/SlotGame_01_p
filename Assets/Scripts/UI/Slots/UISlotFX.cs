using UnityEngine;

public class UISlotFX : MonoBehaviour
{
	[SerializeField]
	private ParticleSystem _sparks01;
	[SerializeField]
	private ParticleSystem _sparks02;

	public void SparksEffect(bool value)
	{
		if (value)
		{
			_sparks01.Play();
			_sparks02.Play();
		}
		else
		{
			_sparks01.Stop();
			_sparks02.Stop();
		}
	}
}