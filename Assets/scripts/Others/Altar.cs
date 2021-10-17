using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using DG.Tweening;

public class Altar : MonoBehaviour
{
    private int playerLayer;	
    // private Animator anim;
    public GameObject alterLight;
    public GameObject alterText;
    private bool highlighted = false;

    private Light2D alterLightScript;

    public float lightIntensity = 8f;

    private bool inRange;

    void Start()
    {
        inRange = false;
        // anim = GetComponent<Animator>();
        playerLayer = LayerMask.NameToLayer("Player");
        alterLightScript = alterLight.GetComponent<Light2D>();
    }

    void Update()
    {
        // if(inRange){
        //     if(Input.GetButtonDown("Interact")){
        //         alterText.gameObject.SetActive(true);
        //     }
        // }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer != playerLayer){
            return;
        }

        inRange = true;
        float oldIntensity = alterLightScript.intensity;
        DOVirtual.Float(oldIntensity, lightIntensity, 2f, (float value) => {
            alterLightScript.intensity = value;
        });

        alterText.gameObject.SetActive(true);
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.layer != playerLayer){
            return;
        }
        inRange = false;
        float oldIntensity = alterLightScript.intensity;
        DOVirtual.Float(oldIntensity, 0, 2f, (float value) => {
            alterLightScript.intensity = value;
        });
        alterText.gameObject.SetActive(false);

    }
}
