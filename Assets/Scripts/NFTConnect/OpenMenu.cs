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
    static public List<bool> CharacterOwn  = new List<bool>();
    bool[] character= new bool[5];
    public TextMeshProUGUI address;
    // Start is called before the first frame update
    
    private void Start()
    {
        address.text = PlayerPrefs.GetString("Account");
    }

    void  SaveData()
    {
        
        character[0]=gameObject.GetComponent<CheckBotBalance>().have;
        character[1]=gameObject.GetComponent<CheckEntBalance>().have;
        character[2]=gameObject.GetComponent<CheckFireBalance>().have;
        character[3]=gameObject.GetComponent<CheckPieceBalance>().have;
        character[4]=gameObject.GetComponent<CheckUnicellBalance>().have;
        
        CharacterOwnedData.SetData(character[0],character[1],character[2],character[3],character[4]);

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
            if (character[i] == true) {isPlayAble = true;
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
