using UnityEngine;
using UnityEngine.UI;

public class FuelBar : MonoBehaviour {

    public Slider slider;
    float timeLeft;
    bool timeIn = true, loadFuel = false;

    // Start is called before the first frame update
    void Start() {
        timeLeft = slider.maxValue;
    }

    // Update is called once per frame
    void Update() {
        if (timeIn) {
            timeLeft -= Time.deltaTime;
            slider.value = timeLeft;
            if (timeLeft <= 0) {
                FindObjectOfType<RaceHandler>().TimeOut();
                timeIn = false;
            }
        }
        if (loadFuel) {
            timeIn = false;
            if (slider.value < slider.maxValue) {
                timeLeft++;
                slider.value = timeLeft;
            }
            else {
                timeIn = true;
                loadFuel = false;
            }
        }
    }
    public void FuelUp() {
        loadFuel = true;
    }
}
