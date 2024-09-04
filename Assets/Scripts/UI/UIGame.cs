using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGame : UIServiceComponent
{
	[SerializeField]
	private UIScreenController m_uiController;

	private UIEvenetsService _uiEvenetsService;

	protected override void Initialize()
	{
		_uiEvenetsService = App.Instance.Services.Get<UIEvenetsService>();
	}


}