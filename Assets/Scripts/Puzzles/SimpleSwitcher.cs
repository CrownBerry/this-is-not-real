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

        private SwitcherState state;
        private bool isPlayerNear;
        private Renderer renderer;


        private void Awake()
        {
            state = SwitcherState.ClosePosition;
            isPlayerNear = false;
            renderer = GetComponent<Renderer>();

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
                    renderer.material.SetColor("_Color", new Color(0, 180f/255f, 80f/255f));
                    break;
                case SwitcherState.ClosePosition:
                    state = SwitcherState.OpenPosition;
                    renderer.material.SetColor("_Color", new Color(180f/255f, 0, 20f/255f));
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