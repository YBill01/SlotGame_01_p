using Game.Profile;
using System;
using UnityEngine;
using UnityEngine.UI;

public class UISettingsScreen : UIScreenPopup
{
	public event Action Exit;
	public event Action Back;
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

	private void Start()
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
	}
	private void OnDisable()
	{
		m_soundToggle.onValueChanged -= SoundToggleOnValueChanged;
		m_musicToggle.onValueChanged -= MusicToggleOnValueChanged;

		m_backButton.onClick.RemoveListener(BackButtonOnClick);
		m_exitButton.onClick.RemoveListener(ExitButtonOnClick);
	}

	public void BackButtonOnDown()
	{
		SelfHideInternal();
	}

	public void SetHomeMode()
	{
		m_backButton.gameObject.SetActive(false);
		m_exitButton.gameObject.SetActive(true);
	}
	public void SetPlayMode()
	{
		m_backButton.gameObject.SetActive(true);
		m_exitButton.gameObject.SetActive(false);
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
}