using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour {
    public float lifetime = 1.0f; // Adjust as needed

    void Start() {
        Destroy(gameObject, lifetime);
    }
}
