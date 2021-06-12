using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public bool activated = false;
    public Renderer buttonBase;

    public Material buttonPressMat;
    public Material buttonReleasedMat;

    public List<Rigidbody> rigidbodies = new List<Rigidbody>();

    public void Awake()
    {
        CheckState();
    }

    public void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
        if(rb != null){
            rigidbodies.Add(rb);
            activated = CheckState();
            Debug.Log("Pressed");
        }
    }
    public void OnTriggerExit(Collider other)
    {
        Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
        if (rb != null){
            Debug.Log("Released");
            if (rigidbodies.Contains(rb)){
                rigidbodies.Remove(rb);
                activated = CheckState();
            }
        }
    }

    private bool CheckState()
    {
        if(rigidbodies.Count > 0){
            buttonBase.material = buttonPressMat;
            return true;
        }else{
            buttonBase.material = buttonReleasedMat;
            return false;
        }
    }
}
