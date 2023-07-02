using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeOutAnimation : MonoBehaviour{

    [SerializeField] Slider slider = null;
    [SerializeField] TMP_Text fuelText = null;
    bool startAnimation = false;
    int animationSmoothness = 2, currentFrame = 0;

    // Start is called before the first frame update
    void Awake() {
        slider.value = slider.maxValue;
        Invoke("StartAnimation", 1);
    }

    // Update is called once per frame
    void Update() {
        if (startAnimation)
            if (++currentFrame > animationSmoothness) {
                slider.value = slider.value - 1;
                fuelText.text = slider.value + " Seconds";
                currentFrame = 0;
            }
    }

    void StartAnimation() {
        startAnimation = true;
    }
}
