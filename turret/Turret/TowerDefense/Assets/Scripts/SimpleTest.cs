using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTest : MonoBehaviour {
    public Vector3 dir;
    public float speed = 5.0f;
    public float time;
    private float curTime;
	// Update is called once per frame
	void Update () {
        curTime -= Time.deltaTime;
        if (curTime < 0)
        {
            curTime = time;
            dir = -dir;
        }
        dir.Normalize();
        transform.Translate(dir*speed*Time.deltaTime);
	}
}
