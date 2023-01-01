using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// This will check if all targets referenced are destroyed, and trigger the action when this is true
/// </summary>
public class TargetCheckerSpawner : GameTrigger
{
    public List<Target> targetsToCheck;
    public TargetSpawner targetSpawner;
    public bool ready = false;
    public bool Trigged = false;
    void Update()
    {
        //add target to check
        if (targetsToCheck.Count < targetSpawner.m_ActiveElements.Count)
        {
            targetsToCheck.Add(targetSpawner.m_ActiveElements[targetsToCheck.Count].target);
            ready = true; //ready to Spawn key after init first target
        }

    }

    private void LateUpdate()
    {
        if (!ready) return; 
        bool allDone = true;
        for(int i = 0; i < targetsToCheck.Count && allDone; ++i)
        {
            allDone &= targetsToCheck[i].Destroyed;
        }

        if (allDone)
        {
            if (Trigged) return;
            Trigged = true; 
            Trigger(); 
            //Destroy(gameObject);
        }
    }

#if UNITY_EDITOR

    public void OnDrawGizmosSelected()
    {
        if (targetsToCheck == null)
            return;
        
        foreach (var target in targetsToCheck)
        {
            Handles.DrawDottedLine(transform.position, target.transform.position, 10);
        }
    }


    
#endif
}
