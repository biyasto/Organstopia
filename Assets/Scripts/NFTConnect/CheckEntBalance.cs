
using System.Numerics;
using UnityEngine;


    public class CheckEntBalance : MonoBehaviour
    {
        private string tokenId = "19928718629562379247015131315189570608910795369549818420571239618949182128138";
        private string contract = "0x2953399124F0cBB46d2CbACD8A89cF0599974963";
        public  bool have;
    

        private async  void Start()
        {
            string chain = "polygon";
            string network = "testnet";
            string account = PlayerPrefs.GetString("Account");

            BigInteger balanceOf = await ERC1155.BalanceOf(chain, network, contract, account, tokenId);
            print("ent"+balanceOf);
            if (balanceOf > 0)
            {
                have = true;
            }
        }

      
    }

