using System.Security.Cryptography.X509Certificates;
using Interfaces;
using JetBrains.Annotations;
using Player;
using Player.FSM;
using UnityEngine;

namespace System.Objects
{
    public class BoxObject : MonoBehaviour, IInteractable
    {
        private BoxStateMachine state;
        private Rigidbody rigidbody;
        [CanBeNull] private PlayerClass player;


        private void Awake()
        {
            state = new BoxStateMachine(this, BoxStateMachine.State.None);
            rigidbody = GetComponent<Rigidbody>();
        }

        #region Events

        private void OnEnable()
        {
            EventManager.StartListening("DropBox", DropOnJump);
        }

        private void OnDisable()
        {
            EventManager.StopListening("DropBox", DropOnJump);
        }

        private void OnDestroy()
        {
            EventManager.StopListening("DropBox", DropOnJump);
        }

        #endregion

        public void Interact()
        {
            player.carryingState.Next();
            state.Switch(player);
        }

        public void DropOnJump(params object[] list)
        {
            if (player == null) return;

            if (!state.IsCarry()) return;

            player.SetMaximumSpeed(5f);
            player.carryingState.Next();
            player.RemoveInteract();
            player = null;
            state.Drop();
        }

        private void Update()
        {
            if (player == null) return;

            if (!state.IsCarry()) return;

            var directionVector = player.transform.position - transform.position;
            var distantion = directionVector.magnitude;
            var pullDirection = directionVector.normalized;

            if (distantion > 5)
            {
                player.SetMaximumSpeed(5f);
                player.carryingState.Next();
                player.RemoveInteract();
                player = null;
                state.Drop();
            }
            else if (distantion > 0.3)
            {
                var pullForce = 10f;
                var pullForDistantion = (distantion - 0.3f) / 2f;
                if (pullForDistantion > 20f)
                {
                    pullForDistantion = 20f;
                }
                pullForce += pullForDistantion;
                state.Move(pullDirection, pullForce);
            }
        }

        public void Move(Vector3 pullDir, float pullF)
        {
            rigidbody.velocity += pullDir * (pullF * Time.deltaTime);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.transform.CompareTag("Player"))
            {
                player = other.transform.GetComponent<PlayerClass>();
                player.GiveInteract(this);
            }
        }
    }
}