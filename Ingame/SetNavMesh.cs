using NavMeshPlus.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetNavMesh : MonoBehaviour
{
    private NavMeshSurface navMeshSurface;
    public static SetNavMesh instance;

    void Start()
    {
        navMeshSurface = GetComponent<NavMeshSurface>();
        if (instance == null)
            instance = this;
    }
    public void Build()
    {
        navMeshSurface.BuildNavMesh();
    }
}
