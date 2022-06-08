using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Experiment_DrawRelocatedPoint : MonoBehaviour
{
    public IrregularQuadrilateralsMono m_quadTarget;
    public WorldToLocalRelocationOfPointsMono m_relocator;
    public ContainMassGroupOfVector3Mono [] m_massPoints;


    public void Update()
    {
        m_relocator.SetOrigine(m_quadTarget.m_origineSystemOrientation, m_quadTarget.m_origineSystemPoint);
        for (int i = 0; i < m_massPoints.Length; i++)
        {
            m_massPoints[i].GetVector3Ref(out Vector3[] points);
            m_relocator.SetPoints(in points);
            m_relocator.ComputeInfo(DrawPoints);
        }


    }

    public   Color m_drawColor = Color.white;
    public int m_drawMax = 10000;
    private void DrawPoints()
    {
        m_relocator.GetRelocatedPoint(out Vector3[] points);
        Vector3 flatPoint = Vector3.zero;
        float td = Time.deltaTime;
        for (int i = 0; i < points.Length && i< m_drawMax; i++)
        {
            flatPoint = points[i];
            flatPoint.y = 0;
            Debug.DrawLine(flatPoint, points[i], m_drawColor,td );
        }
    }
}
