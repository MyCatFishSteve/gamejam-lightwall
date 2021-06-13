using UnityEngine;
using System.Collections.Generic;

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
    /// Game object representing the point where the light gun will fire
    /// </summary>
    private GameObject m_Reticle = null;

    /// <summary>
    /// The renderer component of the reticle
    /// </summary>
    private Renderer m_ReticleRenderer = null;

    /// <summary>
    /// The material used to indicate a valid hit contact
    /// </summary>
    [SerializeField]
    private Material m_ValidHitMaterial = null;

    /// <summary>
    /// The material used to indicate an invalid hit contact
    /// </summary>
    [SerializeField]
    private Material m_InvalidHitMaterial = null;

    /// <summary>
    /// This line renderer will draw a laser between the player and the target reticle
    /// </summary>
    [SerializeField]
    private LineRenderer m_LineRenderer;

    /// <summary>
    /// This is the origin point of the laser sight.
    /// </summary>
    [SerializeField]
    private Transform m_LineOrigin;

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
        if (m_Hit && m_HitInfo.collider.CompareTag(m_PortalWallTag))
        {
            // Recreate the lightwall game object so we have the
            // pusher prefab restored.
            Destroy(m_LightwallGameObject);
            m_LightwallGameObject = Instantiate(m_LightWallPrefab);

            m_LightwallGameObject.transform.position = m_HitInfo.point;
            m_LightwallGameObject.SetActive(true);
            m_Lightwall.Enable();
            m_LightwallGameObject.transform.rotation = Quaternion.FromToRotation(Vector3.forward, m_HitInfo.normal);
        }
    }

    private void Start()
    {
        Debug.Assert(m_ValidHitMaterial != null, "no valid hit material assigned", this);
        Debug.Assert(m_InvalidHitMaterial != null, "no invalid hit material assigned", this);

        Debug.Assert(m_LightWallPrefab != null, "lightwall prefab not set for light gun", this);
        if (m_LightwallGameObject == null)
        {
            m_LightwallGameObject = Instantiate(m_LightWallPrefab);
            m_Lightwall = m_LightwallGameObject.GetComponentInChildren<Lightwall>();
            m_LightwallGameObject.SetActive(false);
        }

        Debug.Assert(m_Reticle == null, "reticle should not be initialised", this);
        m_Reticle = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        m_Reticle.name = "Reticle";
        m_Reticle.transform.localScale *= 0.5f;
        Destroy(m_Reticle.GetComponent<SphereCollider>());
        m_ReticleRenderer = m_Reticle.GetComponent<Renderer>();

        //if the line origin is empty, default to the player position.
        if(m_LineOrigin == null){
            m_LineOrigin = transform;
        }
    }

    private void Update()
    {
        HitTest();
        m_Reticle.transform.position = m_HitInfo.point;

        //set line positions to 0 by default
        m_LineRenderer.SetPosition(1, Vector3.zero);
        m_LineRenderer.SetPosition(0, Vector3.zero);

        if (m_Hit)
        {
            //draw line renderer if we get a hit
            m_LineRenderer.SetPosition(1, m_LineOrigin.position);
            m_LineRenderer.SetPosition(0, m_HitInfo.point);

            if (m_HitInfo.collider.CompareTag(m_PortalWallTag))
            {
                m_ReticleRenderer.material = m_ValidHitMaterial;
                m_LineRenderer.materials[0] = m_ValidHitMaterial;
            }
            else
            {
                m_ReticleRenderer.material = m_InvalidHitMaterial;
            }
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, transform.forward * m_HitMaxDistance);
        if (m_Hit)
        {
            Gizmos.DrawSphere(m_HitInfo.point, 0.1f);
        }
    }
}
