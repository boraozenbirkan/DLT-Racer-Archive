using UnityEngine;

public class BackgroundRolling : MonoBehaviour {

    [SerializeField] float currentSpeed = 0.5f;
    Material myMaterial;
    UnityEngine.Vector2 offset;
    public bool testModeOn = false;
    float speedOfLevelZero = 0.75f;
    float speedOfLevelOne = 1.5f;
    float speedOfLevelTwo = 1.5f;
    float speedOfLevelThree = 1.5f;

    public BackgroundRolling instance;

    private void Awake() {
        if (instance == null)
            instance = this;
        else {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start() {
        myMaterial = GetComponent<Renderer>().material;
        offset = new UnityEngine.Vector2(currentSpeed, 0f);
        float xPos = this.transform.position.x, yPos = this.transform.position.y;
        this.transform.position = new Vector3(xPos, yPos, 10);
        
    }

    // Update is called once per frame
    void Update() {
        offset = new UnityEngine.Vector2(currentSpeed, 0f);
        myMaterial.mainTextureOffset += offset * Time.deltaTime;
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
