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
    static public List<bool> WeaponOwn  = new List<bool>();
    bool[] weapons= new bool[3];
    public TextMeshProUGUI address;
    // Start is called before the first frame update
    
    private void Start()
    {
        address.text = PlayerPrefs.GetString("Account");
    }

    void  SaveData()
    {
        weapons[0]=gameObject.GetComponent<CheckFireBalance>().have;
        weapons[1]=gameObject.GetComponent<CheckPieceBalance>().have;
        weapons[2]=gameObject.GetComponent<CheckUnicellBalance>().have;
        
        WeaponsOwnedData.SetData(weapons[0],weapons[1],weapons[2]);

    }
    

    // Update is called once per frame
    void Update()
    {
    
    }

    public  void EnterGame()
    {
        bool isPlayAble=false;
        for (int i = 0; i < 5; i++)
        {
            if (weapons[i] == true) {isPlayAble = true;
                break;
            }
        }
        SaveData();
        if(isPlayAble)
        SceneManager.LoadScene("TitleScreen");
       
    }

    public void OpenInventory()
    {
        SceneManager.LoadScene("InventoryScene");
        SaveData();
    }
}
