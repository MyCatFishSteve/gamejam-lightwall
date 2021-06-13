using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStepController : MonoBehaviour
{
    public AudioClip[] footStep;
    public AudioSource audioSource;

    private void Awake()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    public void PlayFootStepSFX()
    {
        Debug.Log("Step!");
        int rand = Random.Range(0, footStep.Length);
        audioSource.PlayOneShot(footStep[rand]);
    }

    public void PlayFootStepSFX(int i)
    {
        audioSource.PlayOneShot(footStep[i]);
    }
}
