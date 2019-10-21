using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningProjectile : MonoBehaviour
{
    float angularSpeed;

    // Start is called before the first frame update
    void Start()
    {
        angularSpeed = 360 + Random.Range(-180, 180);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, Time.deltaTime * angularSpeed);
    }
}
