using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupTrigger : MonoBehaviour
{
    [SerializeField] private GameObject popupCanvas;
    [SerializeField] private GameObject character;
    private void Start()
    {
        popupCanvas.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        
        popupCanvas.SetActive(true);
        character.gameObject.GetComponent<Controller>().DisplayCursor(true);
     
    }
    public void TurnOff()
    {
        popupCanvas.SetActive(false);
        character.gameObject.GetComponent<Controller>().DisplayCursor(false);
        Destroy(gameObject);
    }
    
}

