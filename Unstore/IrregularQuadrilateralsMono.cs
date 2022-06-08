using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IrregularQuadrilateralsMono : MonoBehaviour
{

    public IrregularQuadrilateralsTansform m_pointsRef;
    public IrregularQuadrilateralsVector3 m_pointsPosition;
    public IrregularQuadrilateralsVector3 m_pointsRelocatedAtZero;
    public IrregularQuadrilateralsVector3 m_pointsRelocatedAtOnDLTL;
    public IrregularQuadrilateralsVector3 m_pointsRelocatedAtOnDLTLFlat;

    public Quaternion m_origineSystemOrientation;
    public Vector3 m_origineSystemPoint;

    public Color m_border;
    public Color m_diagonal;
    public Color m_resolutionLine;
    public int m_width=6;
    public int m_heigt=6;

    private void Update()
    {
        m_pointsPosition.m_topLeft = m_pointsRef.m_topLeft.position;
        m_pointsPosition.m_topRight = m_pointsRef.m_topRight.position;
        m_pointsPosition.m_downLeft = m_pointsRef.m_downLeft.position;
        m_pointsPosition.m_downRight = m_pointsRef.m_downRight.position;

        Draw(m_pointsPosition);
     
        Vector3 upAxis = Vector3.Cross(-m_pointsPosition.m_downLeft + m_pointsPosition.m_topLeft, -m_pointsPosition.m_downLeft + m_pointsPosition.m_downRight);
        Quaternion forward= Quaternion.LookRotation(-m_pointsPosition.m_downLeft + m_pointsPosition.m_topLeft, upAxis);
        Eloi.E_DrawingUtility.DrawCartesianOrigine(m_pointsPosition.m_downLeft, forward, 5, Time.deltaTime);
        m_origineSystemOrientation = forward;
        m_origineSystemPoint = m_pointsPosition.m_downLeft;

        Eloi.E_RelocationUtility.GetWorldToLocal_Point(m_pointsPosition.m_topLeft, m_pointsPosition.m_downLeft, forward, out m_pointsRelocatedAtOnDLTL.m_topLeft);
        Eloi.E_RelocationUtility.GetWorldToLocal_Point(m_pointsPosition.m_topRight, m_pointsPosition.m_downLeft, forward, out m_pointsRelocatedAtOnDLTL.m_topRight);
        Eloi.E_RelocationUtility.GetWorldToLocal_Point(m_pointsPosition.m_downLeft, m_pointsPosition.m_downLeft, forward, out m_pointsRelocatedAtOnDLTL.m_downLeft);
        Eloi.E_RelocationUtility.GetWorldToLocal_Point(m_pointsPosition.m_downRight, m_pointsPosition.m_downLeft, forward, out m_pointsRelocatedAtOnDLTL.m_downRight);

        Draw(m_pointsRelocatedAtOnDLTL);

        Flat(in m_pointsRelocatedAtOnDLTL, out m_pointsRelocatedAtOnDLTLFlat);
        Draw(m_pointsRelocatedAtOnDLTLFlat);

        //Eloi.E_DrawingUtility.DrawLines(Time.deltaTime, m_resolutionLine, m_pointsPosition.m_downLeft, m_pointsPosition.m_topRight);
    }

    private void Flat(in IrregularQuadrilateralsVector3 from, out IrregularQuadrilateralsVector3 to)
    {
        to = new IrregularQuadrilateralsVector3();
        to.m_downLeft = new Vector3(from.m_downLeft.x, 0, from.m_downLeft.z);
        to.m_downRight = new Vector3(from.m_downRight.x, 0, from.m_downRight.z);
        to.m_topLeft = new Vector3(from.m_topLeft.x, 0, from.m_topLeft.z);
        to.m_topRight = new Vector3(from.m_topRight.x, 0, from.m_topRight.z);
    }

    private void Draw(IrregularQuadrilateralsVector3 target)
    {
        Eloi.E_DrawingUtility.DrawLines(Time.deltaTime, m_border, target.m_topLeft, target.m_topRight, target.m_downRight, target.m_downLeft, target.m_topLeft);
        Eloi.E_DrawingUtility.DrawLines(Time.deltaTime, m_diagonal, target.m_topLeft, target.m_downRight);
        Eloi.E_DrawingUtility.DrawLines(Time.deltaTime, m_diagonal, target.m_downLeft, target.m_topRight);
    }
}



[System.Serializable]
public class IrregularQuadrilateralsTansform {
    public Transform m_topLeft;
    public Transform m_topRight;
    public Transform m_downLeft;
    public Transform m_downRight;
}
[System.Serializable]
public struct IrregularQuadrilateralsVector3 {
    public Vector3 m_topLeft;
    public Vector3 m_topRight;
    public Vector3 m_downLeft;
    public Vector3 m_downRight;
}


public struct GridCellAproximativeInfo
{
    public int xL2R;
    public int yD2T;
    public float xCenter;
    public float yCenter;
    public float minRadius;
    public float maxRadius;
}
public struct GridCellFourPoints
{
    public int xL2R;
    public int yD2T;
    public Vector2 downLeft;
    public Vector2 downRight;
    public Vector2 topLeft;
    public Vector2 topRight;

    public void IsLeftOf(ref bool isAtLeft, in int xL2R) { isAtLeft = xL2R < downLeft.x && xL2R < topLeft.x; }
    public void IsRightOf(ref bool isAtRight, in int xL2R) { isAtRight = xL2R > downRight.x && xL2R > topRight.x; }
    public void IsUpOf(ref bool isAtLeft, in int yD2T) { isAtLeft = yD2T >topLeft.x && yD2T > topRight.x; }
    public void IsDownOf(ref bool isAtRight, in int yD2T) { isAtRight = yD2T < downLeft.y && yD2T < downRight.y; }
}