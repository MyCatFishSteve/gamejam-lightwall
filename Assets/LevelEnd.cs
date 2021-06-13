using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour
{
    public string levelName;
    public Renderer fadeRenderer;

    //controls the speed of the fade
    public float fadeSpeed = 3.0f;
    //controlls where the door is for the lerp.
    private float d = 1.0f;

    public bool fade = false;

    private void FixedUpdate()
    {
        //lerp door controller value between 0 and 1 based on lock condition
        if (fade){
            d += Time.deltaTime * fadeSpeed;
        }else{
            d -= Time.deltaTime * fadeSpeed;
        }

        d = Mathf.Clamp(d, 0, 1);

        fadeRenderer.material.color = new Color(0, 0, 0, (1.0f * d));

        if(d == 1)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player") {
            fade = true;
        }
    }
}
