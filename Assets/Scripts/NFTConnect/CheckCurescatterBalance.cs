using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class CheckCurescatterBalance : MonoBehaviour
{
    private string tokenId = "38943131031766143704984983154691040388593436270428817556432674377467498070034";//"38943131031766143704984983154691040388593436270428817556432674370870428303370";
    private string contract = "0x2953399124F0cBB46d2CbACD8A89cF0599974963";//"0x2953399124F0cBB46d2CbACD8A89cF0599974963";
    


    public  bool have;
    

    private async  void Start()
    {
        string chain = "polygon";
        string network = "testnet";
        string account = PlayerPrefs.GetString("Account");

        BigInteger balanceOf = await ERC1155.BalanceOf(chain, network, contract, account, tokenId);
        print("Curescatter Gun: "+balanceOf);
        if (balanceOf > 0)
        {
            have = true;
        }
    }
}
