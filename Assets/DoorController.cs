using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : IToggle
{
    public bool unlocked = false;

    public GameObject light_locked;
    public GameObject light_unlocked;
    public Transform doorLeft;
    public Transform doorRight;

    public float doorTravelDistance = 2.0f;
    public float doorSpeed = 3.0f;

    //controlls where the door is for the lerp.
    private float d = 0.0f;

    private Vector3 doorStartPos;

    public AudioSource audioS;
    public AudioClip doorOpenSFX;
    public AudioClip doorCloseSFX;

    private void Awake()
    {
        doorStartPos = doorLeft.position;
    }

    private void FixedUpdate()
    {
        //lerp door controller value between 0 and 1 based on lock condition
        if (unlocked){
            d += Time.deltaTime * doorSpeed;
        }else{
            d -= Time.deltaTime * doorSpeed;
        }

        d = Mathf.Clamp(d, 0, 1);

        float doorMovement = d * doorTravelDistance;
        Vector3 LDoorPos = new Vector3(doorStartPos.x + doorMovement, doorStartPos.y, doorStartPos.z);
        Vector3 RDoorPos = new Vector3(doorStartPos.x - doorMovement, doorStartPos.y, doorStartPos.z);

        doorLeft.position = LDoorPos;
        doorRight.position = RDoorPos;

        SetLights(unlocked);
    }

    private void SetLights(bool state)
    {
        light_locked.SetActive(!state);
        light_unlocked.SetActive(state);
    }

    override public void Enable()
    {
        if(unlocked == true){
            return;
        }
        unlocked = true;
        audioS.PlayOneShot(doorOpenSFX);
        SetLights(unlocked);
    }

    override public void Disable()
    {
        if(unlocked == false){
            return;
        }
        unlocked = false;
        audioS.PlayOneShot(doorCloseSFX);
        SetLights(unlocked);
    }

    override public void Toggle()
    {
        unlocked = !unlocked;
        SetLights(unlocked);
    }
}
