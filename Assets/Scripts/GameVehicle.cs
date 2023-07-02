using UnityEngine;

public class GameVehicle : MonoBehaviour {
    // Configuration Parameters
    [Header("Config Params")]
    [SerializeField] float currentSpeed = 1f;
    [SerializeField] float changeAmountPerFrame = 0.001f;
    [SerializeField] float maxChange = 0.05f;
    [SerializeField] int changeIn = 2;  // make a movement in every 2 frame
    public bool testModeOn = false;
    float speedOfLevelZero = 1f;
    float speedOfLevelOne = 4f;
    float speedOfLevelTwo = 4f;
    float speedOfLevelThree = 4f;


    // VaaT (Variables as a Tool)
    bool upMove;
    int nextTime = 0;
    float currentYPos, maxHeight, minHeight;

    // Start is called before the first frame update
    void Start() {
        upMove = true;
        nextTime = 0;
        currentYPos = transform.position.y;
        maxHeight = transform.position.y + maxChange;
        minHeight = transform.position.y - maxChange;
    }

    // Update is called once per frame
    void Update() {
        transform.Translate(Vector2.left * currentSpeed * Time.deltaTime);
        MoveAnim();
    }

    private void MoveAnim() {
        nextTime++;
        if (nextTime == changeIn) {
            if (upMove) {
                currentYPos += changeAmountPerFrame;
                transform.position = new Vector2(transform.position.x, currentYPos);
                if (currentYPos >= maxHeight) {
                    upMove = false;
                }
                nextTime = 0;
                return;
            }
            currentYPos -= changeAmountPerFrame;
            transform.position = new Vector2(transform.position.x, currentYPos);
            if (currentYPos <= minHeight) {
                upMove = true;
            }
            nextTime = 0;
        }
        return;
    }

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
}
