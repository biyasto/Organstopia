using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using NFTConnect;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenMenu : MonoBehaviour
{
   // static public List<bool> WeaponOwn  = new List<bool>();
    bool[] weapons= new bool[4];
    public TextMeshProUGUI address;
    // Start is called before the first frame update
    
    private void Start()
    {
        address.text = PlayerPrefs.GetString("Account");
    }

    void  SaveData()
    {
        Debug.Log("Open menu try save");
        weapons[0]=gameObject.GetComponent<CheckHealmaticBalance>().have;
        weapons[1]=gameObject.GetComponent<CheckGermblasterBalance>().have;
        weapons[2]=gameObject.GetComponent<CheckPurifierBalance>().have;
        weapons[3]=gameObject.GetComponent<CheckCurescatterBalance>().have;
       
        
        WeaponsOwnedData.SetData(weapons[0],weapons[1],weapons[2],weapons[3]);

    }
    

    // Update is called once per frame
    void Update()
    {
    
    }

    public  void EnterGame()
    {
        /*bool isPlayAble=false;
        for (int i = 0; i < 4; i++)
        {
            if (weapons[i] == true) {isPlayAble = true;
                break;
            }
        }*/
        SaveData();
       // if(isPlayAble)
        //SceneManager.LoadScene("TitleScreen");
       
    }

    public void OpenInventory()
    {
        SceneManager.LoadScene("InventoryScene");
        SaveData();
    }
}
