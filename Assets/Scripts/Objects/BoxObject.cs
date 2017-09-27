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
        private Vector3 D;


        private void Awake()
        {
            state = new BoxStateMachine(this, BoxStateMachine.State.None);
            rigidbody = GetComponent<Rigidbody>();
            D = new Vector3();
        }

        private void OnEnable()
        {
            EventManager.StartListening("Interact", TryGrab);
            EventManager.StartListening("Jump", DropOnJump);
        }

        private void OnDisable()
        {
            EventManager.StopListening("Interact", TryGrab);
            EventManager.StopListening("Jump", DropOnJump);
        }

        private void OnDestroy()
        {
            EventManager.StopListening("Interact", TryGrab);
            EventManager.StopListening("Jump", DropOnJump);
        }

        private void TryGrab(params object[] list)
        {
            if (player != null)
            {
                state.Switch(player);
            }
        }

        public void DropOnJump(params object[] list)
        {
            if (player == null) return;

            player.SetMaximumSpeed(5f);
            state.Drop();
        }

        private void Update()
        {
            if (player == null) return;

            D = player.transform.position - transform.position;
            var dist = D.magnitude;
            var pullDir = D.normalized;

            if (dist > 5)
            {
                player.SetMaximumSpeed(5f);
                player = null;
                state.Drop();
            }
            else if (dist > 0.3)
            {
                var pullF = 10f;
                var pullForDist = (dist - 0.3f) / 2f;
                if (pullForDist > 20f)
                {
                    pullForDist = 20f;
                }
                pullF += pullForDist;
                state.Move(pullDir, pullF);
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