using DG.Tweening;
using SlotGame.Profile;
using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button), typeof(CanvasGroup))]
public class UIToolboxPanel : MonoBehaviour
{
	public Action onClick;

	[SerializeField]
	private UIImageEffect m_iconOpen;
	[SerializeField]
	private Image m_iconClose;

	[SerializeField]
	private TMP_Text m_cooldownText;

	private PlayerData _playerData;
	private StatsBehaviour _stats;
	private GameplayEventsService _gameplayEvents;

	private Button _button;
	protected CanvasGroup _canvasGroup;

	private void Awake()
	{
		_playerData = Profile.Instance.Get<PlayerData>().data;
		_stats = App.Instance.Gameplay.Stats;
		_gameplayEvents = App.Instance.Services.Get<GameplayEventsService>();

		_button = GetComponent<Button>();
		_canvasGroup = GetComponent<CanvasGroup>();
	}

	private void OnEnable()
	{
		_gameplayEvents.Match3ReadyToPlay += Match3ReadyToPlay;

		_button.onClick.AddListener(OnClick);

		SetVisible(true);
		SetState(!_stats.IsMatch3Cooldown);
	}
	private void OnDisable()
	{
		_gameplayEvents.Match3ReadyToPlay -= Match3ReadyToPlay;

		_button.onClick.RemoveListener(OnClick);
	}

	private void Update()
	{
		if (_stats.IsMatch3Cooldown)
		{
			SetCooldownProgress(_playerData.match3Cooldown.endTime - DateTime.UtcNow);
		}
	}

	public void SetVisible(bool open)
	{
		if (open)
		{
			_canvasGroup.blocksRaycasts = true;
			_canvasGroup.DOFade(1.0f, 0.25f);
		}
		else
		{
			_canvasGroup.blocksRaycasts = false;
			_canvasGroup.alpha = 0.0f;
		}
	}

	public void SetState(bool open)
	{
		m_iconOpen.gameObject.SetActive(open);
		m_iconClose.gameObject.SetActive(!open);
		m_cooldownText.gameObject.SetActive(!open);

		if (open)
		{
			m_iconOpen.FlashScaleUpEffect();
		}
	}

	public void SetCooldownProgress(TimeSpan timeSpan)
	{
		//m_cooldownText.text = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
		m_cooldownText.text = timeSpan.ToString(@"mm\:ss");
	}

	private void OnClick()
	{
		if (!_stats.IsMatch3Cooldown)
		{
			onClick?.Invoke();
		}
	}

	private void Match3ReadyToPlay(bool state)
	{
		SetState(state);
	}
}