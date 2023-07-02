using System.Collections;
using UnityEngine;

public class TreeSpawner : MonoBehaviour {

    [Header("Config Params")]
    [SerializeField] float minSpawnTime = .5f;
    [SerializeField] float maxSpawnTime = 3f;
    [SerializeField] bool spawn = true;

    [Header("Inputs")]
    [SerializeField] GameTree[] trees = null;

    // Some variables to use as a tool
    int currentStageLevel;
    float floatSpawnTime, fstMin = .5f, fstMax = 3f;
    public bool testModeOn = false;
    bool coroutineStart;

    void Start() {
        spawn = false;
        SetSpawn(true);
        floatSpawnTime = Random.Range(minSpawnTime, maxSpawnTime);
        coroutineStart = true;
    }
    void Update() {
        if (coroutineStart) { // If we didn't start any coroutine yet.
            StartCoroutine(SpawnCheck());
        }
    }

    //  ------------------------------------------------    //
    //                                                      //
    //                  PUBLIC METHODS                      //
    //                                                      //
    //  ------------------------------------------------    //

    public void SetSpawn(bool newStatus) {
        if (newStatus) {   // If we stop spawning
            spawn = newStatus;
        }
        else {
            spawn = newStatus;
        }
    }
    public void SetStageBehavior(int newStage) {
        //This is where we tell the spawner what is new stage and what it should do
        currentStageLevel = newStage;
        if (testModeOn) {
            SetSpawn(false);
        }
        else {
            SetSpawn(true);
        }
        SetSpeedLevel(newStage);
    }
    public void SetSpeedLevel(int newLevel) {
        switch (newLevel) {
            case 0:
                fstMin = .5f; fstMax = 3f;
                StartCoroutine(SpawnCheck());
                break;
            case 1:
                fstMin = .3f; fstMax = .8f;
                break;
            case 2:
                fstMin = .3f; fstMax = .8f;
                break;
            case 3:
                fstMin = .3f; fstMax = .8f;
                break;
            default:
                fstMin = .5f; fstMax = 3f;
                break;
        }
        if (testModeOn) {
            fstMin /= 2; fstMax /= 2;
        }
    }

    //  ------------------------------------------------    //
    //                                                      //
    //                  PRIVATE METHODS                     //
    //                                                      //
    //  ------------------------------------------------    //

    private void SpawnTree() {
        if (trees == null) { return; }
        GameTree newTree = trees[Random.Range(0, trees.Length)];
        newTree = Instantiate(newTree, transform.position, transform.rotation) as GameTree;
        newTree.transform.parent = transform;
        newTree.SetSpeedLevel(currentStageLevel);
    }
    private IEnumerator SpawnCheck() {
        coroutineStart = false; // To avoid multiple coroutines because of Update function
        yield return new WaitForSeconds(floatSpawnTime);
        if (spawn) {        // But if spawn is still true, then spawn.
            SpawnTree();    // This avoids unwanted late-spawns. 
            floatSpawnTime = Random.Range(fstMin, fstMax);
        }
        coroutineStart = true;  // Enabling coroutine again.

    }


}
