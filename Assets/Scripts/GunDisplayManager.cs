using System;
using System.Collections;
using System.Collections.Generic;
using NFTConnect;
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

    [SerializeField] private GameObject Locks;
    [SerializeField]  GameObject EquipCheck;
    public int DisplayIndex = 0;
    void Start()
    {

        DisplayIndex = WeaponsOwnedData.GetEquipWeapon();
        CheckValidIndex();
        DisplayGun(DisplayIndex);
    }

    public void CheckValidIndex()
    {
        if (DisplayIndex < 0 || DisplayIndex >= gunData.Count) 
            DisplayIndex = 0;
    }

    public void ChangeDisplay(bool i)
    {
        if (i) DisplayIndex++;
        else DisplayIndex--;
        CheckValidIndex();
        if (DisplayIndex < 0) DisplayIndex = gunList.Count-1;
        DisplayGun(DisplayIndex);
    }

    public void BuyGun()
    {
        Application.OpenURL("https://opensea.io/collection/organstopia");
    }
    void DisplayGun(int index)
    {
        //Gun equip mark
       
        EquipCheck.SetActive(WeaponsOwnedData.GetEquipWeapon()==index);
        //Debug.Log("index:" + index + " equip:" + WeaponsOwnedData.GetEquipWeapon());
       for (int i = 0; i < gunList.Count; i++)
        {
            if (i == index)
            {
                //gun render
                gunList[i].SetActive(true);
                gunList[i].transform.localPosition  = new Vector3(0, -1.3f, 0);
                gunList[i].GetComponent<Rigidbody>().isKinematic = false;
                gunList[i].gameObject.transform.eulerAngles = new Vector3(0, 90, 0);
                gunName.gameObject.GetComponent<TextMeshProUGUI>().text = gunData[i].gameObject.name;
              
                //Gun data
                gunTriggerType.gameObject.GetComponent<TextMeshProUGUI>().text=  "GUN TYPE: " + gunData[i].gameObject.GetComponent<Weapon>().triggerType;
                gunDamage.gameObject.GetComponent<TextMeshProUGUI>().text = "DAMAGE: " + gunData[i].gameObject.GetComponent<Weapon>().damage;
                gunFireRate.gameObject.GetComponent<TextMeshProUGUI>().text = "FIRE RATE: " +gunData[i].gameObject.GetComponent<Weapon>().fireRate;
                gunReload.gameObject.GetComponent<TextMeshProUGUI>().text = "RELOAD TIME: " +gunData[i].gameObject.GetComponent<Weapon>().reloadTime;
                gunClipsize.gameObject.GetComponent<TextMeshProUGUI>().text = "CLIP SIZE: " +gunData[i].gameObject.GetComponent<Weapon>().clipSize;
                
                //Gun locks
                try
                {
                    Locks.SetActive(!WeaponsOwnedData.WeaponOwn[index]);
                    
                }
                catch (Exception e)
                {
                    Locks.SetActive(true);
                    Console.WriteLine(e);
                   // throw;
                }
            }
            else
            {
                gunList[i].SetActive(false);
                gunList[i].GetComponent<Rigidbody>().isKinematic = true;
            }
        }
    }
    // Update is called once per frame
    public void Equip()
    {
        WeaponsOwnedData.EquipWeaponIndex(DisplayIndex);
        DisplayGun(DisplayIndex);
    }
}
