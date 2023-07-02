using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameWheels : MonoBehaviour{

    [SerializeField] Image[] wheels = null;
    Image[] reverseWheels = new Image[3];
    int livesLeft;

    void Start()
    {
        foreach (Image wheel in wheels) {
            wheel.enabled = true;
        }
        StartCoroutine(WheelLost());

    }
    private IEnumerator WheelLost() {
        if (SceneManager.GetActiveScene().name == "Game Over") {
            yield return new WaitForSeconds(0.5f);
            foreach (Image wheel in wheels) {
                yield return new WaitForSeconds(0.7f);
                wheel.enabled = false;
            }
        }
        else {
            yield return new WaitForSeconds(0.5f);
            livesLeft = FindObjectOfType<StatusHandler>().livesLeft;
            int k = 2;
            for (int j = 0; j < 3; j++) {
                reverseWheels[j] = wheels[k--];
            }
            for (int i = 2; i > livesLeft; i--) {
                yield return new WaitForSeconds(0.7f);
                reverseWheels[i].enabled = false;
            }
            // Insert Cheering Sound
        }
            
    }
}
