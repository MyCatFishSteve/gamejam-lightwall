using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    private Rigidbody m_Rigidbody;

    [SerializeField]
    private float m_Speed = 20.0f;

    [SerializeField]
    private float m_Threshold = 0.5f;

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Rigidbody.solverVelocityIterations = 160;
        m_Rigidbody.velocity = transform.forward * m_Speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Rigidbody.velocity.magnitude < 0.5f)
        {
            Destroy(gameObject);
        }
        m_Rigidbody.velocity = transform.forward * m_Speed;
    }
}
