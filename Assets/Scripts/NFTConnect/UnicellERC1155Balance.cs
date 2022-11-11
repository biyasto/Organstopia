using System.Collections;
using System.Numerics;
using System.Collections.Generic;
using NFTConnect;
using UnityEngine;

public class UnicellERC1155Balance: MonoBehaviour, ICharacterToken
{
    public string tokenId = "38943131031766143704984983154691040388593436270428817556432674370870428303370";

  
    public bool isAvailable;
    public bool IsAvailable => isAvailable;

    public GameObject unitPrefab;
    public GameObject UnitPrefab { get => unitPrefab; set => unitPrefab = value; }

     private  void Start()
    {
      
    }

    public async void CheckAvailable()
    {
        /*string chain = "polygon";
        string network = "testnet";
        string contract = "0x2953399124F0cBB46d2CbACD8A89cF0599974963";
        string account = PlayerPrefs.GetString("Account");
        string tokenId = "38943131031766143704984983154691040388593436270428817556432674370870428303370";

        BigInteger balanceOf = await ERC1155.BalanceOf(chain, network, contract, account, tokenId);
        print( "fire"+balanceOf);

        if (balanceOf > 0)
        {
            isAvailable = true;
        }
        else
        {
            isAvailable = false;
        }*/
        isAvailable = CharacterOwnedData.CharacterOwn[4];
    }
}