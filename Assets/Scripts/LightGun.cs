using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode]
public class LightGun : MonoBehaviour
{
    /// <summary>
    /// Game object tag to spawn lightwalls on
    /// </summary>
    [SerializeField]
    private string m_PortalWallTag = string.Empty;

    /// <summary>
    /// Collision state from hit test
    /// </summary>
    [SerializeField]
    private bool m_Hit = false;

    /// <summary>
    /// Collision information from hit test
    /// </summary>
    private RaycastHit m_HitInfo;

    /// <summary>
    /// Maximum distance to perform a hit test
    /// </summary>
    [SerializeField]
    private float m_HitMaxDistance = 200.0f;

    /// <summary>
    /// 
    /// </summary>
    [SerializeField]
    private GameObject m_LightWallPrefab = null;

    /// <summary>
    /// Active object owned by the light gun
    /// </summary>
    private GameObject m_LightwallGameObject = null;

    /// <summary>
    /// 
    /// </summary>
    private Lightwall m_Lightwall = null;

    /// <summary>
    /// Run a hit test by casting a ray in-front of the player and store the results
    /// </summary>
    private void HitTest()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        m_Hit = Physics.Raycast(ray, out m_HitInfo, m_HitMaxDistance);
    }

    public void Clear()
    {
        m_Lightwall.Disable();
        m_LightwallGameObject.SetActive(false);
    }

    public void Fire()
    {
        HitTest();
        if (m_HitInfo.collider.CompareTag(m_PortalWallTag))
        {
            m_LightwallGameObject.transform.position = m_HitInfo.point;
            m_LightwallGameObject.SetActive(true);
            m_Lightwall.Enable();
            m_LightwallGameObject.transform.rotation = Quaternion.FromToRotation(Vector3.forward, m_HitInfo.normal);
        }
    }

    private void Awake()
    {
        Debug.Assert(m_LightWallPrefab != null, "lightwall prefab not set for light gun", this);
        if (m_LightwallGameObject == null)
        {
            m_LightwallGameObject = Instantiate(m_LightWallPrefab);
            m_Lightwall = m_LightwallGameObject.GetComponentInChildren<Lightwall>();
            m_LightwallGameObject.SetActive(false);
        }
    }

    private void OnDrawGizmos()
    {
        HitTest();
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, transform.forward * m_HitMaxDistance);
        if (m_Hit)
        {
            Gizmos.DrawSphere(m_HitInfo.point, 0.1f);
        }
    }
}
