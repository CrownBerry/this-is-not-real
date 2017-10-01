using System.Security.Cryptography.X509Certificates;
using JetBrains.Annotations;
using Player;
using Player.FSM;
using UnityEngine;

namespace System.Objects
{
    public class BoxObject : MonoBehaviour
    {
        private BoxStateMachine state;
        private Rigidbody rigidbody;
        [CanBeNull] private PlayerClass player;


        private void Awake()
        {
            state = new BoxStateMachine(this, BoxStateMachine.State.None);
            rigidbody = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            EventManager.StartListening("Interact", TryGrab);
            EventManager.StartListening("DropBox", DropOnJump);
        }

        private void OnDisable()
        {
            EventManager.StopListening("Interact", TryGrab);
            EventManager.StopListening("DropBox", DropOnJump);
        }

        private void OnDestroy()
        {
            EventManager.StopListening("Interact", TryGrab);
            EventManager.StopListening("DropBox", DropOnJump);
        }

        private void TryGrab(params object[] list)
        {
            if (player != null)
            {
                player.carryingState.Next();
                state.Switch(player);
            }
        }

        public void DropOnJump(params object[] list)
        {
            if (player == null) return;

            if (!state.IsCarry()) return;

            player.SetMaximumSpeed(5f);
            player.carryingState.Next();
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
            }
        }
    }
}