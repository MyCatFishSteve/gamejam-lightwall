using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class IToggle : MonoBehaviour
{
    abstract public void Enable();
    abstract public void Disable();
    abstract public void Toggle();
}
