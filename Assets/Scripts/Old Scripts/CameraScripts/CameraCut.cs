using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCut : MonoBehaviour {

    Camera cam;
    public float goal;
    public Rect myRect;

    private void Awake()
    {
        goal = 0.0f;
        cam = GetComponent<Camera>();
        myRect = new Rect(0, 0, 1, 0);
        SetScissorRect(cam, myRect);
        print(myRect.height);
    }

    void OnEnable()
    {
        EventManager.StartListening("OnViewportGoal", SetNewGoal);
    }

    void OnDisable()
    {
        EventManager.StopListening("OnViewportGoal", SetNewGoal);
    }

    void Update()
    {
        if (myRect.height < goal)
        {
            if (goal - myRect.height > 0.01)
                myRect.height += Time.deltaTime;
            else
            {
                myRect.height = 1;
                Debug.Log(goal-myRect.height>0.01);
                EventManager.TriggerEvent("OnFinishLineSliding");
            }
        }
        else if (myRect.height > goal)
        {
            if (myRect.height - goal > 0.01)
                myRect.height -= Time.deltaTime;
            else
            {
                myRect.height = 0;
                Debug.Log(myRect.height-goal>0.01);
                EventManager.TriggerEvent("OnFinishLineSliding");
            }
        }
        if (myRect.height != 0)
        {
            SetScissorRect(cam, myRect);
        }
    }

    void SetNewGoal(params object[] list)
    {
        var newGoal = (float)list [0];
        goal = newGoal;
        Debug.Log("New camera goal is " + newGoal.ToString());
    }

    public static void SetScissorRect(Camera cam, Rect r)
    {
        if (r.x < 0)
        {
            r.width += r.x;
            r.x = 0;
        }

        if (r.y < 0)
        {
            r.height += r.y;
            r.y = 0;
        }

        r.width = Mathf.Min(1 - r.x, r.width);
        r.height = Mathf.Min(1 - r.y, r.height);

        cam.rect = new Rect(0, 0, 1, 1);
        cam.ResetProjectionMatrix();
        Matrix4x4 m = cam.projectionMatrix;
        cam.rect = r;
        Matrix4x4 m1 = Matrix4x4.TRS(new Vector3(r.x, r.y, 0), Quaternion.identity, new Vector3(r.width, r.height, 1));
        Matrix4x4 m2 = Matrix4x4.TRS(new Vector3((1 / r.width - 1), (1 / r.height - 1), 0), Quaternion.identity, new Vector3(1 / r.width, 1 / r.height, 1));
        Matrix4x4 m3 = Matrix4x4.TRS(new Vector3(-r.x * 2 / r.width, -r.y * 2 / r.height, 0), Quaternion.identity, Vector3.one);
        cam.projectionMatrix = m3 * m2 * m;
    }
}
