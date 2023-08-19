using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour
{
    Vector3 startingPosition;
    [SerializeField] Vector3 movementVector;
    [SerializeField] float movementFactor;
    [SerializeField] float period = 2f;

    void Start()
    {
        startingPosition = transform.position;
    }

    void Update()
    {
        if (period <= Mathf.Epsilon) { return; }

        float cycles = Time.time / period; // grows over time

        const float tau = 2 * Mathf.PI; // tau value - constant value

        float sinValue = Mathf.Sin(tau * cycles); // passing the radians as input to get a value between -1 and +1
        movementFactor = (sinValue + 1f) / 2; // converting to 0 to 1

        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPosition + offset;
    }
}
