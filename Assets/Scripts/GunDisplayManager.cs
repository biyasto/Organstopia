using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GunDisplayManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]  List<GameObject> gunList;
    [SerializeField]  List<GameObject> gunData;
    [SerializeField]  GameObject gunName;
    [SerializeField]  GameObject gunDamage;
    [SerializeField]  GameObject gunTriggerType;
    [SerializeField]  GameObject gunFireRate;
    [SerializeField]  GameObject gunReload;
    [SerializeField]  GameObject gunClipsize;
    private int DisplayIndex = 0;
    void Start()
    {
        
        DisplayGun(DisplayIndex);
    }

    public void ChangeDisplay(bool i)
    {
        if (i) DisplayIndex++;
        else DisplayIndex--;
        if (DisplayIndex >= gunList.Count || DisplayIndex >= gunData.Count) DisplayIndex = 0;
        if (DisplayIndex < 0) DisplayIndex = gunList.Count-1;
        DisplayGun(DisplayIndex);
    }

    public void BuyGun()
    {
        Application.OpenURL("https://opensea.io/collection/organstopia");
    }
    void DisplayGun(int index)
    {
        for (int i = 0; i < gunList.Count; i++)
        {
            if (i == index)
            {
                gunList[i].SetActive(true);
                gunList[i].gameObject.transform.eulerAngles = new Vector3(0, 90, 0);
                gunName.gameObject.GetComponent<TextMeshProUGUI>().text = gunData[i].gameObject.name;
              
                gunTriggerType.gameObject.GetComponent<TextMeshProUGUI>().text=  "Gun Type: " + gunData[i].gameObject.GetComponent<Weapon>().triggerType;
                gunDamage.gameObject.GetComponent<TextMeshProUGUI>().text = "Damage: " + gunData[i].gameObject.GetComponent<Weapon>().damage;
                gunFireRate.gameObject.GetComponent<TextMeshProUGUI>().text = "Fire Rate: " +gunData[i].gameObject.GetComponent<Weapon>().fireRate;
                gunReload.gameObject.GetComponent<TextMeshProUGUI>().text = "Reload Time: " +gunData[i].gameObject.GetComponent<Weapon>().reloadTime;
                gunClipsize.gameObject.GetComponent<TextMeshProUGUI>().text = "Clip Size: " +gunData[i].gameObject.GetComponent<Weapon>().clipSize;
               
            }
            else gunList[i].SetActive(false);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
