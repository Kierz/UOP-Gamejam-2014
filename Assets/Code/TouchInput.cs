using UnityEngine;
using System.Collections;

public class TouchInput : MonoBehaviour {

	void Update () {
        // seperate and send all touches to an update touch function
        if (Input.touches.Length > 0) {
            for (int i = 0; i < Input.touches.Length; i++) {
                UpdateTouch(Input.touches[i]);
            }
        }
	}

    void UpdateTouch(Touch touch) {
        
    }
}
