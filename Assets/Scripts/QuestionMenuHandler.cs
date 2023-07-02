using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class QuestionMenuHandler : MonoBehaviour{

    [SerializeField] Image[] stars = null;
    [SerializeField] GameObject locked = null;
    [SerializeField] bool isTheFirstGameScene = false, isMainMenuItem = false;

    //VaaT
    int gameSceneStar = -1; // Because 0 star means player got 1 star
    string gameSceneNumberLocked;

    void Start()    {
        if (isMainMenuItem) { return; }

        // Unlock the scene if needed
        gameSceneNumberLocked = name + "_isLocked";
        if (isTheFirstGameScene || PlayerPrefs.GetInt(gameSceneNumberLocked) == 1) {
            locked.SetActive(false);
            GetComponent<Button>().interactable = true;
        }
        else {
            locked.SetActive(true);
            GetComponent<Button>().interactable = false;
        }

        // Updating current star situation
        if (PlayerPrefs.HasKey(name)) {
            gameSceneStar = PlayerPrefs.GetInt(name);
        }
        else
            PlayerPrefs.SetInt(name, -1);   // Because 0 star means player got 1 star
        for (int i = 2; i > gameSceneStar; i--) {
            stars[i].enabled = false;
        }
    }
    void Update()    {        
    }
    public void LoadTheGameScene() {
        SceneManager.LoadScene(name);
    }
}
