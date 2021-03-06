using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Lightwall : IToggle
{
    [SerializeField]
    private float m_ActualLength = 0.0f;

    [SerializeField]
    public float m_TargetLength = 0.0f;

    [SerializeField]
    public float m_WallSpeed = 10.0f;

    public bool m_Enable = true;

    public bool m_Inverted = true;

    private float m_Height = 3.0f;

    private Transform m_MeshTransform = null;

    [SerializeField]
    private LayerMask m_LayerMask = 0;

    private bool m_Hit = false;

    private RaycastHit m_HitInfo;

    private LightSensor m_CurrentLightSensor = null;

    private void Start()
    {
        m_MeshTransform = transform.Find("Mesh");
        Debug.Assert(m_MeshTransform != null, "unable to find lightwall mesh", this);

        // Collide with default non-special game objects
        m_LayerMask = LayerMask.GetMask("Default", "Lightwall");
        // Collide with lightwalls if we are ourselves a lightwall
        if (gameObject.layer == LayerMask.NameToLayer("Lightweight"))
        {
            m_LayerMask |= LayerMask.GetMask("Lightweight");
        }
    }

    private void FixedUpdate()
    {
        if (m_Enable)
        {
            HitTest();
            m_TargetLength = m_HitInfo.distance;
            if (m_Hit)
            {
                
                LightSensor sensor = m_HitInfo.collider.GetComponent<LightSensor>();
                if (sensor)
                {
                    m_CurrentLightSensor = sensor;
                    sensor.Activate();
                }
                else
                {
                    if (m_CurrentLightSensor)
                    {
                        m_CurrentLightSensor.Deactivate();
                        m_CurrentLightSensor = null;
                    }
                }
            }
        }
        else
        {
            m_ActualLength = 0.0f;
            m_TargetLength = 0.0f;
        }
        m_ActualLength = Mathf.Lerp(m_ActualLength, m_TargetLength, Time.fixedDeltaTime * m_WallSpeed);
        m_ActualLength = Mathf.Clamp(m_ActualLength, 0.0f, m_TargetLength);
        m_MeshTransform.localPosition = new Vector3(0, 0, m_ActualLength / 2.0f);
        m_MeshTransform.localScale = new Vector3(1, m_Height, m_ActualLength);
    }

    private void OnDrawGizmos()
    {
        HitTest();

        // Draw ray of current lightwall length
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * m_MeshTransform.localScale.z);

        // Draw sphere at current lightwall length
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position + transform.forward * m_MeshTransform.localScale.z, 0.05f);

        if (m_Hit)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(m_HitInfo.point, 0.05f);
        }
    }

    private void HitTest()
    {
        Ray ray = new Ray(transform.position + transform.forward * 0.01f - (transform.up * 0.75f), transform.forward);
        m_Hit = Physics.Raycast(ray, out m_HitInfo, 1000.0f, m_LayerMask);
    }

    override public void Enable()
    {
        m_Enable = true;
        m_ActualLength = 0.0f;
        m_TargetLength = 0.0f;
    }

    override public void Disable()
    {
        m_Enable = false;
        m_ActualLength = 0.0f;
        m_TargetLength = 0.0f;
    }

    private void OnDisable()
    {
        Disable();
        if (m_CurrentLightSensor)
        {
            m_CurrentLightSensor.Deactivate();
            m_CurrentLightSensor = null;
        }
    }

    override public void Toggle() => m_Enable = !m_Enable;
}