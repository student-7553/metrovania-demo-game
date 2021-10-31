using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundCommet : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator anim;
    public int eachTime;
    private bool running;
    void Start()
    {
        anim = GetComponent<Animator>();
        running = true;
        StartCoroutine(DoWork(eachTime));
    }

    IEnumerator DoWork(int time) 
    {        
        // Do the job until running is set to false
        while (running)
        {   
            anim.SetTrigger("trigger");
            // wait for seconds
            yield return new WaitForSeconds(time);
        }
}


}
