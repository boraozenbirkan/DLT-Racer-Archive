using UnityEngine;

public class ResetData : MonoBehaviour{
    [SerializeField] GameObject decisionInput = null;
    [SerializeField] GameObject deletedText = null;

    public void ShowDecisionInput() {
        decisionInput.SetActive(true);
    }
    public void NoAnswer() {
        decisionInput.SetActive(false);
    }
    public void ResetAllData() {
        PlayerPrefs.DeleteAll();
        /*foreach (SubClass theClass in availableClasses) {
            for (int i = theClass.numberOfScenes; i >= 0; i--) { }
        }*/
        decisionInput.SetActive(false);
        deletedText.SetActive(true);
        Invoke("CloseAll", 2);
    }
    public void CloseAll() {
        deletedText.SetActive(false);
    }

}
