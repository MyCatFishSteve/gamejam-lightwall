using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSensor : MonoBehaviour
{
    /// <summary>
    /// Light sensor permanently activated after one activation
    /// </summary>
    [SerializeField]
    private bool m_OneShot = false;

    /// <summary>
    /// Component to be activated by the light sensor
    /// </summary>
    [SerializeField]
    private IToggle m_TargetComponent = null;

    private void Start()
    {
        Debug.Assert(m_TargetComponent != null, "target component not set for light sensor", this);
    }

    public void Activate()
    {
        m_TargetComponent.Enable();
    }

    public void Deactivate()
    {
        if (m_OneShot)
            return;
        m_TargetComponent.Disable();
    }
}
