using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//Shader_IrregularQuadrilateralsToTexture2D
public class Shader_WorldPointsToLocalPoints : MonoBehaviour
{

    public ComputeShader m_worldToLocalPointsShader;
    public Vector3 m_systemWorldPosition = Vector3.one*3;
    public Quaternion m_systemWorldRotation = Quaternion.identity;
    public List<Vector3> m_worldPoints;
    public Vector3 [] m_worldPointsArray;
    public Vector3 [] m_localPointsArray;
    public void Update()
    {
        ComputeInformation();
    }

    [ContextMenu("SetRotationForward")]
    public void SetRotationForward() => Quaternion.LookRotation(Vector3.forward);

    [ContextMenu("SetRotationForward")]
    public void SetCoordinateSystem(Vector3 worldPoint, Quaternion direction) {
        m_systemWorldRotation = direction;
        m_systemWorldPosition = worldPoint;
    }
    public void SetPoints(in IEnumerable<Vector3> points)
    {
        m_worldPoints = points.ToList();
    }

    [ContextMenu("Compute Given Data")]
    public void ComputeInformation()
    {
        if (m_worldPoints == null || m_worldPoints.Count <= 0)
            return;
        int kernel = m_worldToLocalPointsShader.FindKernel("CSMain");

        int moduloOfGroup = m_worldPoints.Count % 8;
        if (moduloOfGroup != 0) {
            for (int i = 0; i < (8- moduloOfGroup); i++)
            {
                m_worldPoints.Add(Vector3.zero);
            }
        }
        if (m_worldPointsArray.Length != m_worldPoints.Count) { 
            m_worldPointsArray = m_worldPoints.ToArray();
            m_localPointsArray = new Vector3[m_worldPointsArray.Length];
        }
        ComputeBuffer bufferSet = new ComputeBuffer(m_worldPoints.Count, sizeof(float) * 3);
        bufferSet.SetData(m_worldPoints);
        ComputeBuffer bufferGet = new ComputeBuffer(m_worldPoints.Count, sizeof(float) * 3);
        bufferGet.SetData(m_localPointsArray);

        m_worldToLocalPointsShader.SetBuffer(kernel, "m_worldPoint", bufferSet);
        m_worldToLocalPointsShader.SetBuffer(kernel, "m_localPoint", bufferGet);


        Quaternion systemWorldRotationInverse =Quaternion.Inverse( m_systemWorldRotation);
        m_worldToLocalPointsShader.SetFloats("m_systemPosition",
            m_systemWorldPosition.x, m_systemWorldPosition.y, m_systemWorldPosition.z);
        m_worldToLocalPointsShader.SetFloats("m_inverseSystemRotation",
            systemWorldRotationInverse.x,
            systemWorldRotationInverse.y,
            systemWorldRotationInverse.z, 
            systemWorldRotationInverse.w);

        m_worldToLocalPointsShader.Dispatch(kernel, m_worldPoints.Count / 8, 1, 1);
        bufferGet.GetData(m_localPointsArray);

        bufferSet.Dispose();
        bufferGet.Dispose();
    }
}
