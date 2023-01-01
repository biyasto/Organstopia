using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.PlayerLoop;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace System
{
    public class RespawnChecker : MonoBehaviour
    {

        public TargetSpawner targetSpawner;
        public List<Target> targetsList;
        public bool isRespawn = false;
        public bool isComplete = false;
        
        [SerializeField] private int pointValue = 15;
        [SerializeField] float respawnCheckTime = 0.3f;
        
        private float respawnCheckTimer = 0;
        private int CountTargetInit = 0;
        private void Start()
        {
            respawnCheckTimer = respawnCheckTime;
            CountTargetInit = targetsList.Count;
        }
        void Update()
        {
            if (targetSpawner == null) return;
            //add target to check
            if (targetsList.Count - CountTargetInit < targetSpawner.m_ActiveElements.Count)
            {
                targetsList.Add(targetSpawner.m_ActiveElements[targetsList.Count-CountTargetInit].target);
            }

        }

        public void DestroyList()
        {
            foreach (var target in targetsList)
            {
                if (!target.gameObject.activeSelf)
                {
                    target.gameObject.SetActive(false);
                }
            } 
            Destroy(gameObject);
        }
        void FixedUpdate()
        {
            if (isComplete) return;
            
            respawnCheckTimer -= Time.fixedDeltaTime;
            if(respawnCheckTimer<=0)
            {
                //reset timer
                respawnCheckTimer = respawnCheckTime;
                
                //check inactive targets
                int countInactiveTarget = 0;
                foreach (var target in targetsList)
                {
                    if (!target.gameObject.activeSelf)
                    {
                        countInactiveTarget++;
                    }
                }

                if (countInactiveTarget >= targetsList.Count)
                {
                    isRespawn = false;
                    isComplete = true;
                    GameSystem.Instance.TargetDestroyed(pointValue);
                    DestroyList();
                }
                else if (countInactiveTarget <= 0)
                {
                    isRespawn = false;
                }
                else
                {
                    isRespawn = true;
                }

                if (!isComplete && isRespawn)
                {
                    foreach (var target in targetsList)
                    {
                        if (!target.gameObject.activeSelf)
                        {
                            target.gameObject.SetActive(true);
                        }
                    }
                }
            }
        }
    }
}