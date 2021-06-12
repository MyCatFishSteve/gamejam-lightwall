using UnityEngine;

public class LightwallManager : MonoBehaviour
{
    private static LightwallManager m_Instance = null;

    private void Awake()
    {
        if (m_Instance != null)
        {
            Destroy(m_Instance);
        }
        m_Instance = this;
    }

    private void Start()
    {
        
    }

    private void FixedUpdate()
    {

    }
}
