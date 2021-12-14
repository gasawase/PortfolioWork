//TODO: register a player pressing a button to trigger, trigger animation, trigger change in cutoff

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controlStealth : MonoBehaviour
{
    //should probably be an array
    [SerializeField] GameObject mainBody;
    [SerializeField] ParticleSystem smokeSys;
    [SerializeField] private int timesToCycle = 200;
    
    private Material material;
    private Animator mainAnim;
    private bool running;
    private float idleTime;
    private float slamTime;

    private void Start()
    {
        material = GetComponent<SkinnedMeshRenderer>().sharedMaterial;
        mainAnim = mainBody.GetComponent<Animator>();
        //Animator mainAnim = mainBody.GetComponent<Animator>();
        material.SetFloat("Cutoff", (-0.5f) );

        //UpdateAnimClipTimes();
    }

    public void Update()
    {
        //checks for e button pressed
        if (Input.GetButtonDown("StealthAbilityTrigger"))
        {
            Debug.Log("pressed");
            if (!running)
            {
                StartCoroutine(stealthPowerTriggeredCR());
            }
            
        }

    }

    public void UpdateAnimClipTimes()
    {
        AnimationClip[] clips = mainAnim.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            switch (clip.name)
            {
                case "NinjaIdle":
                    idleTime = clip.length;
                    Debug.Log(idleTime);
                    break;
                case "MagicSlam":
                    slamTime = clip.length;
                    Debug.Log(slamTime);
                    break;
            }
        }
    }

    private IEnumerator stealthPowerTriggeredCR()
    {
        //what cutoffStandIn means
        float cutoffStandIn = 0f;
        float waitSlamDone = 1.5f;
        //set transition to magic bomb animation to true
        mainAnim.SetBool("isMimicStealth", true);
        running = true;
        //wait until idle animation is done
        
        //yield return new WaitForSeconds(mainAnim.GetCurrentAnimatorStateInfo(0).length + mainAnim.GetCurrentAnimatorStateInfo(0).normalizedTime);
        //mainBody.GetComponent<Animator>().SetBool("isMimicStealth", false);

        yield return new WaitForSeconds(waitSlamDone);
        
        smokeSys.Play();

        WaitForSeconds wait = new WaitForSeconds(.004f);
        
        //transition from normal to sneak
        for (int i = 0; i < timesToCycle; i++)
        {
            cutoffStandIn += .01f;
            //need to add a wait
            material.SetFloat("Cutoff", cutoffStandIn );
            yield return wait;
            print(cutoffStandIn);
        }
        running = false;
        mainBody.GetComponent<Animator>().SetBool("isMimicStealth", false);
        
    }
}
