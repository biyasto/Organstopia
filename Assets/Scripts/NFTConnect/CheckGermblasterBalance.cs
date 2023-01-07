using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class CheckGermblasterBalance : MonoBehaviour
{
    //xoa so 1 o cuoi
    private string tokenId = "38943131031766143704984983154691040388593436270428817556432674375268474814479";
    private string contract = "0x2953399124F0cBB46d2CbACD8A89cF0599974963";
    public  bool have;
    

    private async  void Start()
    {
        string chain = "polygon";
        string network = "testnet";
        string account = PlayerPrefs.GetString("Account");

        BigInteger balanceOf = await ERC1155.BalanceOf(chain, network, contract, account, tokenId);
        print("GermBlaster Gun: "+balanceOf);
        if (balanceOf > 0)
        {
            have = true;
        }
      
    }
}
