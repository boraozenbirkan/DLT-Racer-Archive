﻿using UnityEngine;

public class RoadCleaner : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D collider) {
        Destroy(collider.gameObject);
    }
}
