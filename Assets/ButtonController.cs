using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public bool activated = false;
    public bool inverted = false;
    public bool toggle = false;
    public Renderer buttonBase;

    public Material buttonPressMat;
    public Material buttonReleasedMat;

    public List<Rigidbody> rigidbodies = new List<Rigidbody>();

    public IToggle targetComponent;

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
            PerformAction();
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
                PerformAction();
            }
        }
    }

    private bool CheckState()
    {
        if(rigidbodies.Count > 0){
            buttonBase.material = buttonPressMat;
            return Invert(true);
        }else{
            buttonBase.material = buttonReleasedMat;
            return Invert(false);
        }
    }

    //this has kinda become a slight mess but this allows for inverting stuff to work
    private void PerformAction()
    {
        if (toggle){
            if (activated){
                targetComponent.Toggle();
            }
            return;
        }

        if (activated){
            targetComponent.Enable();
        }else{
            targetComponent.Disable();
        }
    }

    private bool Invert(bool state)
    {
        if (inverted){
            return !state;
        }else{
            return state;
        }
    }
}
