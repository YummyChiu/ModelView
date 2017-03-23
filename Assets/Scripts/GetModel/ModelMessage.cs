using UnityEngine;
using System.Collections;

public class ModelMessage{
    public int Id { get; set; }
    public string Name { get; set; }
    public int[] Triangles { get; set; }
    public Vector3[] Vertices { get; set; }
    public Vector3[] Normals { get; set; }

    
}
