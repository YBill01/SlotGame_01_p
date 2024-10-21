using SlotGame.Profile;
using System;
using UnityEngine;
using UnityEngine.UI;

public class UISettingsScreen : UIPopupScreen
{
	public event Action Exit;
	public event Action Back;
	public event Action Info;
	public event Action<bool> SoundOn;
	public event Action<bool> MusicOn;

	[Space]
	[SerializeField]
	private UIToggleComponent m_soundToggle;
	[SerializeField]
	private UIToggleComponent m_musicToggle;

	[Space]
	[SerializeField]
	private Button m_backButton;
	[SerializeField]
	private Button m_exitButton;

	[Space]
	[SerializeField]
	private Button m_infoButton;

	protected override void OnPreShow()
	{
		AppData appData = Profile.Instance.Get<AppData>().data;
		m_soundToggle.Value = appData.soundOn;
		m_musicToggle.Value = appData.musicOn;
	}

	private void OnEnable()
	{
		m_soundToggle.onValueChanged += SoundToggleOnValueChanged;
		m_musicToggle.onValueChanged += MusicToggleOnValueChanged;

		m_backButton.onClick.AddListener(BackButtonOnClick);
		m_exitButton.onClick.AddListener(ExitButtonOnClick);

		m_infoButton.onClick.AddListener(InfoButtonOnClick);
	}
	private void OnDisable()
	{
		m_soundToggle.onValueChanged -= SoundToggleOnValueChanged;
		m_musicToggle.onValueChanged -= MusicToggleOnValueChanged;

		m_backButton.onClick.RemoveListener(BackButtonOnClick);
		m_exitButton.onClick.RemoveListener(ExitButtonOnClick);

		m_infoButton.onClick.RemoveListener(InfoButtonOnClick);
	}

	public void BackButtonOnDown()
	{
		SelfHideInternal();
	}

	public void SetHomeMode()
	{
		m_backButton.gameObject.SetActive(false);
		m_exitButton.gameObject.SetActive(true);

		m_infoButton.gameObject.SetActive(false);
	}
	public void SetPlayMode()
	{
		m_backButton.gameObject.SetActive(true);
		m_exitButton.gameObject.SetActive(false);

		m_infoButton.gameObject.SetActive(true);
	}

	private void SoundToggleOnValueChanged(bool value)
	{
		SoundOn?.Invoke(value);
	}
	private void MusicToggleOnValueChanged(bool value)
	{
		MusicOn?.Invoke(value);
	}

	private void BackButtonOnClick()
	{
		Back?.Invoke();
	}
	private void ExitButtonOnClick()
	{
		Exit?.Invoke();
	}
	
	private void InfoButtonOnClick()
	{
		Info?.Invoke();
	}
}