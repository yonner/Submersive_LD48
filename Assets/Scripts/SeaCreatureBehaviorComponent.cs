using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaCreatureBehaviorComponent : MonoBehaviour
{

    Vector3 _wayPoint;

    float acceleration = 2.4f;//5f;

    Rigidbody m_Rigidbody;

    public int Depth;

    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 Waypoint 
    {
        set
        {
            _wayPoint = value;

            transform.LookAt(_wayPoint);
        }
        get
        {
            return _wayPoint;
        }
    }

    private void FixedUpdate()
    {
        //m_Rigidbody.velocity = -transform.forward * acceleration;
        float step = acceleration * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, _wayPoint, step);
    }
}
