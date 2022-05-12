using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{

    public bool passing = false;
    public PlayerManager manager;
    Rigidbody m_Rigidbody;

    private void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        m_Rigidbody.isKinematic = !passing;
        if (passing)
        {
            print("sdd");
            m_Rigidbody.velocity = transform.forward * manager.ballSpeed;
        }

        
    }
}
