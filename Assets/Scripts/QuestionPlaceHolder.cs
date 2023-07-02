using UnityEngine;

public class QuestionPlaceHolder : MonoBehaviour{

    QuestionHolder[] questions;
    QuestionHolder currentQuestion, nextQuestion;
    int currentNumberOfQuestion;
    int numberOfLastQuestions = 10;
    [SerializeField] RaceHandler raceHandler = null;

    public bool testModeOn = false, playerIsDead = false;
    bool transactionHasBeenActivated = false, scaleDown = false, inLastQuestion = false,
        swipe = false, scaleUp = false, doItOnce = false, doItOnce1 = true, triggered = false,
        scaleUpHasFinished = false;
    public float scaleDownValue = 0.5f, scaleUpValue = 1f, scaleSmoothness = 0.05f;
    public float currentXPos, swipeSmoothness = 50f;
    Vector2 vector2 = new Vector2(0, 0);
    Vector2 nextQuestionPos = new Vector2(2000, 0);

    


    // Start is called before the first frame update
    void Start() {
        // Cleaning variables. So, if player goes one scene to another
        // there won't be a problem.
        questions = null;
        currentNumberOfQuestion = 0;
        questions = raceHandler.GetOrderedQuestions();
        currentQuestion = questions[currentNumberOfQuestion];
        nextQuestion = questions[currentNumberOfQuestion + 1];
        numberOfLastQuestions = questions.Length - 1;
    }

    // Update is called once per frame
    void Update() {
        if (triggered) { SetStageBehaviour(2); triggered = false; } // For Debugging
        /*if (testModeOn) {
            scaleSmoothness = 0.03f;
            swipeSmoothness = 200f;
        }                               This place for TEST MODE
        else {
            scaleSmoothness = 0.01f;
            swipeSmoothness = 20f;
        }*/
        TransactionEvents(); 
    }

    public void SetStageBehaviour(int newStage) {
        if (newStage == 2) {
            currentQuestion.SetInteractable(false);
            nextQuestion.SetInteractable(false);
            // Placing next question to the "next quesion's position"
            if (currentNumberOfQuestion < 9) {
                nextQuestionPos.y = nextQuestion.transform.localPosition.y;
                nextQuestion.transform.localPosition = nextQuestionPos;
            }

            scaleDown = true;
            transactionHasBeenActivated = true;
        }
        if (newStage == 0) {
            if (inLastQuestion && doItOnce1) { // We're using this doItOnce1 in order to prevent
                currentQuestion.SetInteractable(true);  // Second command thing. Actually, I'm not sure.
                doItOnce1 = false;
            }                
        }
    }

    public int GetCurrentNumberOfQuestion() {
        return currentNumberOfQuestion;
    }

    private void TransactionEvents() {
        if (transactionHasBeenActivated && !inLastQuestion) {
            if (scaleDown) {
                scaleUpHasFinished = false;
                if (currentQuestion.transform.localScale.x > scaleDownValue) {
                    vector2.x = vector2.y = currentQuestion.transform.localScale.x - scaleSmoothness;
                    currentQuestion.transform.localScale = vector2;
                    vector2.x = vector2.y = nextQuestion.transform.localScale.x - scaleSmoothness;
                    nextQuestion.transform.localScale = vector2;
                }
                else {
                    scaleDown = false;
                    doItOnce = swipe = true;
                    vector2.x = vector2.y = 0;      // We're cleaning vector2 object
                }
            }
            if (swipe) {
                if (doItOnce) { //This is for taking current position for once.
                    currentXPos = currentQuestion.transform.localPosition.x;
                    doItOnce = false;
                }
                // I'm updating vector2.y for every time for both questions
                vector2.x = currentQuestion.transform.localPosition.x - swipeSmoothness;
                vector2.y = currentQuestion.transform.localPosition.y;
                currentQuestion.transform.localPosition = vector2;

                vector2.x = nextQuestion.transform.localPosition.x - swipeSmoothness;
                vector2.y = nextQuestion.transform.localPosition.y;
                nextQuestion.transform.localPosition = vector2;

                if (currentXPos > currentQuestion.transform.localPosition.x + 2000f) {
                    scaleUp = true;
                    swipe = false;
                    // When program executes this part, it's alreay 1 time more forward. So;
                    vector2.x = currentQuestion.transform.localPosition.x + swipeSmoothness;
                    vector2.y = currentQuestion.transform.localPosition.y;
                    currentQuestion.transform.localPosition = vector2;

                    vector2.x = nextQuestion.transform.localPosition.x + swipeSmoothness;
                    vector2.y = nextQuestion.transform.localPosition.y;
                    nextQuestion.transform.localPosition = vector2;

                    vector2.x = vector2.y = 0;  // We're cleaning vector2 object
                }
            }
            if (scaleUp) {
                if (currentQuestion.transform.localScale.x < scaleUpValue) {
                    // Scale up
                    vector2.x = vector2.y = currentQuestion.transform.localScale.x + scaleSmoothness;
                    currentQuestion.transform.localScale = vector2;
                    vector2.x = vector2.y = nextQuestion.transform.localScale.x + scaleSmoothness;
                    nextQuestion.transform.localScale = vector2;
                }
                else {
                    scaleUp = false; scaleUpHasFinished = true;
                    vector2.x = vector2.y = 0;
                    // switching to the next questions
                    currentNumberOfQuestion++;
                    currentQuestion = questions[currentNumberOfQuestion];
                    if (currentNumberOfQuestion < numberOfLastQuestions) { // If we don't reach the end, take next one
                        nextQuestion = questions[currentNumberOfQuestion + 1];
                    }
                    else {
                        inLastQuestion = true;
                        raceHandler.inLastQuestion = true;
                    }
                    raceHandler.SetQuestionNumberText(currentNumberOfQuestion);
                }
            }
            if (playerIsDead && scaleUpHasFinished && !inLastQuestion) {
                currentQuestion.SetInteractable(true);
                transactionHasBeenActivated = false;
                playerIsDead = false;
                scaleUpHasFinished = false;
            }
        }
    }

}
