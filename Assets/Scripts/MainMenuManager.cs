using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; // ¡Esencial para que funcionen las corrutinas!
using UnityEngine.EventSystems;

public class MainMenuManager : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject controlsPanel;

    [Header("Fade Settings")]
    [SerializeField] private CanvasGroup fadeGroup; // Aquí arrastrarás el panel del Fade
    [SerializeField] private float fadeDuration = 1.5f;

    [Header("Keyboard Navigation")]
    [SerializeField] private GameObject backButton;        // <--- NUEVO: El botón de regresar del panel de controles
    [SerializeField] private GameObject controlsMenuButton; // <--- NUEVO: El botón de "Controles" del menú principal

    public void PlayGame()
    {
        // Iniciamos la transición en lugar de cargar la escena directo
        StartCoroutine(FadeAndLoad());
    }


    public void QuitGame()
    {
        Debug.Log("Cerrando Solento Flower...");
        Application.Quit();
    }

    // Esta parte estaba fuera de la clase, por eso fallaba
    IEnumerator FadeAndLoad()
    {
        float timer = 0f;

        // Bucle de fade-in (aparece el negro)
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            // Asegúrate de que fadeGroup no sea nulo antes de acceder a él
            if (fadeGroup != null) 
            {
                fadeGroup.alpha = timer / fadeDuration;
            }
            yield return null;
        }

        // Carga la escena
        SceneManager.LoadScene("Nivel_01");
    }

    public void OpenControls()
    {
        if (controlsPanel != null)
        {
            controlsPanel.SetActive(true);
            
            // NUEVO: Limpiamos la selección actual y ponemos el foco en el botón de regresar
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(backButton);
        }
    }

    public void CloseControls()
    {
        if (controlsPanel != null)
        {
            controlsPanel.SetActive(false);
            
            // NUEVO: Al cerrar, devolvemos el foco al botón de "Controles" del menú principal
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(controlsMenuButton);
        }
    }
}