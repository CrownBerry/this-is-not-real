using System;
using Interfaces;
using Player;
using Puzzles.Enums;
using UnityEngine;

namespace Puzzles
{
    public class SimpleSwitcher : MonoBehaviour, ISwitchable, IInteractable
    {
        public String PuzzleName;

        public SwitcherState state;
        public bool isPlayerNear;

        private void Awake()
        {
            state = SwitcherState.ClosePosition;
            isPlayerNear = false;

            AddItselfToPuzzle();
        }

        private void AddItselfToPuzzle()
        {
            try
            {
                var puzzle = GameObject.Find(PuzzleName).GetComponent<IPuzzle>();
                puzzle.AddToList(this);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }

        public void Interact()
        {
            switch (state)
            {
                case SwitcherState.OpenPosition:
                    state = SwitcherState.ClosePosition;
                    break;
                case SwitcherState.ClosePosition:
                    state = SwitcherState.OpenPosition;
                    break;
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

        public bool IsClose()
        {
            return state == SwitcherState.ClosePosition;
        }
    }
}