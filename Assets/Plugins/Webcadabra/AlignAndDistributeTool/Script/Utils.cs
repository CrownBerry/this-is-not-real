using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UnityEditor;

public class Utils{

    public static Vector3 SizeObject(GameObject obj)
    {
        List<Transform> objList = new List<Transform>();
        foreach (Renderer child in obj.GetComponentsInChildren<Renderer>()) objList.Add(child.transform);
        if (objList.Count == 0) return Vector3.zero;
        float maxCoordinateX = objList.Max(go => go.transform.position.x + go.GetComponent<Renderer>().bounds.size.x / 2);
        float maxCoordinateY = objList.Max(go => go.transform.position.y + go.GetComponent<Renderer>().bounds.size.y / 2);
        float maxCoordinateZ = objList.Max(go => go.transform.position.z + go.GetComponent<Renderer>().bounds.size.z / 2);
        float minCoordinateX = objList.Min(go => go.transform.position.x - go.GetComponent<Renderer>().bounds.size.x / 2);
        float minCoordinateY = objList.Min(go => go.transform.position.y - go.GetComponent<Renderer>().bounds.size.y / 2);
        float minCoordinateZ = objList.Min(go => go.transform.position.z - go.GetComponent<Renderer>().bounds.size.z / 2);
        Vector3 maxCoordinate = new Vector3(maxCoordinateX, maxCoordinateY, maxCoordinateZ);
        Vector3 minCoordinate = new Vector3(minCoordinateX, minCoordinateY, minCoordinateZ);
        return maxCoordinate-minCoordinate;
    }

    public static Vector3 LocalSizeObject(GameObject obj)
    {
        List<Transform> objList = new List<Transform>();
        foreach (Renderer child in obj.GetComponentsInChildren<Renderer>()) objList.Add(child.transform);
        float maxCoordinateX = objList.Max(go => go.transform.localPosition.x + go.GetComponent<MeshFilter>().sharedMesh.bounds.size.x / 2);
        float maxCoordinateY = objList.Max(go => go.transform.localPosition.y + go.GetComponent<MeshFilter>().sharedMesh.bounds.size.y / 2);
        float maxCoordinateZ = objList.Max(go => go.transform.localPosition.z + go.GetComponent<MeshFilter>().sharedMesh.bounds.size.z / 2);
        float minCoordinateX = objList.Min(go => go.transform.localPosition.x - go.GetComponent<MeshFilter>().sharedMesh.bounds.size.x / 2);
        float minCoordinateY = objList.Min(go => go.transform.localPosition.y - go.GetComponent<MeshFilter>().sharedMesh.bounds.size.y / 2);
        float minCoordinateZ = objList.Min(go => go.transform.localPosition.z - go.GetComponent<MeshFilter>().sharedMesh.bounds.size.z / 2);
        Vector3 maxCoordinate = new Vector3(maxCoordinateX, maxCoordinateY, maxCoordinateZ);
        Vector3 minCoordinate = new Vector3(minCoordinateX, minCoordinateY, minCoordinateZ);
        Vector3 size = maxCoordinate - minCoordinate;
        return size;
    }

    public static void SetPivot(GameObject obj, Vector3 pivot = default(Vector3), Vector3 pivotLast = default(Vector3))
    {
        if (pivotLast == Vector3.zero) pivotLast = GetCenter(obj);
        List<Transform> fList = new List<Transform>();
        foreach (Renderer child in obj.GetComponentsInChildren<Renderer>()) fList.Add(child.transform);
        Vector3 correction = pivotLast- pivot;
        foreach (Transform child in fList) child.localPosition += correction;
        obj.transform.position -= Quaternion.Euler(obj.transform.eulerAngles) * correction;
    }

    public static Vector3 GetSize(GameObject obj)
    {
        List<Transform> objList = new List<Transform>();
        foreach (Renderer child in obj.GetComponentsInChildren<Renderer>()) objList.Add(child.transform);
        if (objList.Count == 0) return Vector3.zero;
        float maxCoordinateX = objList.Max(go => go.transform.localPosition.x + go.GetComponent<MeshFilter>().sharedMesh.bounds.size.x / 2);
        float maxCoordinateY = objList.Max(go => go.transform.localPosition.y + go.GetComponent<MeshFilter>().sharedMesh.bounds.size.y / 2);
        float maxCoordinateZ = objList.Max(go => go.transform.localPosition.z + go.GetComponent<MeshFilter>().sharedMesh.bounds.size.z / 2);
        float minCoordinateX = objList.Min(go => go.transform.localPosition.x - go.GetComponent<MeshFilter>().sharedMesh.bounds.size.x / 2);
        float minCoordinateY = objList.Min(go => go.transform.localPosition.y - go.GetComponent<MeshFilter>().sharedMesh.bounds.size.y / 2);
        float minCoordinateZ = objList.Min(go => go.transform.localPosition.z - go.GetComponent<MeshFilter>().sharedMesh.bounds.size.z / 2);
        if (!obj.GetComponent<Renderer>())
        {
            if (maxCoordinateX < 0) maxCoordinateX = 0;
            if (maxCoordinateY < 0) maxCoordinateY = 0;
            if (maxCoordinateZ < 0) maxCoordinateZ = 0;
            if (minCoordinateX > 0) minCoordinateX = 0;
            if (minCoordinateY > 0) minCoordinateY = 0;
            if (minCoordinateZ > 0) minCoordinateZ = 0;
        }
        Vector3 size = new Vector3(Mathf.Abs(maxCoordinateX - minCoordinateX),
            Mathf.Abs(maxCoordinateY - minCoordinateY), Mathf.Abs(maxCoordinateZ - minCoordinateZ));
        return size;
    }

