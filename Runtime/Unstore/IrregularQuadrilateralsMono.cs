using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IrregularQuadrilateralsMono : MonoBehaviour
{

    public IrregularQuadrilateralsTansform m_pointsRef;
    public IrregularQuadrilateralsWorkSpace m_quadWorkspace;

    private void Update()
    {
        IrregularQuadrilateralsUtility.GetWorkspaceFromTransform(
            m_pointsRef.m_downLeft,
            m_pointsRef.m_topLeft,
            m_pointsRef.m_downRight,
            m_pointsRef.m_topRight,
            out m_quadWorkspace);
     }
    public void GetCoordinateSystemInfo(out Vector3 worldPositionSystem, out Quaternion worldRotationSystem)
    {
        worldPositionSystem = m_quadWorkspace.m_originPosition;
        worldRotationSystem = m_quadWorkspace.m_origineDirection;
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


[System.Serializable]
public struct GridCellAproximativeInfo
{
    public int xL2R;
    public int yD2T;
    public float xCenter;
    public float yCenter;
    public float minRadius;
    public float maxRadius;
}
[System.Serializable]
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


public class IrregularQuadrilateralsUtility
{
    public static void GetWorkspaceFromTransform(
       in  Transform leftDown,
       in  Transform leftTop,
       in  Transform rightDown,
       in Transform rightTop,
        out IrregularQuadrilateralsWorkSpace workspace )
    {
        GetWorkspaceFromWorldSpace(leftDown.position,  leftTop.position,  rightDown.position,  rightTop.position,
           out workspace);
    }
    public static void GetWorkspaceFromWorldSpace(
    in  Vector3 leftDown,
    in  Vector3 leftTop,
    in  Vector3 rightDown,
    in  Vector3 rightTop,
      out IrregularQuadrilateralsWorkSpace workspace)
    {

        GetWorkspaceFromWorldSpace(in leftDown, in leftTop, in rightDown, in rightTop,
            out IrregularQuadrilateralsWorkSpaceConstructor c);
        workspace.m_origineDirection = c.m_origineSystemOrientation;
        workspace.m_originPosition = c.m_origineSystemPoint;
        workspace.m_worldSpace = c.m_pointsPosition;
        workspace.m_zeroLocalSpace = c.m_pointsRelocatedAtOnDLTLFlat;


    }
    public static void GetWorkspaceFromWorldSpace(
    in Vector3 leftDown,
    in Vector3 leftTop,
    in Vector3 rightDown,
    in Vector3 rightTop,
     out IrregularQuadrilateralsWorkSpaceConstructor workspace)
    {

        workspace = new IrregularQuadrilateralsWorkSpaceConstructor();
        workspace.m_pointsPosition.m_topLeft = leftTop;
        workspace.m_pointsPosition.m_topRight = rightTop;
        workspace.m_pointsPosition.m_downLeft = leftDown;
        workspace.m_pointsPosition.m_downRight = rightDown;
        Vector3 upAxis = Vector3.Cross(-
            workspace.m_pointsPosition.m_downLeft +
            workspace.m_pointsPosition.m_topLeft, -workspace.m_pointsPosition.m_downLeft + workspace.m_pointsPosition.m_downRight);
        Quaternion forward = Quaternion.LookRotation(-workspace.m_pointsPosition.m_downLeft + workspace.m_pointsPosition.m_topLeft, upAxis);
        Eloi.E_DrawingUtility.DrawCartesianOrigine(workspace.m_pointsPosition.m_downLeft, forward, 5, Time.deltaTime);
        workspace.m_origineSystemOrientation = forward;
        workspace.m_origineSystemPoint = workspace.m_pointsPosition.m_downLeft;

        Eloi.E_RelocationUtility.GetWorldToLocal_Point(workspace.m_pointsPosition.m_topLeft, workspace.m_pointsPosition.m_downLeft, forward, out workspace.m_pointsRelocatedAtOnDLTL.m_topLeft);
        Eloi.E_RelocationUtility.GetWorldToLocal_Point(workspace.m_pointsPosition.m_topRight, workspace.m_pointsPosition.m_downLeft, forward, out workspace.m_pointsRelocatedAtOnDLTL.m_topRight);
        Eloi.E_RelocationUtility.GetWorldToLocal_Point(workspace.m_pointsPosition.m_downLeft, workspace.m_pointsPosition.m_downLeft, forward, out workspace.m_pointsRelocatedAtOnDLTL.m_downLeft);
        Eloi.E_RelocationUtility.GetWorldToLocal_Point(workspace.m_pointsPosition.m_downRight, workspace.m_pointsPosition.m_downLeft, forward, out workspace.m_pointsRelocatedAtOnDLTL.m_downRight);
        Flat(in workspace.m_pointsRelocatedAtOnDLTL, out workspace.m_pointsRelocatedAtOnDLTLFlat);
     }
    private static void Flat(in IrregularQuadrilateralsVector3 from, out IrregularQuadrilateralsVector3 to)
    {
        to = new IrregularQuadrilateralsVector3();
        to.m_downLeft = new Vector3(from.m_downLeft.x, 0, from.m_downLeft.z);
        to.m_downRight = new Vector3(from.m_downRight.x, 0, from.m_downRight.z);
        to.m_topLeft = new Vector3(from.m_topLeft.x, 0, from.m_topLeft.z);
        to.m_topRight = new Vector3(from.m_topRight.x, 0, from.m_topRight.z);
    }



    public static void DrawIrregularBorderAndDiagonal(in IrregularQuadrilateralsVector3 target, in Color border, in Color diagonal, float deltaTime = 0)
    {
        if (deltaTime <= 0)
            deltaTime = Time.deltaTime;
        Eloi.E_DrawingUtility.DrawLines(deltaTime, border, target.m_topLeft, target.m_topRight, target.m_downRight, target.m_downLeft, target.m_topLeft);
        Eloi.E_DrawingUtility.DrawLines(deltaTime, diagonal, target.m_topLeft, target.m_downRight);
        Eloi.E_DrawingUtility.DrawLines(deltaTime, diagonal, target.m_downLeft, target.m_topRight);
    }
    public static void DrawIrregularGrid(in IrregularQuadrilateralsVector3 quadTarget,
        in uint gridWidth,
        in uint gridHeight,
        in Color horizontalColor,
        in Color verticalColor, 
        float deltaTime = 0)
    {
        if (deltaTime <= 0)
            deltaTime = Time.deltaTime;

        Vector3 tl = quadTarget.m_topLeft, dl = quadTarget.m_downLeft;
        Vector3 tr = quadTarget.m_topRight, dr = quadTarget.m_downRight;

        float percent = 1f / (float)gridHeight;
        float halfPercent = percent * 0.5f;
        for (int i = 0; i < gridHeight; i++)
        {

            Vector3 left = Vector3.Lerp(dl, tl, percent * i);
            Vector3 right = Vector3.Lerp(dr, tr, percent * i);
            Debug.DrawLine(left, right, horizontalColor, Time.deltaTime);

        }

        percent = 1f / (float)gridWidth;
        halfPercent = percent * 0.5f;
        for (int i = 0; i < gridWidth; i++)
        {
            Vector3 top = Vector3.Lerp(tl, tr, percent * i);
            Vector3 down = Vector3.Lerp(dl, dr, percent * i);
            Debug.DrawLine(top, down, verticalColor, Time.deltaTime);

        }


        //Eloi.E_DrawingUtility.DrawLines(deltaTime, border, target.m_topLeft, target.m_topRight, target.m_downRight, target.m_downLeft, target.m_topLeft);
        //Eloi.E_DrawingUtility.DrawLines(deltaTime, diagonal, target.m_topLeft, target.m_downRight);
        //Eloi.E_DrawingUtility.DrawLines(deltaTime, diagonal, target.m_downLeft, target.m_topRight);
    }


}

[System.Serializable]
public struct IrregularQuadrilateralsWorkSpace
{
    public Vector3 m_originPosition;
    public Quaternion m_origineDirection;
    public IrregularQuadrilateralsVector3 m_worldSpace;
    public IrregularQuadrilateralsVector3 m_zeroLocalSpace;

}

[System.Serializable]
public struct IrregularQuadrilateralsWorkSpaceConstructor
{
    public IrregularQuadrilateralsVector3 m_pointsPosition;
    public IrregularQuadrilateralsVector3 m_pointsRelocatedAtZero;
    public IrregularQuadrilateralsVector3 m_pointsRelocatedAtOnDLTL;
    public IrregularQuadrilateralsVector3 m_pointsRelocatedAtOnDLTLFlat;

    public Quaternion m_origineSystemOrientation;
    public Vector3 m_origineSystemPoint;


}