using System;
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using Puzzles.Enums;
using UnityEngine;

namespace Puzzles
{
    public class SimpleDoorPuzzle : MonoBehaviour, IPuzzle
    {
        PuzzleState state;
        IList<ISwitchable> switchers = new List<ISwitchable>();

        void Awake()
        {
            state = PuzzleState.NotSolved;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                foreach (var switchable in switchers)
                {
                    Debug.Log(string.Format("{0}", switchable.IsClose()));
                }
            }

            if (!switchers.Any()) return;

            if (switchers.Any(_ => _.IsClose()))
            {
                if (state == PuzzleState.NotSolved) return;
                Close();
                return;
            }

            if (state == PuzzleState.Solved) return;

            Open();
            state = PuzzleState.Solved;
        }

        public void AddToList(ISwitchable switcher)
        {
            switchers.Add(switcher);
        }

        public void Open()
        {
            state = PuzzleState.Solved;
            Debug.Log(string.Format("Puzzle {0} solved", gameObject.name));
        }

        public void Close()
        {
            state = PuzzleState.NotSolved;
            Debug.Log(string.Format("Puzzle {0} not solved", gameObject.name));
        }
    }
}