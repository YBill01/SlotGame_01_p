using UnityEngine;

public class UIInfoScreen : UIPopupScreen
{
	[Space]
	[SerializeField]
	private UIInfoPatternsPanel m_patternsPanel;

	protected override void OnInit()
	{
		base.OnInit();

		m_patternsPanel.SetData(App.Instance.Gameplay.Stats.GetCurrentLevelData().slotConfig.pattern);
		m_patternsPanel.CreateItems();
	}
	protected override void OnHide()
	{
		m_patternsPanel.ClearItems();
	}

	public void BackButtonOnDown()
	{
		SelfHideInternal();
	}
}