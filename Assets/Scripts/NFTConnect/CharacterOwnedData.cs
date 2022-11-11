using System.Collections.Generic;
using Org.BouncyCastle.Crypto.Tls;
using UnityEngine;

namespace NFTConnect
{
    
    public static class CharacterOwnedData
    {
        public static List<bool> CharacterOwn = new List<bool>();

        public static void SetData(bool char0, bool char1, bool char2, bool char3, bool char4)
        {
            CharacterOwn.Clear();
           
                CharacterOwn.Add(char0);  Debug.Log("save data bot"+char0); 
                CharacterOwn.Add(char1); Debug.Log("save data ent"+char1); 
                CharacterOwn.Add(char2); Debug.Log("save data fire"+char2); 
                CharacterOwn.Add(char3); Debug.Log("save data piece"+char3); 
                CharacterOwn.Add(char4); Debug.Log("save data unicell"+char4); 

        }
      
    }
}