using System;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class Player : MonoBehaviour {
    // Configuration Parameters
    [Header("Player Movement")]
    [SerializeField] float changeAmountPerFrame = 0.001f;
    [SerializeField] int NumberOfFrame = 30;
    [SerializeField] float currentSpeed = 0;
    float speedOfLevelZero = 0f;
    float speedOfLevelOne = 1.5f;
    float speedOfLevelTwo = 1f;
    float speedOfLevelThree = 1.5f;

    [Header("Relatives")]
    [SerializeField] RaceHandler raceHandler = null;
    [SerializeField] GameObject explosionAnim = null;

    // Relative game objects

    // Some variables as a input
    float line1Pos = 9.4f, line2Pos = 8.8f, line3Pos = 8.2f, line4Pos = 7.60f; // Fixed Positions of lines
    float screenEndPos = 4.5f, screenBeginningPos = 0.9f;
    float targetLinePos;
    float closeEnough = 0.01f;

    // VaaT
    float currentYPos; // 
    public bool testModeOn = false;
    int nextTime = 0; // Purpose: A tmp tool for MoveAnim() function
    bool stageOneIsActive = false, stageTwoIsActive = false, stageZeroIsActive = false,
        stageThreeIsActive = false, doItOnce0 = true;
    public bool startNextLine = false, startCurrentLine = true;
    GameObject explosionAnimNew;


    void Start() {
        nextTime = 0;
        stageZeroIsActive = true;
        raceHandler = FindObjectOfType<RaceHandler>();
        currentSpeed = 0;
        explosionAnimNew = explosionAnim; // We are creating a new animation that will be a holder
        startNextLine = false;
    } 

    void Update() {
        if (doItOnce0) {
            raceHandler = FindObjectOfType<RaceHandler>();
            doItOnce0 = false;
        }
        // Start with correct line
        if (startCurrentLine) { GoLine(raceHandler.GetCurrentTurth()); startCurrentLine = false;
            raceHandler.SetStartingSpawner();
        }
        // Start with correct line if you respawned!
        if (startNextLine) { GoLine(raceHandler.GetPreviousTruth()); startNextLine = false; }  
        //UpdatePos(); // Set max and min pos related to current pos
        MoveAnim(); // Gives a suspension animation
        // Stage Behaviours
        if (stageZeroIsActive) {
            //FindObjectOfType<AudioManager>().Play("Car_0");
            //FindObjectOfType<AudioManager>().Stop("Car_1");
            transform.Translate(Time.deltaTime * currentSpeed, 0, 0);
        }
        if (stageOneIsActive) {
            //FindObjectOfType<AudioManager>().Play("Car_1");
            //FindObjectOfType<AudioManager>().Stop("Car_0");
            if (transform.position.x < screenEndPos) {
                transform.Translate(Time.deltaTime * currentSpeed, 0, 0);
            }
            else {
                stageOneIsActive = false;
                raceHandler.SetTransactionStage(2);
            }

        }
        if (stageTwoIsActive) {
            if (transform.position.y > targetLinePos 
                && !isCloseEnough(transform.position.y, targetLinePos)) {
                transform.Translate(0, -(Time.deltaTime * currentSpeed), 0); // Go down
            }
            else if (transform.position.y < targetLinePos
                && !isCloseEnough(transform.position.y, targetLinePos)) {
                transform.Translate(0, Time.deltaTime * currentSpeed, 0); // Go up
            }
            else {
                Vector2 currentPlayerPos = transform.position;  // There 3 lines places
                currentPlayerPos.y = targetLinePos;             // player to the exact position
                transform.position = currentPlayerPos;
                stageTwoIsActive = false;
                raceHandler.SetTransactionStage(3);
            }             
        }
        if (stageThreeIsActive) {
            if (transform.position.x > screenBeginningPos) { // Make player go back to beginning
                transform.Translate(-(Time.deltaTime * currentSpeed), 0, 0); // of the screen
            }
            else { // When it comes to the target place, end the stage.
                stageThreeIsActive = false;
                Invoke("WaitForDeath", 3); // To make player go fast longer. So can crash fast.
            }
        }
    }

    // ##################### PUBLIC METHODS ##################### //
    // ###### Getters and Setters ###### //

    public void SetSpeedLevel(int newLevel) {
        switch (newLevel) {
            case 0:
                currentSpeed = speedOfLevelZero;
                break;
            case 1:
                currentSpeed = speedOfLevelOne;
                break;
            case 2:
                currentSpeed = speedOfLevelTwo;
                break;
            case 3:
                currentSpeed = speedOfLevelThree;
                break;
            default:
                currentSpeed = speedOfLevelZero;
                break;
        }
        if (testModeOn) { currentSpeed *= 2; }
    }
    public void SetStageBehaviour(int newStage) {
        switch (newStage) {
            case 0: // No action, Stage 0
                SetSpeedLevel(0);
                stageZeroIsActive = true;
                break;
            case 1: // Stage 1, go to right!
                SetSpeedLevel(1);
                SetNewTargetLine();
                stageZeroIsActive = stageThreeIsActive = false; // I did this from here because it starts auto.
                stageOneIsActive = true;
                break;
            case 2: // Stage 2, go to the new line!
                SetSpeedLevel(2);
                stageTwoIsActive = true;
                break;
            case 3:
                SetSpeedLevel(3);
                stageThreeIsActive = true;
                break;
            default:
                SetSpeedLevel(0);
                break;
        }
    }

    // ##################### PRIVATE METHODS ##################### //

    private void MoveAnim() {
        nextTime++;
        if (nextTime < NumberOfFrame) {
            currentYPos = transform.position.y + changeAmountPerFrame;
            transform.position = new UnityEngine.Vector2(transform.position.x, currentYPos);
        }
        if (nextTime >= NumberOfFrame) {
            currentYPos = transform.position.y - changeAmountPerFrame;
            transform.position = new UnityEngine.Vector2(transform.position.x, currentYPos);
        }
        if (nextTime > NumberOfFrame * 2 - 3) {
            nextTime = 0;
        }
    }
    private void SetNewTargetLine() {
        int selectedButtonNumber = raceHandler.GetSelectedButtonNumber();
        switch (selectedButtonNumber) {
            case 0:
                targetLinePos = line1Pos;
                GetComponentInChildren<SpriteRenderer>().sortingOrder = 2;
                break;
            case 1:
                targetLinePos = line2Pos;
                GetComponentInChildren<SpriteRenderer>().sortingOrder = 3;
                break;
            case 2:
                targetLinePos = line3Pos;
                GetComponentInChildren<SpriteRenderer>().sortingOrder = 4;
                break;
            case 3:
                targetLinePos = line4Pos;
                GetComponentInChildren<SpriteRenderer>().sortingOrder = 5;
                break;
        }
    }
    private bool isCloseEnough(float firstPos, float secondPos) {
        if (Math.Abs(firstPos - secondPos) < closeEnough)
            return true;
        else
            return false;
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        this.GetComponentInChildren<SpriteRenderer>().enabled = false;
        this.GetComponent<BoxCollider2D>().enabled = false;
        explosionAnimNew = (GameObject)Instantiate(explosionAnim, transform.position, transform.rotation);
        // If we destroy the gameobject directly, there would be error related to the Player Object 
        Destroy(explosionAnimNew, 0.8f);
        FindObjectOfType<AudioManager>().Play("Explosion");
        raceHandler.AccidentOccured(); 
    }
    private void GoLine(int newLine) {
        switch (newLine) {
            case 0:
                transform.position = new UnityEngine.Vector2(screenBeginningPos, line1Pos);
                GetComponentInChildren<SpriteRenderer>().sortingOrder = 2;
                break;
            case 1:
                transform.position = new UnityEngine.Vector2(screenBeginningPos, line2Pos);
                GetComponentInChildren<SpriteRenderer>().sortingOrder = 3;
                break;
            case 2:
                transform.position = new UnityEngine.Vector2(screenBeginningPos, line3Pos);
                GetComponentInChildren<SpriteRenderer>().sortingOrder = 4;
                break;
            case 3:
                transform.position = new UnityEngine.Vector2(screenBeginningPos, line4Pos);
                GetComponentInChildren<SpriteRenderer>().sortingOrder = 5;
                break;

        }
    }
    private void WaitForDeath() {
        if (!stageOneIsActive)
            raceHandler.SetTransactionStage(0);
    }
}
