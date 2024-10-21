using System;
using UnityEngine;

public class UILoader : UIServiceComponent
{
	[SerializeField]
	private UIScreenController m_uiController;

	private LoaderService _loaderService;

	override protected void Awake()
	{
		base.Awake();

		DontDestroyOnLoad(gameObject);

		_loaderService = App.Instance.Services.Get<LoaderService>();
	}

	/*private void Start()
	{
		//test...
		m_uiController.Show<UILoaderScreen2>()
			.OnShow((x) =>
			{
				m_uiController.Hide<UILoaderScreen2>();
			});
	}*/

	public void LoadScene(int indexScene, Action action = null)
	{
		m_uiController.Show<UILoader2Screen>()
			.OnShow((x) =>
			{
				_loaderService.LoadScene(indexScene, 0.1f)
					.OnComplete(() =>
					{
						m_uiController.Hide<UILoader2Screen>();

						action?.Invoke();
					});
			});
	}
}