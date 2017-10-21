using System.Managers;
using UnityEngine;

namespace Puzzles
{
    public class KeyObject : MonoBehaviour
    {
        public string PuzzleName;
        private KeyStorage keyStorage;

        private void Awake()
        {
            keyStorage = GameObject.Find("GameManager").GetComponent<KeyStorage>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("Player")) return;

            keyStorage.AddKey(PuzzleName);
            Destroy(gameObject);
        }
    }
}