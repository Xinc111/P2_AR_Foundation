using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] private AudioSource music;
    [SerializeField] private AudioSource buttonSFX;

    [SerializeField] private Button musicButton; // Referencia al botón de música
    [SerializeField] private Button sfxButton;   // Referencia al botón de SFX
    [SerializeField] private Button exitButton;  // Referencia al botón de salida (exit)

    [SerializeField] private Color enabledColor = Color.white; // Color cuando está activado
    [SerializeField] private Color disabledColor = new Color(0.5f, 0.5f, 0.5f); // Color más oscuro cuando está desactivado

    private bool isMusicOn = true;
    private bool isSFXOn = true;

    void Start()
    {
        // Asegurarnos de que el botón de salida tenga un listener para el evento de click
        if (exitButton != null)
        {
            exitButton.onClick.AddListener(OnExitButtonClicked);
        }
    }

    // Función para alternar música
    public void ToggleMusic()
    {
        if (isMusicOn)
        {
            music.Pause();
            ChangeButtonColor(musicButton, false); // Cambiar el color del botón a desactivado
        }
        else
        {
            music.Play();
            ChangeButtonColor(musicButton, true); // Cambiar el color del botón a activado
        }

        isMusicOn = !isMusicOn;
    }

    // Función para alternar SFX
    public void ToggleSFX()
    {
        isSFXOn = !isSFXOn;
        ChangeButtonColor(sfxButton, isSFXOn); // Cambiar el color del botón a activado o desactivado
    }

    // Función para cambiar el color del botón
    private void ChangeButtonColor(Button button, bool isEnabled)
    {
        if (button != null)
        {
            // Obtener la referencia al componente Image
            Image buttonImage = button.GetComponent<Image>();

            if (buttonImage != null)
            {
                buttonImage.color = isEnabled ? enabledColor : disabledColor; // Cambiar el color
            }

            // Cambiar el ColorBlock para reflejar el estado desactivado/activado
            ColorBlock colorBlock = button.colors;
            colorBlock.normalColor = isEnabled ? enabledColor : disabledColor;
            colorBlock.highlightedColor = isEnabled ? enabledColor : disabledColor;
            colorBlock.pressedColor = isEnabled ? enabledColor : disabledColor;
            colorBlock.selectedColor = isEnabled ? enabledColor : disabledColor;
            button.colors = colorBlock;
        }
    }

    // Función para reproducir el sonido del botón
    public void PlayButtonSound()
    {
        if (isSFXOn && buttonSFX != null)
        {
            buttonSFX.Play();
        }
    }

    // Método que se llama cuando el botón de salida es presionado
    void OnExitButtonClicked()
    {
        // Mostrar un mensaje en la consola para verificar que el botón está funcionando
        Debug.Log("Exit Button Pressed");

        // Cerrar la aplicación
        Application.Quit();

        // Si estás en el editor de Unity, la función Application.Quit no cerrará el editor,
        // pero puedes simularlo con el siguiente código (solo en el Editor de Unity):
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
