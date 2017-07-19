using System;
using System.Collections;
using System.Security.Cryptography;
using UnityEngine;

namespace CameraScripts
{
    public class ShadowSlider : MonoBehaviour
    {
        private readonly float min = -150f;
        private readonly float max = Screen.height + 150f;

        private void OnEnable()
        {
            EventManager.StartListening("OnShadowSlide", CallSliding);
        }

        private void OnDisable()
        {
            EventManager.StopListening("OnShadowSlide", CallSliding);
        }

        private void OnDestroy()
        {
            EventManager.StopListening("OnShadowSlide", CallSliding);
        }

        private void CallSliding(params object[] list)
        {
            var sliding = Sliding((bool) list[0]);
            print(string.Format("Min is: {0}, and msx is: {1}", min, max));
            StartCoroutine(sliding);
        }

        private IEnumerator Sliding(bool fromLab)
        {
            var goal = fromLab ? min : max;
            var dir = fromLab ? -1f : 1f;
//            var step = 100 + (Time.deltaTime * (max-min) + min );
            print(string.Format("Shadow goal is: {0}, and direction is: {1}", goal, dir));
            while (transform.position.y < max && transform.position.y > min)
            {
                var step = -min + (Time.deltaTime * (max-min) + min );
                print(string.Format("New pos is: {0}", transform.position.y + dir * step));
                transform.position = new Vector3(transform.position.x,
                    transform.position.y + dir * step,
                    transform.position.z);
                yield return null;
            }
            transform.position = new Vector3(transform.position.x, goal, transform.position.z);
            print(string.Format("goal was: {0}, set position to: {1}", goal, transform.position.y));
            yield break;
        }
    }
}