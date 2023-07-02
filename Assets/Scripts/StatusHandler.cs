using UnityEngine;
using UnityEngine.SceneManagement;

public class StatusHandler : MonoBehaviour {

    string currentSubMenu = null;
    int currentGameSceneNumber = 0;
    public int livesLeft;

    // Dont Destroy Stuff
    private static StatusHandler thisObject = null;
    private void Awake() {
        if (thisObject == null) {
            thisObject = this;
            DontDestroyOnLoad(this);
        }
        else
            Destroy(gameObject);
    }
    // End of the dont destroy stuff

    #region Screen Loads
    public void WinStatus() {
        livesLeft = FindObjectOfType<RaceHandler>().currentLives;   
        PlayerPrefs.SetInt(GetCurrentGameScene(), livesLeft);       // In a win situation,         
        string nextSceneUnlock = GetNextGameScene() + "_isLocked";  // Record the stars player got        
        PlayerPrefs.SetInt(nextSceneUnlock, 1);                     // Unlock next game scene
        SceneManager.LoadScene("Win");
    }
    public void GameOverStatus() {
        SceneManager.LoadScene("Game Over");
    }
    public void TimeOutStatus() {
        SceneManager.LoadScene("Time Out");
    }
    #endregion
    #region End Game Screen Stuff
    public string GetCurrentSubMenu() {
        return currentSubMenu;
    }
    public string GetCurrentGameScene() {
        return currentSubMenu + currentGameSceneNumber.ToString();
    }
    public string GetNextGameScene() {
        return currentSubMenu + (currentGameSceneNumber + 1).ToString();
    }
    #endregion

    public void UpdateGameSceneInfo(string currentSubMenu, int currentGameSceneNumber) {
        this.currentSubMenu = currentSubMenu;
        this.currentGameSceneNumber = currentGameSceneNumber;
    }
}
