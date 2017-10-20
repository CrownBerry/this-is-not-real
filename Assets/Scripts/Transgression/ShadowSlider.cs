using UnityEngine;
using UnityEngine.UI;

namespace Transgression
{
    public class ShadowSlider : MonoBehaviour
    {
        private float goal;
        private float max = Screen.height + 250f;
        private float maxStart;
        private float min = -250f;
        private float minStart;
        private float realHeight;

        private float summary;
        private RectTransform transform;

        private void Awake()
        {
            var canvas = FindObjectOfType<Canvas>().GetComponent<CanvasScaler>();

            realHeight = canvas.referenceResolution.y;

            minStart = 0f;
            maxStart = realHeight;
            min = -250f;
            max = realHeight + 250f;

            transform = GetComponent<RectTransform>();
            goal = min;
            transform.anchoredPosition3D = new Vector3(transform.anchoredPosition3D.x,
                goal,
                transform.anchoredPosition3D.z);
            summary = 0f;
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
                if (Mathf.Abs(yPosition - minStart) < 20)
                    EventManager.TriggerEvent("OnViewportGoal", 1f);
                Slide(1);
            }
            else if (goal < yPosition)
            {
                if (Mathf.Abs(yPosition - maxStart) < 20)
                    EventManager.TriggerEvent("OnViewportGoal", 0f);
                Slide(-1);
            }
        }

        private void SetNewGoal(params object[] list)
        {
            var direction = (bool) list[0];
            goal = direction ? max : min;
        }

        private void Slide(int direction)
        {
            var step = Time.deltaTime * (maxStart - minStart) * 2.5f;
            transform.anchoredPosition3D = new Vector3(transform.anchoredPosition3D.x,
                transform.anchoredPosition3D.y + direction * step,
                transform.anchoredPosition3D.z);

            if (Mathf.Abs(goal - transform.anchoredPosition3D.y) < step)
            {
                transform.anchoredPosition3D = new Vector3(transform.anchoredPosition3D.x,
                    goal,
                    transform.anchoredPosition3D.z);
                EventManager.TriggerEvent("EndTransgression");
            }
//            }
        }
    }
}