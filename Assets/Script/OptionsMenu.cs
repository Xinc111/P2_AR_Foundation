using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] private Button musicButton;
    [SerializeField] private Button sfxButton;
    [SerializeField] private Button exitButton;

    [SerializeField] private Color enabledColor = Color.white;
    [SerializeField] private Color disabledColor = new Color(0.5f, 0.5f, 0.5f);

    private void Start()
    {
        // Sincronizar colores con el estado de AudioManager
        UpdateButtonVisuals();

        if (exitButton != null)
        {
            exitButton.onClick.AddListener(OnExitButtonClicked);
        }
    }

    public void ToggleMusic()
    {
        AudioManager.Instance.ToggleMusic();
        UpdateButtonColor(musicButton, AudioManager.Instance.IsMusicOn);
        AudioManager.Instance.PlaySFX();
    }

    public void ToggleSFX()
    {
        AudioManager.Instance.ToggleSFX();
        UpdateButtonColor(sfxButton, AudioManager.Instance.IsSFXOn);
        AudioManager.Instance.PlaySFX();
    }

    private void UpdateButtonVisuals()
    {
        UpdateButtonColor(musicButton, AudioManager.Instance.IsMusicOn);
        UpdateButtonColor(sfxButton, AudioManager.Instance.IsSFXOn);
    }

    private void UpdateButtonColor(Button button, bool isEnabled)
    {
        if (button == null) return;

        Image buttonImage = button.GetComponent<Image>();
        if (buttonImage != null)
        {
            buttonImage.color = isEnabled ? enabledColor : disabledColor;
        }

        ColorBlock colors = button.colors;
        colors.normalColor = isEnabled ? enabledColor : disabledColor;
        colors.highlightedColor = isEnabled ? enabledColor : disabledColor;
        colors.pressedColor = isEnabled ? enabledColor * 0.9f : disabledColor * 0.9f;
        colors.selectedColor = isEnabled ? enabledColor : disabledColor;
        button.colors = colors;
    }

    public void OnRestartPress()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        Debug.Log("Reiniciando la escena: " + currentScene.name); // Mensaje en consola
        SceneManager.LoadScene(currentScene.buildIndex);
    }

    private void OnExitButtonClicked()
    {
        Debug.Log("Exit Button Pressed");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
