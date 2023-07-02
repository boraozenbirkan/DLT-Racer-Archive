using UnityEngine;

public class GameTree : MonoBehaviour {

    [SerializeField] float currentSpeed = 3f;
    public bool testModeOn = false;
    float speedOfLevelZero = 3f;
    float speedOfLevelOne = 6f;
    float speedOfLevelTwo = 6f;
    float speedOfLevelThree = 6f;

    // Start is called before the first frame update
    void Start() {
    }

    // Update is called once per frame
    void Update() {
        transform.Translate(Vector2.left * currentSpeed * Time.deltaTime);
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
