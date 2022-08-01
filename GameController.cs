using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameSettings settings;

    public bool pauseMenuClosed = true;
    public Animator pauseMenuAnim;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        pauseMenuAnim.SetBool("Open", false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            Cursor.lockState = (pauseMenuClosed) ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = pauseMenuClosed;
            pauseMenuClosed = !pauseMenuClosed;
            pauseMenuAnim.SetBool("Open", !pauseMenuClosed);
        }
    }

    public void ChangeVolume(Slider slider)
    {
        settings.musicVolume = slider.value;
    }

    public void ChangeCamSensitivity(Slider slider)
    {
        settings.camSensitivity = slider.value;
    }

    public void ExitToMenu() 
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void Respawn()
    {
        SceneManager.LoadScene("Game2.0");
    }

    public void ResumeGame() 
    {
        pauseMenuClosed = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        pauseMenuAnim.SetBool("Open", false);
    }
}
