using System.Collections.Generic;
using Org.BouncyCastle.Crypto.Tls;
using UnityEngine;

namespace NFTConnect
{
    
    public static class WeaponsOwnedData
    {
        public static readonly List<bool> WeaponOwn = new List<bool>(new bool[] { false, false, false, false });
        private static int EquipWeapon = -1;
        
        public static void SetData(bool wep0, bool wep1, bool wep2, bool wep3)
        {
            Debug.Log("Clear Data");
            WeaponOwn.Clear();
           
            WeaponOwn.Add(wep0); Debug.Log("data weapon 0: "+wep0); 
            WeaponOwn.Add(wep1); Debug.Log("data weapon 1: "+wep1); 
            WeaponOwn.Add(wep2); Debug.Log("data weapon 2: "+wep2); 
            WeaponOwn.Add(wep3); Debug.Log("data weapon 3:"+wep3);
            EquipWeaponInit();

        }
        public static void EquipWeaponIndex(int index)
        {
            if (WeaponOwn[index])
            {
                EquipWeapon = index;
                Debug.Log("Equip: Weapon #" + index);
            }
            else Debug.Log(" Equip: Fail");
        }

        public static void EquipWeaponInit()
        {
            if (EquipWeapon != -1) return;
            var index = -1;
            
            foreach (var wp in WeaponOwn)
            {
                index++;
                if (wp) 
                {
                    EquipWeapon = index;
                    Debug.Log("Init Equip: Weapon #" + index);
                    break;
                }
            }
           
        }
        public static int GetEquipWeapon()
        {
            return EquipWeapon;
        }
    }
}