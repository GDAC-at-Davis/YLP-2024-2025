using UnityEngine;
using System;
using System.Collections.Generic;
using Hitbox.Emitters;
using Hitbox.System;
using Timeline;
using Random = UnityEngine.Random;

public class CoffinTimelineController : MonoBehaviour
{
    public event Action<HitboxInstantiateResult> OnLandHit;

    [SerializeField]
    private BasicHitboxEmitter hitboxEmitter;

    [SerializeField]
    private ManualTimelinePlayer coffinOpenClose;

    [SerializeField]
    private ManualTimelinePlayer coffinChewOpen;

    [SerializeField]
    private float minIdleTime;

    [SerializeField]
    private float maxIdleTime;

    private float timeToIdle;

    private float idleTimer;

    private bool isIdle = true;

    private bool isChewing = false;

    private void Awake()
    {
        isIdle = true;
        timeToIdle = Random.Range(minIdleTime, maxIdleTime);
    }

    private void Update()
    {
        if(isIdle && !isChewing)
        {
            if(idleTimer > timeToIdle)
            {
                isIdle = false;
                coffinOpenClose.Play();
                Debug.Log("Playing coffin open, idleTimer: " + idleTimer + " timeToIdle: " + timeToIdle);
            }
            else
            {
                idleTimer += Time.deltaTime;
            }
        }
    }

    private void FixedUpdate()
    {
        if(!isIdle)
        {
            if(isChewing)
            {
                coffinChewOpen.Evaluate(Time.fixedDeltaTime);
            }
            else
            {
                coffinOpenClose.Evaluate(Time.fixedDeltaTime);
            }
        }
    }

    public void OnCoffinClose()
    {
        Debug.Log("OnCoffinClose");
        if (!isChewing)
        {
            isIdle = true;
            idleTimer = 0;
            timeToIdle = Random.Range(minIdleTime, maxIdleTime);
        }
    }

    public void OnPlayerTrapped()
    {
        if (!isChewing)
        {
            Debug.Log("Playing coffin chew");
            isChewing = true;
            coffinChewOpen.Play();
        }
    }

    public void OnCoffinFinish()
    {
        Debug.Log("Returning to idle");
        idleTimer = 0;
        isIdle = true;
        isChewing = false;
        timeToIdle = Random.Range(minIdleTime, maxIdleTime);
    }
}
