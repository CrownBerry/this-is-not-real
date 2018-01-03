using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

public class AlignWindow : EditorWindow
{

    private Vector3 totalSize, localSize;
    private float offset = 0;
    private Vector3 pivot, pivotLast;
    private Vector3 rotation;


    [MenuItem("Window/Alig and Distribute Tools")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        AlignWindow alignWindow =
            (AlignWindow) EditorWindow.GetWindow(typeof (AlignWindow), false, "Align and distribute tool");
        Utils.CenterWindows(alignWindow, 263, 376);
    }

    void OnGUI()
    {
        GUILayout.Label("Alig and Distribute:", EditorStyles.boldLabel);
        GUILayout.BeginVertical("box");
        EditorGUILayout.LabelField(" Total Width: ", totalSize.x.ToString());
        EditorGUILayout.LabelField(" Total Height: ", totalSize.y.ToString());
        EditorGUILayout.LabelField(" Total Depth: ", totalSize.z.ToString());
        offset = EditorGUILayout.FloatField("Offset: ", offset);
        EditorGUILayout.Space();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(new GUIContent((Texture) Resources.Load("AlignButtons/XLowest"), "Horizontal Align Left"),
            GUILayout.Height(32), GUILayout.Width(32))) AlignLowest(0);
        if (GUILayout.Button(
            new GUIContent((Texture) Resources.Load("AlignButtons/XMiddle"), "Horizontal Align Center"),
            GUILayout.Height(32), GUILayout.Width(32))) AlignMiddle(0);
        if (GUILayout.Button(
            new GUIContent((Texture) Resources.Load("AlignButtons/XHighest"), "Horizontal Align Right"),
            GUILayout.Height(32), GUILayout.Width(32))) AlignHighest(0);
        if (
            GUILayout.Button(
                new GUIContent((Texture) Resources.Load("AlignButtons/XDLowest"), "Horizontal  Distribute Left"),
                GUILayout.Height(32), GUILayout.Width(32))) DistributeLowest(0);
        if (
            GUILayout.Button(
                new GUIContent((Texture) Resources.Load("AlignButtons/XDMiddle"), "Horizontal  Distribute Center"),
                GUILayout.Height(32), GUILayout.Width(32))) DistributeMiddle(0);
        if (
            GUILayout.Button(
                new GUIContent((Texture) Resources.Load("AlignButtons/XDHighest"), "Horizontal  Distribute Right"),
                GUILayout.Height(32), GUILayout.Width(32))) DistributeHighest(0);
        if (
            GUILayout.Button(
                new GUIContent((Texture) Resources.Load("AlignButtons/XDBetweenEdge"),
                    "Horizontal  Distribute Between Edge"), GUILayout.Height(32), GUILayout.Width(32)))
            DistributeBetweenEdge(0);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button(new GUIContent((Texture) Resources.Load("AlignButtons/YLowest"), "Vertical Align Bottom"),
            GUILayout.Height(32), GUILayout.Width(32))) AlignLowest(1);
        if (GUILayout.Button(new GUIContent((Texture) Resources.Load("AlignButtons/YMiddle"), "Vertical Align Center"),
            GUILayout.Height(32), GUILayout.Width(32))) AlignMiddle(1);
        if (GUILayout.Button(new GUIContent((Texture) Resources.Load("AlignButtons/YHighest"), "Vertical Align Up"),
            GUILayout.Height(32), GUILayout.Width(32))) AlignHighest(1);
        if (
            GUILayout.Button(
                new GUIContent((Texture) Resources.Load("AlignButtons/YDLowest"), "Vertical Distribute Bottom"),
                GUILayout.Height(32), GUILayout.Width(32))) DistributeLowest(1);
        if (
            GUILayout.Button(
                new GUIContent((Texture) Resources.Load("AlignButtons/YDMiddle"), "Vertical Distribute Center"),
                GUILayout.Height(32), GUILayout.Width(32))) DistributeMiddle(1);
        if (
            GUILayout.Button(
                new GUIContent((Texture) Resources.Load("AlignButtons/YDHighest"), "Vertical Distribute Up"),
                GUILayout.Height(32), GUILayout.Width(32))) DistributeHighest(1);
        if (
            GUILayout.Button(
                new GUIContent((Texture) Resources.Load("AlignButtons/YDBetweenEdge"),
                    "Vertical Distribute Between Edge"), GUILayout.Height(32), GUILayout.Width(32)))
            DistributeBetweenEdge(1);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button(new GUIContent((Texture) Resources.Load("AlignButtons/ZLowest"), "Lateral Align Front"),
            GUILayout.Height(32), GUILayout.Width(32))) AlignLowest(2);
        if (GUILayout.Button(new GUIContent((Texture) Resources.Load("AlignButtons/ZMiddle"), "Lateral Align Center"),
            GUILayout.Height(32), GUILayout.Width(32))) AlignMiddle(2);
        if (GUILayout.Button(new GUIContent((Texture) Resources.Load("AlignButtons/ZHighest"), "Lateral Align Back"),
            GUILayout.Height(32), GUILayout.Width(32))) AlignHighest(2);
        if (
            GUILayout.Button(
                new GUIContent((Texture) Resources.Load("AlignButtons/ZDLowest"), "Lateral Distribute Front"),
                GUILayout.Height(32), GUILayout.Width(32))) DistributeLowest(2);
        if (
            GUILayout.Button(
                new GUIContent((Texture) Resources.Load("AlignButtons/ZDMiddle"), "Lateral Distribute Center"),
                GUILayout.Height(32), GUILayout.Width(32))) DistributeMiddle(2);
        if (
            GUILayout.Button(
                new GUIContent((Texture) Resources.Load("AlignButtons/ZDHighest"), "Lateral Distribute Back"),
                GUILayout.Height(32), GUILayout.Width(32))) DistributeHighest(2);
        if (
            GUILayout.Button(
                new GUIContent((Texture) Resources.Load("AlignButtons/ZDBetweenEdge"), "Lateral Distribute Between Edge"),
                GUILayout.Height(32), GUILayout.Width(32))) DistributeBetweenEdge(2);
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();

        if (Selection.gameObjects.Length == 1)
        {
            GUILayout.Label("Pivot:", EditorStyles.boldLabel);
            GUILayout.BeginVertical("box");
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("X", GUILayout.MaxWidth(14));
            pivot.x = EditorGUILayout.Slider(pivot.x, -localSize.x / 2, localSize.x / 2);
            if (GUILayout.Button("Center", GUILayout.Width(55))) pivot.x = 0F;
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Y", GUILayout.MaxWidth(14));
            pivot.y = EditorGUILayout.Slider(pivot.y, -localSize.y / 2, localSize.y / 2);
            if (GUILayout.Button("Center", GUILayout.Width(55))) pivot.y = 0F;
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Z", GUILayout.MaxWidth(14));
            pivot.z = EditorGUILayout.Slider(pivot.z, -localSize.z / 2, localSize.z / 2);
            if (GUILayout.Button("Center", GUILayout.Width(55))) pivot.z = 0F;
            EditorGUILayout.EndHorizontal();
            pivot = EditorGUILayout.Vector3Field("", pivot);
            if (GUILayout.Button("Center Pivot")) pivot = Vector3.zero;
            GUILayout.EndVertical();
            
        }

        if (GUI.changed)
        {
            if (rotation != Selection.activeGameObject.transform.eulerAngles)GetPivotParams();
            if (pivot != pivotLast)
            {
                
                Utils.ChangePivot(Selection.activeGameObject, pivot, pivotLast);
                localSize = Utils.CalculateLocalSize(Selection.gameObjects);
                pivotLast = pivot;
            }
        }
    }

    void OnSelectionChange()
    {
        totalSize = Utils.CalculateSize(Selection.gameObjects);
        if (Selection.gameObjects.Length == 1) GetPivotParams();
        Repaint();
    }

    private void GetPivotParams()
    {
        localSize = Utils.CalculateLocalSize(Selection.gameObjects);
        pivot = pivotLast = Vector3.zero;
        rotation = Selection.activeGameObject.transform.eulerAngles;
        pivot = pivotLast = Utils.GetPivot(Selection.activeGameObject);
       
        Repaint();
    }

    private void SetPivotCenter()
    {
        List<GameObject> objList = new List<GameObject>();
        foreach (GameObject obj in Selection.gameObjects)
            if (Utils.CheckRenderer(obj)) objList.Add(obj);
        if (objList.Count < 1)
        {
            EditorUtility.DisplayDialog("Warning", "Select one or more game objects including renderer component.", "OK");
            return;
        }
        foreach (GameObject obj in objList)
        {
            Utils.SetPivot(obj);
        }
    }

    private void AlignLowest(int axis)
    {
        List<float> positionsList = new List<float>();
        foreach (GameObject obj in Selection.gameObjects)
        {
            if (Utils.CheckRenderer(obj))
                positionsList.Add(obj.transform.position[axis] - Utils.SizeObject(obj)[axis]/2);
        }
        if (positionsList.Count < 2)
        {
            EditorUtility.DisplayDialog("Warning", "Select two or more game objects including renderer component.", "OK");
            return;
        }
        foreach (GameObject obj in Selection.gameObjects)
            Utils.ChangePosition(obj.transform, axis,
                positionsList.Min() + Utils.SizeObject(obj)[axis]/2);
    }

    private void AlignMiddle(int axis)
    {
        List<float> positionsList = new List<float>();
        foreach (GameObject obj in Selection.gameObjects)
        {
            if (Utils.CheckRenderer(obj))
                positionsList.Add(obj.transform.position[axis]);
        }
        if (positionsList.Count < 2)
        {
            EditorUtility.DisplayDialog("Warning", "Select two or more game objects including renderer component.", "OK");
            return;
        }
        foreach (GameObject obj in Selection.gameObjects)
            Utils.ChangePosition(obj.transform, axis,
                positionsList.Average());
    }

    private void AlignHighest(int axis)
    {
        List<float> positionsList = new List<float>();
        foreach (GameObject obj in Selection.gameObjects)
        {
            if (Utils.CheckRenderer(obj))
                positionsList.Add(obj.transform.position[axis] + Utils.SizeObject(obj)[axis]/2);
        }
        if (positionsList.Count < 2)
        {
            EditorUtility.DisplayDialog("Warning", "Select two or more game objects including renderer component.", "OK");
            return;
        }
        foreach (GameObject obj in Selection.gameObjects)
            Utils.ChangePosition(obj.transform, axis,
                positionsList.Max() - Utils.SizeObject(obj)[axis]/2);
    }

    private void DistributeLowest(int axis)
    {
        List<GameObject> objList = new List<GameObject>();
        float totalSize = 0;
        foreach (GameObject obj in Selection.gameObjects)
            if (Utils.CheckRenderer(obj))
            {
                objList.Add(obj);
                totalSize += Utils.SizeObject(obj)[axis];
            }
        if (objList.Count < 2)
        {
            EditorUtility.DisplayDialog("Warning", "Select two or more game objects including renderer component.", "OK");
            return;
        }
        float minCoordinate = objList.Min(go => go.transform.position[axis] - Utils.SizeObject(go.gameObject)[axis]/2);
        objList.Sort((g1, g2) => (g1.transform.position[axis] >= g2.transform.position[axis]) ? 1 : -1);
        float currentPosition = minCoordinate;
        foreach (GameObject obj in objList)
        {
            Utils.ChangePosition(obj.transform, axis,
                currentPosition + Utils.SizeObject(obj)[axis]/2);
            currentPosition = currentPosition + offset + Utils.SizeObject(obj)[axis];
        }
    }

    private void DistributeMiddle(int axis)
    {
        List<GameObject> objList = new List<GameObject>();
        float totalSize = 0;
        foreach (GameObject obj in Selection.gameObjects)
            if (Utils.CheckRenderer(obj))
            {
                objList.Add(obj);
                totalSize += Utils.SizeObject(obj)[axis];
            }
        if (objList.Count < 2)
        {
            EditorUtility.DisplayDialog("Warning", "Select two or more game objects including renderer component.", "OK");
            return;
        }
        float minCoordinate = objList.Min(go => go.transform.position[axis] - Utils.SizeObject(go.gameObject)[axis]/2);
        float maxCoordinate = objList.Max(go => go.transform.position[axis] + Utils.SizeObject(go.gameObject)[axis]/2);
        objList.Sort((g1, g2) => (g1.transform.position[axis] >= g2.transform.position[axis]) ? 1 : -1);
        float currentPosition = (minCoordinate + maxCoordinate)/2 - (totalSize + offset*(objList.Count - 1))/2;
        foreach (GameObject obj in objList)
        {
            Utils.ChangePosition(obj.transform, axis,
                currentPosition + Utils.SizeObject(obj)[axis]/2);
            currentPosition = currentPosition + offset + Utils.SizeObject(obj)[axis];
        }
    }

    private void DistributeHighest(int axis)
    {
        List<GameObject> objList = new List<GameObject>();
        float totalSize = 0;
        foreach (GameObject obj in Selection.gameObjects)
            if (Utils.CheckRenderer(obj))
            {
                objList.Add(obj);
                totalSize += Utils.SizeObject(obj)[axis];
            }
        if (objList.Count < 2)
        {
            EditorUtility.DisplayDialog("Warning", "Select two or more game objects including renderer component.", "OK");
            return;
        }
        float maxCoordinate = objList.Max(go => go.transform.position[axis] + Utils.SizeObject(go.gameObject)[axis]/2);
        objList.Sort((g1, g2) => (g1.transform.position[axis] <= g2.transform.position[axis]) ? 1 : -1);
        float currentPosition = maxCoordinate;
        foreach (GameObject obj in objList)
        {
            Utils.ChangePosition(obj.transform, axis,
                currentPosition - Utils.SizeObject(obj)[axis]/2);
            currentPosition = currentPosition - offset - Utils.SizeObject(obj)[axis];
        }
    }

    private void DistributeBetweenEdge(int axis)
    {
        List<GameObject> objList = new List<GameObject>();
        float totalSize = 0;
        foreach (GameObject obj in Selection.gameObjects)
            if (Utils.CheckRenderer(obj))
            {
                objList.Add(obj);
                totalSize += Utils.SizeObject(obj)[axis];
            }
        if (objList.Count < 2)
        {
            EditorUtility.DisplayDialog("Warning", "Select two or more game objects including renderer component.", "OK");
            return;
        }
        float minCoordinateEdgeObjects, maxCoordinateEdgeObjects;
        GameObject minObj, maxObj;
        minObj = maxObj = objList[0];
        foreach (GameObject obj in objList)
        {
            if (obj.transform.position[axis] - Utils.SizeObject(obj.gameObject)[axis]/2 <
                minObj.transform.position[axis] - Utils.SizeObject(minObj.gameObject)[axis]/2) minObj = obj;
            if (obj.transform.position[axis] + Utils.SizeObject(obj.gameObject)[axis] / 2 >
                maxObj.transform.position[axis] + Utils.SizeObject(maxObj.gameObject)[axis] / 2) maxObj = obj;
        }

        objList.Remove(minObj);
        objList.Remove(maxObj);
        
        minCoordinateEdgeObjects = minObj.transform.position[axis] + Utils.SizeObject(minObj.gameObject)[axis]/2;
        maxCoordinateEdgeObjects = maxObj.transform.position[axis] - Utils.SizeObject(maxObj.gameObject)[axis]/2;

        float length = maxCoordinateEdgeObjects - minCoordinateEdgeObjects;
        Vector3 _totalSize=Vector3.zero;
        float _offset, currentPosition;
        foreach (GameObject obj in objList)
            if (Utils.CheckRenderer(obj)) _totalSize += Utils.SizeObject(obj);

        if (_totalSize[axis] > length)
        {
            _offset = 0;
            currentPosition = minCoordinateEdgeObjects - (_totalSize[axis] - length) / 2;
        }
        else
        {
            _offset = (length - _totalSize[axis])/(objList.Count + 1);
            currentPosition = minCoordinateEdgeObjects + _offset;
        }
        objList.Sort((g1, g2) => (g1.transform.position[axis] >= g2.transform.position[axis]) ? 1 : -1);

        foreach (GameObject obj in objList)
        {
            Utils.ChangePosition(obj.transform, axis,
                currentPosition + Utils.SizeObject(obj)[axis] / 2);
            currentPosition = currentPosition + _offset + Utils.SizeObject(obj)[axis];
        }
    }
}
