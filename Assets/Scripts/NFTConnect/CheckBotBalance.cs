using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class CheckBotBalance : MonoBehaviour
{
    private string tokenId = "19928718629562379247015131315189570608910795369549818420571239620048693755914";
    private string contract = "0x2953399124F0cBB46d2CbACD8A89cF0599974963";
    
    public  bool have;
    

    private async  void Start()
    {
        string chain = "polygon";
        string network = "testnet";
        string account = PlayerPrefs.GetString("Account");

        BigInteger balanceOf = await ERC1155.BalanceOf(chain, network, contract, account, tokenId);
        print("bot"+balanceOf);
        if (balanceOf > 0)
        {
            have = true;
        }
    }
}
