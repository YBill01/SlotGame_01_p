using SlotGame.Profile;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILevelUpScreen : UIFadeFrontScreen
{
	[Space]
	[SerializeField]
	private Button m_closeButton;

	[Space]
	[SerializeField]
	private Animator m_personageAnimator;
	[SerializeField]
	private Animator m_screenAnimator;

	[Space]
	[SerializeField]
	private TMP_Text m_currentLevelText;
	[SerializeField]
	private TMP_Text m_levelText;
	[Space]
	[SerializeField]
	private TMP_Text m_rewardText;

	protected override void OnInit()
	{
		_fadeInDuration = 0.125f;
		_fadeOutDuration = 0.125f;

		m_currentLevelText.text = $"{Profile.Instance.Get<PlayerData>().data.progress.level}";
		m_levelText.text = $"{Profile.Instance.Get<PlayerData>().data.progress.level + 1}";

		int rewardAmount = 0;
		foreach (RewardData reward in App.Instance.Gameplay.Stats.GetCurrentLevelData().levelUpReward)
		{
			if (reward.item.type == ItemType.Coins)
			{
				rewardAmount += reward.count;
			}
		}

		m_rewardText.text = $"+{rewardAmount}";

		App.Instance.Gameplay.Stats.SetNewLevel(Profile.Instance.Get<PlayerData>().data.progress.level + 1);
	}

	protected override void OnShow()
	{
		App.Instance.Services
			.Get<UIService>()
			.Get<UISoundService>()
			.PlayMusicOnceShot(1);
	}

	private void OnEnable()
	{
		m_closeButton.onClick.AddListener(CloseButtonOnClick);
	}
	private void OnDisable()
	{
		m_closeButton.onClick.RemoveListener(CloseButtonOnClick);
	}

	private void OnPersonagePlayWin()
	{
		m_personageAnimator.SetTrigger("Win");
	}
	private void OnScreenPlayClose()
	{
		SelfHideInternal();
	}

	private void CloseButtonOnClick()
	{
		m_screenAnimator.SetTrigger("Close");
		m_personageAnimator.SetTrigger("Go");
	}
}