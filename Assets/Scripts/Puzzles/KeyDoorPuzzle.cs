using Interfaces;
using Puzzles.Enums;
using UnityEngine;

namespace Puzzles
{
    public class KeyDoorPuzzle : MonoBehaviour, IPuzzle
    {
        public int KeysNeeded;
        private PuzzleState state;
        private Vector3 openPosition;

        private void Awake()
        {
            state = PuzzleState.NotSolved;
            openPosition = new Vector3(transform.position.x,
                transform.position.y - transform.localScale.y,
                transform.position.z);
        }

        private void Update()
        {
            if (state != PuzzleState.Solved) return;

            transform.position = Vector3.Lerp(transform.position, openPosition, Time.deltaTime);
        }

        public void AddToList(ISwitchable switcher)
        {
        }

        public void Open()
        {
            state = PuzzleState.Solved;
        }

        public void Close()
        {
        }

        public int NeedKey()
        {
            return KeysNeeded;
        }
    }
}