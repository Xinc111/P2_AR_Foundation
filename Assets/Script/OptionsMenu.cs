using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] private AudioSource music;
    [SerializeField] private AudioSource buttonSFX;

    [SerializeField] private Button musicButton; // Referencia al bot�n de m�sica
    [SerializeField] private Button sfxButton;   // Referencia al bot�n de SFX
    [SerializeField] private Button exitButton;  // Referencia al bot�n de salida (exit)

    [SerializeField] private Color enabledColor = Color.white; // Color cuando est� activado
    [SerializeField] private Color disabledColor = new Color(0.5f, 0.5f, 0.5f); // Color m�s oscuro cuando est� desactivado

    private bool isMusicOn = true;
    private bool isSFXOn = true;

    void Start()
    {
        // Asegurarnos de que el bot�n de salida tenga un listener para el evento de click
        if (exitButton != null)
        {
            exitButton.onClick.AddListener(OnExitButtonClicked);
        }
    }

    // Funci�n para alternar m�sica
    public void ToggleMusic()
    {
        if (isMusicOn)
        {
            music.Pause();
            ChangeButtonColor(musicButton, false); // Cambiar el color del bot�n a desactivado
        }
        else
        {
            music.Play();
            ChangeButtonColor(musicButton, true); // Cambiar el color del bot�n a activado
        }

        isMusicOn = !isMusicOn;
    }

    // Funci�n para alternar SFX
    public void ToggleSFX()
    {
        isSFXOn = !isSFXOn;
        ChangeButtonColor(sfxButton, isSFXOn); // Cambiar el color del bot�n a activado o desactivado
    }

    // Funci�n para cambiar el color del bot�n
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

    // Funci�n para reproducir el sonido del bot�n
    public void PlayButtonSound()
    {
        if (isSFXOn && buttonSFX != null)
        {
            buttonSFX.Play();
        }
    }

    // M�todo que se llama cuando el bot�n de salida es presionado
    void OnExitButtonClicked()
    {
        // Mostrar un mensaje en la consola para verificar que el bot�n est� funcionando
        Debug.Log("Exit Button Pressed");

        // Cerrar la aplicaci�n
        Application.Quit();

        // Si est�s en el editor de Unity, la funci�n Application.Quit no cerrar� el editor,
        // pero puedes simularlo con el siguiente c�digo (solo en el Editor de Unity):
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
