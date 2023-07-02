using System.Collections;
using UnityEngine;

public class VehicleSpawner : MonoBehaviour{
    [Header("Config Params")]
    [SerializeField] [Range(2, 5)] int sortingOrder = 0;
    [SerializeField] public bool spawn = true; 
    public bool testModeOn = false;
    [SerializeField] bool HTPModeOn = false;
    

    [Header("Inputs")]
    [SerializeField] GameVehicle[] vehicles = null;

    // Relatives
    RaceHandler raceHandler;

    // VaaT
    public int currentStageLevel;
    float floatSpawnTime, fstMin = 1.2f, fstMax = 5.5f;
    bool coroutineStart;

    void Start() {
        floatSpawnTime = Random.Range(fstMin, fstMax);
        coroutineStart = true;
        if (HTPModeOn)
            raceHandler = null;
        else
            raceHandler = FindObjectOfType<RaceHandler>();
        SetSpawn(true);
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

    //  --------------------------------------------------  //
    //              Getters And Setters                     //
    //  --------------------------------------------------  //

    public void SetSpawn(bool newStatus) {
        // If new status be true and the correct true option is NOT equal to spawner's order
        //Debug.Log("New Status: " + newStatus + "    Current Truth: " +
        //    raceHandler.GetCurrentTurth() + "   Next Trurth: " + raceHandler.GetNewTurth() +
        //    "    Spawner's Order: " + (sortingOrder - 2)
        //    + "  Do It Once: " + doItOnce);
        if (!HTPModeOn) {
            if (newStatus && raceHandler.GetCurrentTurth() != sortingOrder - 2) {
                spawn = true;
            }
            else { // otherwise stop spawning
                spawn = false;
            }
        }
        else
            spawn = true;
    }
    public void SetStageBehaviour(int newStage) {
        currentStageLevel = newStage;
        if (newStage == 3) {
            SetSpawn(true);
        }
        else if (newStage == 0) {
            SetSpeedLevel(newStage);
            return;
        }
        else {
            SetSpawn(false);
        }
        if (testModeOn) {
            SetSpawn(false);
        }
        SetSpeedLevel(newStage);
    }
    public void SetSpeedLevel(int newLevel) {
        switch (newLevel) {
            case 0:
                fstMin = 1.2f; fstMax = 5.5f;
                break;
            /*case 1:
                fstMin = .5f; fstMax = 3f;
                break;                          Spawn is closed
            case 2:                             For stage 1 and 2
                fstMin = 4f; fstMax = 8f;
                break;*/
            case 3:
                Spawn();
                fstMin = 0.7f; fstMax = 1.5f;
                break;
            default:
                fstMin = 1f; fstMax = 6f;
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

    private void Spawn() { 
        if (vehicles == null) { return; }
        if (!spawn) { return; }
        GameVehicle newVehicle = vehicles[Random.Range(0, vehicles.Length)];
        newVehicle = Instantiate(newVehicle, transform.position, transform.rotation) as GameVehicle;
        newVehicle.transform.parent = transform;
        newVehicle.GetComponentInChildren<SpriteRenderer>().sortingOrder = sortingOrder;
        newVehicle.SetSpeedLevel(currentStageLevel);
    }

    private IEnumerator SpawnCheck() {
        coroutineStart = false; // To avoid multiple coroutines because of Update function
        yield return new WaitForSeconds(floatSpawnTime);
        if (spawn) {    // But if spawn is still true, then spawn.
            Spawn();    // This avoids unwanted late-spawns. 
            floatSpawnTime = Random.Range(fstMin, fstMax);
        }
        coroutineStart = true; // Enabling coroutine again.

    }


}
