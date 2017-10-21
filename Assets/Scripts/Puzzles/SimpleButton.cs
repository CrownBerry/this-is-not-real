using System.Managers;
using Interfaces;
using Player;
using UnityEngine;

namespace Puzzles
{
    public class SimpleButton : MonoBehaviour, IInteractable
    {
        public string PuzzleName;
        private IPuzzle puzzle;
        private KeyStorage keyStorage;

        private void Awake()
        {
            puzzle = GameObject.Find(PuzzleName).GetComponent<IPuzzle>();
            keyStorage = GameObject.Find("GameManager").GetComponent<KeyStorage>();
        }

        public void Interact()
        {
            if (keyStorage.HasEnoughKey(PuzzleName, puzzle.NeedKey()))
            {
                puzzle.Open();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                other.GetComponent<PlayerClass>().GiveInteract(this);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                other.GetComponent<PlayerClass>().RemoveInteract();
            }
        }
    }
}