using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour{

    #region End Game Scrren Buttons
    public void TryAgainButton() {
        SceneManager.LoadScene(FindObjectOfType<StatusHandler>().GetCurrentGameScene());
    }
    public void BackToSubMenuButton() {
        SceneManager.LoadScene(FindObjectOfType<StatusHandler>().GetCurrentSubMenu());
        // Make Background music loudered again
        FindObjectOfType<AudioManager>().SetVolume("Background Music", 0.07f);
    }
    public void NextGameButton() {
        if (SceneUtility.GetBuildIndexByScenePath(FindObjectOfType<StatusHandler>().GetNextGameScene()) >= 0)
            SceneManager.LoadScene(FindObjectOfType<StatusHandler>().GetNextGameScene());
        else {
            SceneManager.LoadScene(FindObjectOfType<StatusHandler>().GetCurrentSubMenu());
        }
    }
    #endregion

    public void BackToStartMenuButton() {
        SceneManager.LoadScene("Menus");
        // Make Background music loudered again
    }
    public void QuitButton() {
        Application.Quit();
    }

}
