﻿using System;
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using Puzzles.Enums;
using UnityEngine;
using UnityEngine.Networking;

namespace Puzzles
{
    public class SimpleDoorPuzzle : MonoBehaviour, IPuzzle
    {
        PuzzleState state;
        IList<ISwitchable> switchers = new List<ISwitchable>();

        private Vector3 openPosition;
        private Vector3 closePosition;

        void Awake()
        {
            state = PuzzleState.NotSolved;
            closePosition = transform.position;
            openPosition = new Vector3(transform.position.x,
                transform.position.y - transform.localScale.y,
                transform.position.z);
        }

        private void Update()
        {
            ChangePosition();
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

        private void ChangePosition()
        {
            var newPosition = new Vector3();
            switch (state)
            {
                case PuzzleState.Solved:
                    newPosition = openPosition;
                    break;
                case PuzzleState.NotSolved:
                    newPosition = closePosition;
                    break;
            }

            transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime);
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