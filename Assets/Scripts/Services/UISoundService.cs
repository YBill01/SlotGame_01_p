using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;

public class UISoundService : UIServiceComponent
{
	[SerializeField]
	private AudioMixer m_audioMixer;

	[Header("AudioSources")]
	[SerializeField]
	private AudioSource m_musicSource;
	[SerializeField]
	private AudioSource m_sfxSource;
	[SerializeField]
	private AudioSource m_sfxLoopSource;

	[Header("AudioClips")]
	[SerializeField]
	private AudioClip[] m_musicClips;
	[Space(10)]
	[SerializeField]
	private AudioClip[] m_sfxClips;

	protected override void Awake()
	{
		DontDestroyOnLoad(gameObject);

		base.Awake();
	}

	public void PlayMusic(int index)
	{
		m_musicSource.clip = m_musicClips[index];

		m_musicSource.Play();
		m_musicSource.DOFade(1.0f, 0.5f)
			.From(0.0f);
	}
	public void StopMusic()
	{
		m_musicSource.DOFade(0.0f, 0.5f)
			.From(1.0f)
			.OnComplete(() =>
			{
				m_musicSource.Stop();
			});
	}
	public void PlayMusicOnceShot(int index)
	{
		m_musicSource.PlayOneShot(m_musicClips[index]);
	}

	public void PlaySFXOnceShot(int index, bool randomPith = false)
	{
		if (randomPith)
		{
			AudioSource randomPithAudioSource = gameObject.AddComponent<AudioSource>();
			randomPithAudioSource.outputAudioMixerGroup = m_sfxSource.outputAudioMixerGroup;
			randomPithAudioSource.pitch = Random.Range(0.9f, 1.1f);
			randomPithAudioSource.loop = false;
			randomPithAudioSource.PlayOneShot(m_sfxClips[index]);
			Destroy(randomPithAudioSource, m_sfxClips[index].length / Mathf.Abs(randomPithAudioSource.pitch));
		}
		else
		{
			m_sfxSource.PlayOneShot(m_sfxClips[index]);
		}
	}
	public void PlaySFX(int index)
	{
		m_sfxLoopSource.clip = m_sfxClips[index];

		m_sfxLoopSource.Play();
		m_sfxLoopSource.DOFade(1.0f, 0.5f)
			.From(0.0f);
	}
	public void StopSFX()
	{
		m_sfxLoopSource.DOFade(0.0f, 0.5f)
			.From(1.0f)
			.OnComplete(() =>
			{
				m_sfxLoopSource.Stop();
			});
	}

	public void MusicVolume(float value)
	{
		m_audioMixer.SetFloat("music", Mathf.Log10(value) * 20);
	}
	public void SFXVolume(float value)
	{
		m_audioMixer.SetFloat("sfx", Mathf.Log10(value) * 20);
	}
}