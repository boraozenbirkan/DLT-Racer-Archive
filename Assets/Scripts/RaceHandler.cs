using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RaceHandler : MonoBehaviour {
    [Header("Relatives")]
    [SerializeField] Image[] lifeImages = null;
    [SerializeField] TextMeshProUGUI questionNumberText = null;
    [SerializeField] VehicleSpawner[] vehicleSpawners = null;
    [SerializeField] TreeSpawner treeSpawner = null;
    [SerializeField] QuestionHolder[] givenQuestions = null;
    [SerializeField] RoadScrolling road = null;
    [SerializeField] QuestionPlaceHolder questionPlaceHolder = null;
    [SerializeField] FuelBar fuelBar = null;
    [SerializeField] string currenSubMenu = null;
    [SerializeField] int currentGameSceneNumber = 0;
    [SerializeField] bool HTPModeOn = false;

    // Relative Game Objects
    Player currentPlayerObject;

    // VaaT (Variables as a Tool)
    QuestionHolder[] randomQuestions = null;
    int[] notTakenNums = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9};
    [HideInInspector] public int currentLives = 2;
    /*[SerializeField]*/ bool transactionStageHasChanged = false;
    /*[SerializeField]*/ int currentTransactionStage = 0;
    /*[SerializeField]*/ int currentNumberOfQuestion = 0;
    /*[SerializeField]*/ bool testModeOn = false;
    int selectedButtonNumber;
    [HideInInspector] public int previousNumberOfQuestion;
    AudioManager audioManager;
    bool doItOnce0 = true;


    [HideInInspector] public bool inLastQuestion = false, currentNumberOfQuestionHasChanged = false;
    float currentYPos;
    float questionXPos = 0.0f;
    



    void Start() {
        if (HTPModeOn) { return; }
        // Make Background music less louder
        audioManager = FindObjectOfType<AudioManager>();
        audioManager.SetVolume("Background Music", 0.05f);
        audioManager.Play("Outdoor");
        // creating size related randomquestions.
        randomQuestions = new QuestionHolder[givenQuestions.Length]; ; 

        // Initializing Objects
        currentPlayerObject = FindObjectOfType<Player>();

        // Randomly reorganizing the question order
        for (int order = 0; order < givenQuestions.Length; order++) {
            int randomOption = getRandomOrder();
            randomQuestions[order] = givenQuestions[randomOption];

            currentYPos = randomQuestions[order].transform.localPosition.y;
            Vector3 newPos = new Vector3(questionXPos, currentYPos, 0);
            randomQuestions[order].transform.localPosition = newPos;
            questionXPos += 2000.0f;
        }

        // Giving initial value to Number of Question Text
        questionNumberText.text = (currentNumberOfQuestion + 1).ToString();

        // Giving initial stage info to all objects
        currentTransactionStage = 0; 
        transactionStageHasChanged = true;
    }
    void Update() {
        if (doItOnce0) {
            currentPlayerObject = FindObjectOfType<Player>();
        }
        if (HTPModeOn) { return; }
        if (transactionStageHasChanged) {
            // Fuel Up if we entered stage 3 (New question coming)
            if (currentTransactionStage == 3) { fuelBar.FuelUp(); }
            
            // Set all objects' stage level
            currentPlayerObject.SetStageBehaviour(currentTransactionStage);
            foreach (GameVehicle currentVehicles in FindObjectsOfType<GameVehicle>()) {
                currentVehicles.SetSpeedLevel(currentTransactionStage);
            }
            foreach (VehicleSpawner spawner in vehicleSpawners) {
                spawner.SetStageBehaviour(currentTransactionStage);
            }
            foreach (GameTree currentTrees in FindObjectsOfType<GameTree>()) {
                currentTrees.SetSpeedLevel(currentTransactionStage);
            }
            treeSpawner.SetStageBehavior(currentTransactionStage);
            road.SetSpeedLevel(currentTransactionStage);
            questionPlaceHolder.SetStageBehaviour(currentTransactionStage);
            transactionStageHasChanged = false;
        }
        TestMode();
    }

    

    //  ------------------------------------------------    //
    //                                                      //
    //                  PUBLIC METHODS                      //
    //                                                      //
    //  ------------------------------------------------    //

    public void AccidentOccured() {
        if (currentLives > 0) {
            ReduceLife();
            StartCoroutine(DeadRoutine());
            Invoke("StageZeroDelay", 1);
        }
        else {
            ReduceLife();
            Invoke("PlayGameOverScene", 2);        
        }
    }
    public void ButtonSelectionEvent(bool correctButtonSelected, int selectedButtonNumber) {
        // We are taking info about player did choose true or false option
        // With 2 lines below, we are changing the stage to 1 and saying that
        // "Yeah, somethings happened"
        if (inLastQuestion && correctButtonSelected) {
            PlayWinScreen();
            return;
        }
        else if (correctButtonSelected) {
            questionPlaceHolder.playerIsDead = true;
        }
        currentNumberOfQuestionHasChanged = false;
        this.selectedButtonNumber = selectedButtonNumber;
        SetTransactionStage(1);
    }
    public void SetStartingSpawner() {
        vehicleSpawners[GetCurrentTurth()].SetSpawn(false);
    }

    //  --------------------------------------------------  //
    //              Getters And Setters                     //
    //  --------------------------------------------------  //

    public void SetQuestionNumberText(int currentNumberOfQuestion) {
        this.currentNumberOfQuestion = currentNumberOfQuestion;
        questionNumberText.text = (currentNumberOfQuestion + 1).ToString();
        currentNumberOfQuestionHasChanged = true;
    }
    public int GetSelectedButtonNumber() {
        return selectedButtonNumber;
    }
    public void SetTransactionStage (int newStage) {
        currentTransactionStage = newStage;
        transactionStageHasChanged = true;
    }
    public int GetTransactionStage() {
        return currentTransactionStage;
    }
    public int GetPreviousTruth() {
        if (!inLastQuestion)
            return randomQuestions[currentNumberOfQuestion - 1].GetCurrentCorretNumber();
        else
            return randomQuestions[currentNumberOfQuestion].GetCurrentCorretNumber();
    }
    public int GetCurrentTurth() {
        return randomQuestions[currentNumberOfQuestion].GetCurrentCorretNumber();
    }
    public QuestionHolder[] GetOrderedQuestions() {
        return randomQuestions;
    }

    //  ------------------------------------------------    //
    //                                                      //
    //                  PRIVATE METHODS                     //
    //                                                      //
    //  ------------------------------------------------    //

    private void ReduceLife() {
        lifeImages[currentLives].enabled = false;
        currentLives--;
    }
    private IEnumerator DeadRoutine() {
        yield return new WaitForSeconds(1);
        questionPlaceHolder.playerIsDead = true;
        // Bring back player to scope
        currentPlayerObject.transform.position = new Vector2(0.9f, 0f);
        if (inLastQuestion) {
            currentPlayerObject.startCurrentLine = true; // If it is in Last question, that send it "current" correct pos.
            PlayWinScreen();
        }
        else if (currentNumberOfQuestionHasChanged) {
            currentPlayerObject.startNextLine = true; // Settle it in the next line
            currentNumberOfQuestionHasChanged = false;
        } else {
            currentPlayerObject.startCurrentLine = true;
        }
        currentPlayerObject.GetComponent<BoxCollider2D>().enabled = true;
        var endTime = Time.time + 3;
        while (Time.time < endTime) {
            currentPlayerObject.GetComponentInChildren<SpriteRenderer>().enabled = false;
            yield return new WaitForSeconds(0.2f);
            currentPlayerObject.GetComponentInChildren<SpriteRenderer>().enabled = true;
            yield return new WaitForSeconds(0.2f);
        }
    }
    private void StageZeroDelay() {
        SetTransactionStage(0);
    }
    private int getRandomOrder() {
        bool gotIt = false;
        int tmpNum = 0;
        while (!gotIt) {
            tmpNum = Random.Range(0, givenQuestions.Length);
            for (int num = 0; num < notTakenNums.Length; num++) {
                if (tmpNum == notTakenNums[num]) {
                    notTakenNums[num] = 50; // Giving an meaningless value
                    gotIt = true;
                }
            }
        }
        return tmpNum;
    }
    private void TestMode() {
        currentPlayerObject.testModeOn = testModeOn;
        foreach (GameVehicle currentVehicles in FindObjectsOfType<GameVehicle>()) {
            currentVehicles.testModeOn = testModeOn;
        }
        foreach (VehicleSpawner spawner in vehicleSpawners) {
            spawner.testModeOn = testModeOn;
        }
        foreach (GameTree currentTrees in FindObjectsOfType<GameTree>()) {
            currentTrees.testModeOn = testModeOn;
        }
        treeSpawner.testModeOn = testModeOn;
        road.testModeOn = testModeOn;
        questionPlaceHolder.testModeOn = testModeOn;
    }

    private void PlayWinScreen() {
        // Below 2 lines of code make player go right and disappear
        currentPlayerObject.SetStageBehaviour(0); // We need to do this for activate stage 0 beh.
        currentPlayerObject.SetSpeedLevel(1);
        Invoke("WinDelay", 4);
    }
    private void WinDelay() {
        audioManager.Stop("Outdoor");
        audioManager.SetPause("Background Music", true);
        FindObjectOfType<AdManager>().PlayInterstitialAd();
        FindObjectOfType<AdManager>().endGameType = "Win";        
        FindObjectOfType<StatusHandler>().UpdateGameSceneInfo(currenSubMenu, currentGameSceneNumber);   
    }
    private void PlayGameOverScene() {
        audioManager.Stop("Outdoor");
        audioManager.SetPause("Background Music", true);
        FindObjectOfType<AdManager>().PlayInterstitialAd();
        FindObjectOfType<AdManager>().endGameType = "Game Over";
        FindObjectOfType<StatusHandler>().UpdateGameSceneInfo(currenSubMenu, currentGameSceneNumber);
    }
    public void TimeOut() {
        audioManager.Stop("Outdoor");
        audioManager.SetPause("Background Music", true);
        FindObjectOfType<AdManager>().PlayInterstitialAd();
        FindObjectOfType<AdManager>().endGameType = "Time Out";
        FindObjectOfType<StatusHandler>().UpdateGameSceneInfo(currenSubMenu, currentGameSceneNumber);
    }
}
