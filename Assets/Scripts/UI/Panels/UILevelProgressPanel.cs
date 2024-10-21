using DG.Tweening;
using N.Fridman.FormatNums.Scripts.Helpers;
using SlotGame.Profile;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UILevelProgressPanel : MonoBehaviour, IUIStats
{
	[SerializeField]
	private Scrollbar m_scrollbar;
	[SerializeField]
	private UIImageEffect m_scrollbarLine;
	
	[SerializeField]
	private TMP_Text m_levelText;

	[SerializeField]
	private UIFloatingStringEffect m_floatingString;

	private PlayerData _playerData;
	private StatsBehaviour _stats;

	private int _amount;

	private Tween _tweenPunch;

	private void Awake()
	{
		_playerData = Profile.Instance.Get<PlayerData>().data;
		_stats = App.Instance.Gameplay.Stats;

		SetLevel(_playerData.progress.level);
		SetAmount(_playerData.progress.points);
	}

	public void SetLevel(int value)
	{
		m_levelText.text = $"level {value}";
	}

	public void SetAmount(int amount)
	{
		_amount = Mathf.Min(amount, _stats.GetCurrentLevelData().points);

		m_scrollbar.size = (float)_amount / _stats.GetCurrentLevelData().points;
	}

	public void Add(int count)
	{
		_tweenPunch?.Complete();
		_tweenPunch = (transform as RectTransform).DOPunchAnchorPos(new Vector2(Random.Range(-16.0f, 16.0f), 32.0f), 0.35f);

		m_scrollbarLine.FlashEffect();
		m_floatingString.FloatingStringEffect($"+{FormatNumsHelper.FormatNum((float)count)}", new Color(0.92f, 0.64f, 1.0f));

		SetAmount(_amount + count);
	}
	public void Take(int count)
	{
		m_floatingString.FloatingStringEffect($"-{FormatNumsHelper.FormatNum((float)count)}", Color.red);

		SetAmount(_amount - count);
	}

	public RectTransform GetRectTransformCollecting()
	{
		return (RectTransform)m_scrollbarLine.transform;
	}
}