    public static Vector3 GetCenter(GameObject obj)
    {
        Vector3 size = Utils.GetSize(obj);
        List<Transform> fList = new List<Transform>();
        foreach (Renderer child in obj.GetComponentsInChildren<Renderer>()) { fList.Add(child.transform);}
        if(fList.Count==0)return Vector3.zero;
        float maxCoordinateX = fList.Max(go => go.transform.localPosition.x + go.GetComponent<MeshFilter>().sharedMesh.bounds.size.x / 2);
        float maxCoordinateY = fList.Max(go => go.transform.localPosition.y + go.GetComponent<MeshFilter>().sharedMesh.bounds.size.y / 2);
        float maxCoordinateZ = fList.Max(go => go.transform.localPosition.z + go.GetComponent<MeshFilter>().sharedMesh.bounds.size.z / 2);
        Vector3 maxCoordinate = new Vector3(maxCoordinateX, maxCoordinateY, maxCoordinateZ);
        Vector3 center = size/2- maxCoordinate;
        return center;
    }

    public static bool CheckRenderer(GameObject obj)
    {
        bool value = false;
        if(obj)if (obj.GetComponentsInChildren<Renderer>().Length > 0) value = true;
        return value;
    }

    public static void ChangePosition(Transform transform,int axis, float value)
    {
        if (axis == 0) transform.position = new Vector3(value, transform.position.y, transform.position.z);
        if (axis == 1) transform.position = new Vector3(transform.position.x, value, transform.position.z);
        if (axis == 2) transform.position = new Vector3(transform.position.x, transform.position.y, value);
    }

    public static void ChangeLocalPosition(Transform transform, int axis, float value)
    {
        if (axis == 0) transform.localPosition = new Vector3(value, transform.localPosition.y, transform.localPosition.z);
        if (axis == 1) transform.localPosition = new Vector3(transform.localPosition.x, value, transform.localPosition.z);
        if (axis == 2) transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, value);
    }

    public static void CenterWindows(EditorWindow windows, int width, int heigh)
    {
        windows.position = new Rect((Screen.currentResolution.width - width) / 2, (Screen.currentResolution.height - heigh) / 2, width, heigh);
    }

    public static Vector3 CalculateSize(GameObject[] objects)
    {
        Vector3 _totalSize = Vector3.zero;
        foreach (GameObject obj in objects)
            if (Utils.CheckRenderer(obj)) _totalSize += Utils.SizeObject(obj);
        return _totalSize;
    }

    public static Vector3 CalculateLocalSize(GameObject[] objects)
    {
        Vector3 size = Vector3.zero;
        foreach (GameObject obj in objects)
            if (Utils.CheckRenderer(obj)) size += GetSize(obj);
        return size;
    }

    public static Vector3 GetPivot(GameObject obj)
    {
        Vector3 pivot=new Vector3();
        if (obj.GetComponent<MeshFilter>())
        {
            Bounds bounds = obj.GetComponent<MeshFilter>().sharedMesh.bounds;
            pivot = -1*bounds.center;
        }
        else
        {
            pivot = GetCenter(obj);
        }
        return pivot;
    }

    public static void ChangePivot(GameObject obj, Vector3 pivot,Vector3 pivotLast)
    {
        if (obj.GetComponent<MeshFilter>())
        {
            Mesh mesh = obj.GetComponent<MeshFilter>().sharedMesh;
            Vector3 correction = pivotLast - pivot;
            obj.transform.position -= Quaternion.Euler(obj.transform.eulerAngles) * Vector3.Scale(correction, obj.transform.localScale);
            Vector3[] verts = mesh.vertices;
            for (int i = 0; i < verts.Length; i++) verts[i] += correction;
            mesh.vertices = verts;
            mesh.RecalculateBounds();
            if (obj.GetComponent(typeof (Collider)))
            {
                Collider col = obj.GetComponent(typeof (Collider)) as Collider;
                if (col is BoxCollider) ((BoxCollider) col).center += correction;
                if (col is CapsuleCollider) ((CapsuleCollider) col).center += correction;
                if (col is SphereCollider) ((SphereCollider) col).center += correction;
            }
        }
        else
        {
            Utils.SetPivot(obj,pivot);
        }
    }

}
