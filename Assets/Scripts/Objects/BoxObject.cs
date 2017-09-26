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
        [CanBeNull] public PlayerClass player;
        public Vector3 D;
        public Vector3 boxVelocity;
        public float force;


        private void Awake()
        {
            state = new BoxStateMachine(this, BoxStateMachine.State.None);
            rigidbody = GetComponent<Rigidbody>();
            D = new Vector3();
            boxVelocity = rigidbody.velocity;
        }

        private void OnEnable()
        {
            EventManager.StartListening("Interact", TryGrab);
        }

        private void OnDisable()
        {
            EventManager.StopListening("Interact", TryGrab);
        }

        private void OnDestroy()
        {
            EventManager.StopListening("Interact", TryGrab);
        }

        private void TryGrab(params object[] list)
        {
            if (player != null)
            {
                state.Switch();
            }
        }

        private void Update()
        {
            boxVelocity = rigidbody.velocity;
            force = 0f;
            if (player == null) return;

            D = player.transform.position - transform.position;
            var dist = D.magnitude;
            var pullDir = D.normalized;

            if (dist > 5)
            {
                player = null;
                state.Drop();
            }
            else if (dist > 0.3)
            {
                var pullF = 50f;
                var pullForDist = (dist - 0.3f);
                if (pullForDist > 70f)
                {
                    pullForDist = 70f;
                }
                pullF += pullForDist;
                state.Move(pullDir, pullF);
                force = pullF;
            }
        }

        public void Move(Vector3 pullDir, float pullF)
        {
            rigidbody.AddForce(pullDir * (pullF * Time.deltaTime), ForceMode.Acceleration);
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