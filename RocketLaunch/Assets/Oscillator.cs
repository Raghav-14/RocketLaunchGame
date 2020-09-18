using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movmentVector = new Vector3(10f, 10f, 10f);
    [SerializeField] float periodForCycle = 2f;

    float movmentFactor; //0 is not move , 1 is full move
    Vector3 startingPosition;
    const float tau = 2 * Mathf.PI;

    void Start()
    {
        startingPosition = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(periodForCycle <= Mathf.Epsilon) { return; } // period is never be zero
        float cycle = Time.time / periodForCycle; //It return cycle as per frame rate.
  
        float rawSinWave = Mathf.Sin(tau * cycle); //Caluclate sin wave between -1 to +1
                                                   //but we want 0 to 1 .....for this we add below logic
        movmentFactor = rawSinWave / 2f + 0.5f;
        
        //code for how much obstacles moves between range 0 and 1
        Vector3 offset = movmentFactor * movmentVector;
        this.transform.position = startingPosition + offset;
    }
}
