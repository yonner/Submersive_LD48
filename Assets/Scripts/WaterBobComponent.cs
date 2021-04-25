using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBobComponent : MonoBehaviour
{
    public States state = States.onSurface;
    bool bobDown = true;
    public float BobDiveSpeed = .002f;
    float currentBobTimer;
    public float BobTimer = .5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (state == States.onSurface)
        {
            if (bobDown)
            {
                Vector2 myVectorD = Vector2.zero;
                myVectorD.y -= BobDiveSpeed;

                transform.Translate(myVectorD);

                currentBobTimer -= Time.deltaTime;

                if (currentBobTimer < 0)
                {
                    currentBobTimer = BobTimer;
                    bobDown = false;
                }
            }

            if (!bobDown)
            {
                Vector2 myVectorU = Vector2.zero;
                myVectorU.y += BobDiveSpeed;

                transform.Translate(myVectorU);

                currentBobTimer -= Time.deltaTime;

                if (currentBobTimer < 0)
                {
                    currentBobTimer = BobTimer;
                    bobDown = true;
                }
            }
        }
    }
}
