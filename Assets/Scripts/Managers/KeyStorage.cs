using System.Collections.Generic;
using UnityEngine;

namespace System.Managers
{
    public class KeyStorage : MonoBehaviour
    {
        private Dictionary<string, int> storage = new Dictionary<string, int>();

        public void AddKey(string puzzleName)
        {
            if (storage.ContainsKey(puzzleName))
            {
                storage[puzzleName] = storage[puzzleName] + 1;
            }
            else
            {
                storage.Add(puzzleName, 1);
            }
        }

        public bool HasEnoughKey(string puzzleName, int keys)
        {
            return storage[puzzleName] == keys;
        }
    }
}