using System;
using UnityEngine;
using UnityEngine.UI;

namespace Transgression
{
    public enum SliderState
    {
        Idle,
        Sliding
    }

    public class ShadowSlider : MonoBehaviour
    {
        private float goal;
        private float max = Screen.height + 250f;
        private float maxStart;
        private float min = -250f;
        private float minStart;
        private float realHeight;

        private float summary;
        private RectTransform rectTransform;

        private SliderState state;

        private void Awake()
        {
            var canvas = FindObjectOfType<Canvas>().GetComponent<CanvasScaler>();

            realHeight = canvas.referenceResolution.y;

            minStart = 0f;
            maxStart = realHeight;
            min = -250f;
            max = realHeight + 250f;

            rectTransform = GetComponent<RectTransform>();
            goal = min;
            rectTransform.anchoredPosition3D = new Vector3(rectTransform.anchoredPosition3D.x,
                goal,
                rectTransform.anchoredPosition3D.z);
            summary = 0f;

            state = SliderState.Idle;
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
            var yPosition = rectTransform.anchoredPosition3D.y;

            if (yPosition < goal)
            {
                if (Mathf.Abs(yPosition - minStart) < 20
                    && state == SliderState.Idle)
                {
                    EventManager.TriggerEvent("OnViewportGoal", 1f);
                    state = SliderState.Sliding;
                }
                Slide(1);
            }
            else if (goal < yPosition)
            {
                if (Mathf.Abs(yPosition - maxStart) < 20
                    && state == SliderState.Idle)
                {
                    EventManager.TriggerEvent("OnViewportGoal", 0f);
                    state = SliderState.Sliding;
                }
                Slide(-1);
            }
        }

        private void SetNewGoal(params object[] list)
        {
            var direction = (bool) list[0];
            goal = direction ? max : min;
            var viewportGoal = Math.Abs(goal - max) < float.Epsilon ? 1f : 0f;
        }

        private void Slide(int direction)
        {
            var step = Time.deltaTime * (maxStart - minStart) * 2.5f;
            rectTransform.anchoredPosition3D = new Vector3(rectTransform.anchoredPosition3D.x,
                rectTransform.anchoredPosition3D.y + direction * step,
                rectTransform.anchoredPosition3D.z);

            if (Mathf.Abs(goal - rectTransform.anchoredPosition3D.y) < step)
            {
                rectTransform.anchoredPosition3D = new Vector3(rectTransform.anchoredPosition3D.x,
                    goal,
                    rectTransform.anchoredPosition3D.z);
                EventManager.TriggerEvent("EndTransgression");
                state = SliderState.Idle;
            }
//            }
        }
    }
}