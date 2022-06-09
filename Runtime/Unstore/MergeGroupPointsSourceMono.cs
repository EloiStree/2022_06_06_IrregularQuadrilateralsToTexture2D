using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MergeGroupPointsSourceMono : ContainMassGroupOfVector3Mono
{

    public List<ContainMassGroupOfVector3Mono> m_vector3Groups = new List<ContainMassGroupOfVector3Mono>();
    public Vector3 [] m_pointsMerge = new Vector3[0];
    public override void GetVector3Ref(out Vector3[] points)
    {
        List<Vector3> merged = new List<Vector3>();
        merged.Clear();
        for (int i = 0; i < m_vector3Groups.Count; i++)
        {
            m_vector3Groups[i].GetVector3Ref(out Vector3[] p);
            merged.AddRange(p);
        }
        m_pointsMerge = merged.ToArray();
        points = m_pointsMerge;
    }
}
