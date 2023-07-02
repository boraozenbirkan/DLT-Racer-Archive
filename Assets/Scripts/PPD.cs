using UnityEngine;

public class PPD : MonoBehaviour {
    [SerializeField] QuestionHolder[] prefabs = null;
    [SerializeField] string[] prefabsData;
    // Start is called before the first frame update
    void Start() {
        prefabsData = new string[prefabs.Length];
    }

    // Update is called once per frame
    void Update() {
        for (int i = 0; i < prefabs.Length; i++) {
            prefabsData[i] = prefabs[i].questionCode + ": " + PlayerPrefs.GetInt(prefabs[i].questionCode);
        }
    }
}
