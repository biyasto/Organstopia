using System.Collections.Generic;
using Org.BouncyCastle.Crypto.Tls;
using UnityEngine;

namespace NFTConnect
{
    
    public static class WeaponsOwnedData
    {
        public static List<bool> WeaponOwn = new List<bool>();

        public static void SetData(bool wep0, bool wep1, bool wep2, bool wep3)
        {
            Debug.Log("Clear Data");
            WeaponOwn.Clear();
           
            WeaponOwn.Add(wep0); Debug.Log("data weapon 0: "+wep0); 
            WeaponOwn.Add(wep1); Debug.Log("data weapon 1: "+wep1); 
            WeaponOwn.Add(wep2); Debug.Log("data weapon 2: "+wep2); 
            WeaponOwn.Add(wep2); Debug.Log("data weapon 3:"+wep3); 
               

        }
      
    }
}