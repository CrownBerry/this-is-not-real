using System;
using System.Collections;
using UnityEngine;

namespace Transgression
{
    public class ShadowSlider : MonoBehaviour
    {
        private readonly float min = -1200f;
        private readonly float max = Screen.height;
        private RectTransform transform;

        private float goal;

        private void Awake()
        {
            transform = GetComponent<RectTransform>();
            goal = min;
            transform.anchoredPosition3D= new Vector3(transform.anchoredPosition3D.x,
                goal,
                transform.anchoredPosition3D.z);
        }

        private void OnEnable()
        {
            EventManager.StartListening("OnShadowSlide", SetNewGoal);
        }

        private void OnDisable()
        {
            EventManager.StopListening("OnShadowSlide", SetNewGoal);
        }

        private void OnDestroy()
        {
            EventManager.StopListening("OnShadowSlide", SetNewGoal);
        }

        private void Update()
        {
            var yPosition = transform.anchoredPosition3D.y;
            if (yPosition < goal)
            {
                if (yPosition - min < float.Epsilon)
                {
                    transform.anchoredPosition3D = new Vector3(transform.anchoredPosition3D.x,
                        min * 0.75f,
                        transform.anchoredPosition3D.z);
                }
                Slide(1);
            }
            else if (goal < yPosition)
            {
                if (max - yPosition < float.Epsilon)
                {
                    transform.anchoredPosition3D = new Vector3(transform.anchoredPosition3D.x,
                        max * 0.75f,
                        transform.anchoredPosition3D.z);
                }
                Slide(-1);
            }
        }

//        private void CallSliding(params object[] list)
//        {
//            var sliding = Sliding((bool) list[0]);
//            print(string.Format("Min is: {0}, and msx is: {1}", min, max));
//            StartCoroutine(sliding);
//        }

        private void SetNewGoal(params object[] list)
        {
            var direction = (bool) list[0];
            goal = direction ? max : min;
        }

        private void Slide(int direction)
        {
            var step = Time.deltaTime * (max - min) * 0.7f;
            transform.anchoredPosition3D = new Vector3(transform.anchoredPosition3D.x,
                transform.anchoredPosition3D.y + direction * step,
                transform.anchoredPosition3D.z);
            if (Mathf.Abs(goal - transform.anchoredPosition3D.y) < step)
            {
                transform.anchoredPosition3D = new Vector3(transform.anchoredPosition3D.x,
                    goal,
                    transform.anchoredPosition3D.z);
            }
        }

//        private IEnumerator Sliding(bool fromLab)
//        {
//            var goal = fromLab ? min : max;
//            var dir = fromLab ? -1f : 1f;
//            var step = 100 + (Time.deltaTime * (max-min) + min );
//            print(string.Format("Shadow goal is: {0}, and direction is: {1}", goal, dir));
//            while (transform.position.y < max && transform.position.y > min)
//            {
//                var step = -min + (Time.deltaTime * (max-min) + min );
//                print(string.Format("New pos is: {0}", transform.position.y + dir * step));
//                transform.position = new Vector3(transform.position.x,
//                    transform.position.y + dir * step,
//                    transform.position.z);
//                yield return null;
//            }
//            transform.position = new Vector3(transform.position.x, goal, transform.position.z);
//            print(string.Format("goal was: {0}, set position to: {1}", goal, transform.position.y));
//            yield break;
//        }
    }
}