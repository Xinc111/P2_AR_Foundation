using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    public static AudioManager Instance { get; private set; }

    public bool IsMusicOn { get; private set; }
    public bool IsSFXOn { get; private set; }

    private void Awake()
    {
        // Singleton para acceso global si quieres usarlo en otros scripts
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Opcional: conserva entre escenas
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Cargar preferencias guardadas
        IsMusicOn = PlayerPrefs.GetInt("MusicOn", 1) == 1;
        IsSFXOn = PlayerPrefs.GetInt("SFXOn", 1) == 1;

        if (IsMusicOn) musicSource.Play();
        else musicSource.Pause();
    }

    public void ToggleMusic()
    {
        IsMusicOn = !IsMusicOn;

        if (IsMusicOn) musicSource.Play();
        else musicSource.Pause();

        PlayerPrefs.SetInt("MusicOn", IsMusicOn ? 1 : 0);
    }

    public void ToggleSFX()
    {
        IsSFXOn = !IsSFXOn;
        PlayerPrefs.SetInt("SFXOn", IsSFXOn ? 1 : 0);
    }

    public void PlaySFX()
    {
        if (IsSFXOn && sfxSource != null)
        {
            sfxSource.Play();
        }
    }
}
