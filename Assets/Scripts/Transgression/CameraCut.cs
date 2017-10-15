using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class CameraCut : MonoBehaviour
{
    private Camera _camera;
    public float goal;
    public Rect myRect;

    private float step;

    private float summary;

    private void Awake()
    {
        goal = 0.0f;
        _camera = GetComponent<Camera>();
        myRect = new Rect(0, 0, 1, 0);
        SetScissorRect(_camera, myRect);
        summary = 0f;
    }

    private void OnEnable()
    {
        EventManager.StartListening("OnViewportGoal", SetNewGoal);
    }

    private void OnDisable()
    {
        EventManager.StopListening("OnViewportGoal", SetNewGoal);
    }

    private void Update()
    {
        step = Time.deltaTime * 2.5f;
        if (myRect.height < goal)
        {
            if (goal - myRect.height > step + 0.01)
            {
                myRect.height += step;
            }
            else
            {
                myRect.height = 1;
                EventManager.TriggerEvent("EndTransgression");
            }
        }
        else if (myRect.height > goal)
        {
            if (myRect.height - goal > step + 0.01)
                myRect.height -= step;
            else
            {
                myRect.height = 0;
                SetScissorRect(_camera, myRect);
                EventManager.TriggerEvent("EndTransgression");
            }
        }
        if (myRect.height != 0)
        {
            SetScissorRect(_camera, myRect);
        }
    }

    private void SetNewGoal(params object[] list)
    {
        var newGoal = (float) list[0];
        goal = newGoal;
    }

    public static void SetScissorRect(Camera camera, Rect rect)
    {
        if (rect.x < 0)
        {
            rect.width += rect.x;
            rect.x = 0;
        }

        if (rect.y < 0)
        {
            rect.height += rect.y;
            rect.y = 0;
        }

        rect.width = Mathf.Min(1 - rect.x, rect.width);
        rect.height = Mathf.Min(1 - rect.y, rect.height);

        camera.rect = new Rect(0, 0, 1, 1);
        camera.ResetProjectionMatrix();
        var m = camera.projectionMatrix;
        camera.rect = rect;
        var m1 = Matrix4x4.TRS(new Vector3(rect.x, rect.y, 0), Quaternion.identity,
            new Vector3(rect.width, rect.height, 1));
        var m2 = Matrix4x4.TRS(new Vector3((1 / rect.width - 1), (1 / rect.height - 1), 0), Quaternion.identity,
            new Vector3(1 / rect.width, 1 / rect.height, 1));
        var m3 = Matrix4x4.TRS(new Vector3(-rect.x * 2 / rect.width, -rect.y * 2 / rect.height, 0), Quaternion.identity,
            Vector3.one);
        camera.projectionMatrix = m3 * m2 * m;
    }
}