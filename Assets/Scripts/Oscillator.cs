using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour
{

    //! Holds the starting position.
    Vector3 startingPosition = Vector3.zero;

    //! Used to hold the movement data.
    [SerializeField]
    Vector3 movementVector = Vector3.zero;

    //! movement speed.
    float movementFactor = 0.0f;

    //! Holds the value to calculate cycles.
    [SerializeField]
    float period = 2f;
    
    void Start()
    {
        startingPosition = transform.position;

    }

    
    void Update()
    {
        MoveObstacle();
    }

    //! Handle the movement of the obstacle.
    //! We used sin function to make it go back and forth.
    void MoveObstacle()
    {
        if (period <= Mathf.Epsilon)
        {
            return;
        }
        // Continually going over time.
        float cycles = Time.time / period;

        // Constant value of 6.283
        const float tau = Mathf.PI * 2;

        // Going from -1 to 1
        float rawSinWave = Mathf.Sin(cycles * tau);

        // Recalculate to go from 0 to 1 instead of -1 to 1.
        movementFactor = (rawSinWave + 1f) / 2f;

        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPosition + offset;
    }
}
