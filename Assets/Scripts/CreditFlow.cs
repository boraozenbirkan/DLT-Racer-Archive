using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditFlow : MonoBehaviour{

    [SerializeField] GameObject creditText = null;
    Vector2 vector;
    float yPos;

    int frame = 0;
    // Start is called before the first frame update
    void Start()    {
        vector.x = creditText.transform.position.x;
        yPos = creditText.transform.position.y;
    }

    // Update is called once per frame
    void Update()    {
        if (++frame > 3) {
            vector.y = yPos;
            creditText.transform.position = vector;
            frame = 0; yPos += 5;
        }
        if (creditText.transform.localPosition.y > 0) {
            SceneManager.LoadScene("Menus");
        }
    }
}
