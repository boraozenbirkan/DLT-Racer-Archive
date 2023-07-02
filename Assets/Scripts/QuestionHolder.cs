using UnityEngine;
using UnityEngine.UI;

public class QuestionHolder : MonoBehaviour{
    [Header("Question Info")]
    [SerializeField] public string questionCode = null;
    [SerializeField] string[] optionTexts = null;
    [SerializeField] [Range(0, 3)] int correctNumber = 0;


    [SerializeField] UnityEngine.UI.Button[] buttons = null;

    RaceHandler raceHandler;
    int selectedButton = 0, newCorrectNumber = 0, currentOKP;
    public ColorBlock correctColor;
    public ColorBlock WrongColor;
    bool selected = false;
    string correctOptionText;
    int[] notTakenNums = new int[] { 0, 1, 2, 3};

    void Start() {
        // Detecting relative objects
        raceHandler = FindObjectOfType<RaceHandler>();

        // Loading correct option's text
        correctOptionText = optionTexts[correctNumber];
        
        // Making options randomly ordered
        for (int order = 0; order < optionTexts.Length; order++) {
            int randomOption = getRandomOrder();
            buttons[order].GetComponentInChildren<Text>().text = optionTexts[randomOption];
        }

        // Loading new correct button number
        for (int buttonNum = 0; buttonNum < 4; buttonNum++) {
            if (correctOptionText.Equals(buttons[buttonNum].GetComponentInChildren<Text>().text))
                newCorrectNumber = buttonNum;
        }

        // Updating or loading the OKP
        if (PlayerPrefs.HasKey(questionCode))
            currentOKP = PlayerPrefs.GetInt(questionCode);
        else
            PlayerPrefs.SetInt(questionCode, 100);
    }

    // ##################### PUBLIC METHODS ##################### //
    #region ButtonMethods
    public void SelectedButtonFirst() {
        if (selected || !FindObjectOfType<Player>()) { return; }
        selectedButton = 0;
        ButtonSelectionEvents();
        selected = true;
    }
    public void SelectedButtonSecond() {
        if (selected || !FindObjectOfType<Player>()) { return; }
        selectedButton = 1;
        ButtonSelectionEvents();
        selected = true;
    }
    public void SelectedButtonThird() {
        if (selected || !FindObjectOfType<Player>()) { return; }
        selectedButton = 2;
        ButtonSelectionEvents();
        selected = true;
    }
    public void SelectedButtonForth() {
        if (selected || !FindObjectOfType<Player>()) { return; }
        selectedButton = 3;
        ButtonSelectionEvents();
        selected = true;
    }
    public void SetInteractable(bool newStatus) {
        foreach (UnityEngine.UI.Button button in buttons) {
            button.interactable = newStatus;
        }
    }
    #endregion

    // ###### Getters and Setters ###### //

    public void SetPos(float x, float y) {
        Vector3 newPos = new Vector3(x, y, 0);
        transform.position = newPos;
    }
    public int GetCurrentCorretNumber() {
        return newCorrectNumber;
    }



    // ##################### PRIVATE METHODS ##################### //
    private void ButtonSelectionEvents() {
        SetInteractable(false);
        //FindObjectOfType<QuestionPlaceHolder>().triggered = true; //For debugging
        if (selectedButton == newCorrectNumber) {
            buttons[selectedButton].colors = correctColor;
            currentOKP = PlayerPrefs.GetInt(questionCode) + 5;
            PlayerPrefs.SetInt(questionCode, currentOKP);
            raceHandler.ButtonSelectionEvent(true, selectedButton);
        }
        else {
            buttons[selectedButton].colors = WrongColor;
            buttons[newCorrectNumber].colors = correctColor;
            currentOKP = PlayerPrefs.GetInt(questionCode) - 5;
            PlayerPrefs.SetInt(questionCode, currentOKP);
            raceHandler.ButtonSelectionEvent(false, selectedButton);
        }
    }    

    private int getRandomOrder() {
        bool gotIt = false;
        int tmpNum = 0;
        while (!gotIt) {
            tmpNum = Random.Range(0, 4);
            for (int num = 0; num < notTakenNums.Length; num++) {
                if (tmpNum == notTakenNums[num]) {
                    notTakenNums[num] = 5;
                    gotIt = true;
                }
            }
        }
        return tmpNum;
    }
}
