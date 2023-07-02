using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HTPPlayer : MonoBehaviour{

    //[SerializeField] Player player;
    [SerializeField] VehicleSpawner[] spawners = null;
    [SerializeField] GameObject hand = null;
    [SerializeField] RoadScrolling road = null;
    [SerializeField] GameObject startInfo = null;
    [SerializeField] GameObject question0 = null;
    [SerializeField] Button[] buttons0 = null;
    [SerializeField] GameObject question1 = null;
    [SerializeField] Button[] buttons1 = null;
    [SerializeField] FuelBar fuel = null;
    [SerializeField] GameObject explosionAnim = null;
    [SerializeField] GameObject crashInfo = null;
    [SerializeField] Image endScreen = null;
    [SerializeField] Image wheel = null;

    AudioManager audioManager;
    GameObject explosionAnimNew;

    bool stage0 = false, stage1 = false, stage2 = false, stage3 = false, stage4 = false, swipe = false,
        stage5 = false, stage6 = false, stage7 = false, stage8 = false, stage9 = false;
    bool doItOnce0 = false;
    float transparency = 0;
    Vector2 vector = new Vector2(0, 0);
    Vector2 crashVector = new Vector2(0, 0);
    Vector2 crashVectorSaved = new Vector2(0, 0);

    void Start()    {
        //0 Start traffic
        //0 Drag the hand over the correct option
        //1 When hand comes the option, make it green and drag player to the end and stop traffic
        //2 When it reaches to the end drag it to he line
        //3 Then make it go back.
        //4 When it reaches to the back, start the wrong scene.
        //5 - 6 - 7 - 8 - 9
        audioManager = FindObjectOfType<AudioManager>();
        audioManager.SetVolume("Background Music", 0.05f);
        audioManager.Play("Outdoor");
        StartCoroutine(StartInfo());
        doItOnce0 = true;
        explosionAnimNew = explosionAnim;
        vector.x = hand.transform.localPosition.x;
        vector.y = hand.transform.localPosition.y;
    }

    void Update()    {
        if (stage0) {
            if (hand.transform.localPosition.y < -450) {
                vector.y++;
                hand.transform.localPosition = vector;
            }
            else{
                StartCoroutine(Blink());
                stage0 = false;
                stage1 = true;
            }
        }
        if (stage1) {
            if (doItOnce0) {
                // Stop traffic
                foreach (VehicleSpawner spawner in spawners) {
                    spawner.spawn = false;
                }
                // Find and speed up all vehicles
                foreach (GameVehicle currentVehicles in FindObjectsOfType<GameVehicle>()) {
                    currentVehicles.SetSpeedLevel(1);
                }
                // make it green
                var colors = buttons0[1].colors;
                colors.normalColor = Color.green;
                buttons0[1].colors = colors;
                // Set the new speed
                road.SetSpeedLevel(1);
                doItOnce0 = false;
            }
            if (this.transform.position.x < 4.75)
                this.transform.Translate(Time.deltaTime * 2, 0, 0);
            else {
                stage1 = false;
                stage2 = true;
                doItOnce0 = true;
            }
        }
        if (stage2) {
            if (this.transform.position.y < 8.89f)
                this.transform.Translate(0, Time.deltaTime, 0);
            else {
                stage2 = false;
                stage3 = true;
            }
        }
        if (stage3) {
            if (doItOnce0) {
                // Start traffic
                foreach (VehicleSpawner spawner in spawners) {
                    spawner.currentStageLevel = 3;
                    spawner.spawn = true;
                    spawner.SetSpeedLevel(0);
                }
                spawners[1].spawn = false; // But not for the correct line
                swipe = true;
                doItOnce0 = false;
            }
            if (this.transform.position.x > 0.9f)
                this.transform.Translate(-Time.deltaTime * 2, 0, 0);
            else {
                stage3 = false;
                stage4 = true;
                doItOnce0 = true;
            }
        }
        if (swipe) {
            if (question1.transform.localPosition.x > 0) {
                vector = question0.transform.localPosition;
                vector.x = vector.x - 10;
                question0.transform.localPosition = vector;
                vector = question1.transform.localPosition;
                vector.x = vector.x - 10;
                question1.transform.localPosition = vector;
            }
            else {
                vector = hand.transform.localPosition;
                swipe = false;
            }
        }
        if (stage4) {
            if (doItOnce0) {
                crashInfo.SetActive(true);
                // Set the speed
                road.SetSpeedLevel(0);
                foreach (VehicleSpawner spawner in spawners) {
                    spawner.currentStageLevel = 0;
                    spawner.SetSpeedLevel(0);
                }
                fuel.FuelUp();
                doItOnce0 = false;
            }
            if (hand.transform.localPosition.y < -330 && !swipe) {
                vector.y++;
                hand.transform.localPosition = vector;
            }
            else if (hand.transform.localPosition.y > -350){
                StartCoroutine(Blink());
                stage4 = false;
                stage5 = true;
                doItOnce0 = true;
            }
        }
        if (stage5) {
            if (doItOnce0) {
                // Stop traffic
                foreach (VehicleSpawner spawner in spawners) {
                    spawner.spawn = false;
                }
                // Find and speed up all vehicles
                foreach (GameVehicle currentVehicles in FindObjectsOfType<GameVehicle>()) {
                    currentVehicles.SetSpeedLevel(1);
                }
                // Color buttons
                var colors = buttons0[0].colors;
                colors.normalColor = Color.red;
                buttons1[0].colors = colors;
                colors = buttons0[3].colors;
                colors.normalColor = Color.green;
                buttons1[3].colors = colors;
                // Set the new speed
                road.SetSpeedLevel(1);
                doItOnce0 = false;
            }
            if (this.transform.position.x < 4.75)
                this.transform.Translate(Time.deltaTime * 2, 0, 0);
            else {
                stage5 = false;
                stage6 = true;
                doItOnce0 = true;
            }
        }
        if (stage6) {
            if (this.transform.position.y < 9.45f)
                this.transform.Translate(0, Time.deltaTime, 0);
            else {
                stage6 = false;
                stage7 = true;
            }
        }
        if (stage7) {
            if (doItOnce0) {
                crashInfo.SetActive(true);
                // Start traffic
                foreach (VehicleSpawner spawner in spawners) {
                    spawner.currentStageLevel = 3;
                    spawner.spawn = true;
                    spawner.SetSpeedLevel(0);
                }
                spawners[3].spawn = false; // But not for the correct line
            }
            if (this.transform.position.x > 0.9f)
                this.transform.Translate(-Time.deltaTime * 2, 0, 0);
            else {
                StartCoroutine(Crash());
                stage7 = false;
                doItOnce0 = true;
            }
        }
        if (stage8) {
            if (transparency < 1) {
                transparency += 0.005f;
                var color1 = endScreen.color;
                color1.a = transparency;
                endScreen.color = color1;
            }
            else {
                stage8 = false;
                stage9 = true;
            }
        }
        if (stage9) {
            audioManager.SetVolume("Background Music", 0.2f);
            audioManager.Stop("Outdoor");
            SceneManager.LoadScene("Menus");
        }
    }

    public void ButtonSelected(Button selectedButton) {
        string selectedText = selectedButton.GetComponentInChildren<Text>().text;
        if (selectedText == "You'll allow the driver to see you in their mirrors") {

        }
    }
    private IEnumerator Blink() {
        hand.transform.GetChild(0).gameObject.SetActive(true);
        float time = 0.2f;
        hand.transform.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(time);
        hand.transform.GetChild(0).gameObject.SetActive(false);
        yield return new WaitForSeconds(time);
        hand.transform.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(time);
        hand.transform.GetChild(0).gameObject.SetActive(false);
    }
    private IEnumerator StartInfo() {
        yield return new WaitForSeconds(0.1f);
        spawners[3].spawn = false;
        float time = 1f;
        for (int i = 0; i < 3; i++) {
            yield return new WaitForSeconds(time);
            startInfo.SetActive(false);
            yield return new WaitForSeconds(time/2);
            startInfo.SetActive(true);
        }
        yield return new WaitForSeconds(2);
        startInfo.GetComponentInParent<Canvas>().enabled = false;
        stage0 = true;
    }
    private IEnumerator Crash() {
        yield return new WaitForSeconds(4);
        stage8 = true;
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        this.GetComponentInChildren<SpriteRenderer>().enabled = false;
        this.GetComponent<BoxCollider2D>().enabled = false;
        explosionAnimNew = (GameObject)Instantiate(explosionAnim, transform.position, transform.rotation);
        // If we destroy the gameobject directly, there would be error related to the Player Object 
        Destroy(explosionAnimNew, 0.8f);
        wheel.enabled = false;
        FindObjectOfType<AudioManager>().Play("Explosion");
    }
}
