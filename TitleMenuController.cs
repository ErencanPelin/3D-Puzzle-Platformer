using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleMenuController : MonoBehaviour
{
    public Animator settingsAnim;
    public GameSettings settings;
    public GameObject musicControl;

    private void Awake()
    {
        if (GameObject.Find("SettingsObject(Clone)") == null)
        {
            Instantiate(musicControl);
        }

        settingsAnim.SetBool("Open", false);
    }

    public void OpenSettings() 
    {
        settingsAnim.SetBool("Open", true);
    }

    public void CloseSettings() 
    {
        settingsAnim.SetBool("Open", false);
    }

    public void LoadGame() 
    {
        SceneManager.LoadScene("Game2.0");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ChangeCamSensitivity(Slider slider) 
    {
        settings.camSensitivity = slider.value;
    }
}
