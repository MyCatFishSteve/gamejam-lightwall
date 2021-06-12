using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightwall : MonoBehaviour
{
    [SerializeField]
    private float m_ActualLength = 0.0f;

    [SerializeField]
    public float m_TargetLength = 0.0f;

    [SerializeField]
    public float m_WallSpeed = 10.0f;

    private float m_Height = 1.0f;

    private Transform m_MeshTransform = null;

    private bool m_Hit = false;

    private RaycastHit m_HitInfo;

    private void Start()
    {
        m_MeshTransform = transform.Find("Mesh");
        Debug.Assert(m_MeshTransform != null, "unable to find lightwall mesh", this);
    }

    private void FixedUpdate()
    {
        ProcessRay();
        m_ActualLength = Mathf.Lerp(m_ActualLength, m_TargetLength, Time.fixedDeltaTime * m_WallSpeed);
        m_ActualLength = Mathf.Clamp(m_ActualLength, 0.0f, m_TargetLength);
        m_MeshTransform.localPosition = new Vector3(0, 0, m_ActualLength / 2.0f);
        m_MeshTransform.localScale = new Vector3(1, m_Height, m_ActualLength);

    }

    private void OnDrawGizmos()
    {
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

    public void ProcessRay()
    {
        Ray ray = new Ray(transform.position + transform.forward * 0.01f, transform.forward);
        if (Physics.Raycast(ray, out m_HitInfo, 1000.0f))
        {
            m_Hit = true;
            m_TargetLength = Vector3.Distance(transform.position, m_HitInfo.point);
        }
        else
        {
            m_Hit = false;
            m_TargetLength = 1000.0f;
        }
    }
}