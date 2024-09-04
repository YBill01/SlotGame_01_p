using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UIBackgroundScrollBehaviour : MonoBehaviour
{
	[SerializeField]
	private float m_scrollSpeed = 0.2f;

	private Image _image;

	private void Awake()
	{
		_image = GetComponent<Image>();

		_image.material = new Material(_image.material);
		_image.material.SetFloat("_ScrollSpeed", m_scrollSpeed);
		//_image.material.SetVector("_ScrollSpeed", new Vector3(0.0f, m_scrollSpeed, 0.0f));
	}

	private void OnDestroy()
	{
		DestroyImmediate(_image.material);
	}
}