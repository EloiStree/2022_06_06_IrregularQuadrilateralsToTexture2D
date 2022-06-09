using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FetchAllWorldPointsAndPushInQuadToTexture2DMono : MonoBehaviour
{
    public ContainMassGroupOfVector3Mono m_groupOfPoints;
    public IrregularQuadrilateralsMono m_irregularQuadUsed;
    public Shader_WorldPointsToLocalPoints m_worldToLocalShader;
    public Shader_LocalPointsInQuadToTextureWhite m_localToTexture2D;

    public int m_pointsUsed;
    public Texture2D m_result;
    public Eloi.ClassicUnityEvent_Texture2D m_onTextureComputed;

    public bool m_useUnityUpdate=true;

    private void Update()
    {
        if (m_useUnityUpdate)
        {
            Refresh();
        }
    }

    public void Refresh() {
        m_groupOfPoints.GetVector3Ref(out Vector3[] points);
        m_pointsUsed = points.Length;
        m_irregularQuadUsed.GetCoordinateSystemInfo(out Vector3 worldPositionSystem, out Quaternion worldRotationSystem);
        m_worldToLocalShader.SetCoordinateSystem(worldPositionSystem, worldRotationSystem);
        m_worldToLocalShader.SetPoints(points); 
        m_worldToLocalShader.ComputeInformation();
        m_localToTexture2D.SetLocalPoints(m_worldToLocalShader.m_localPointsArray);
        SetLocalTextureLocalQuadZone();
        m_localToTexture2D.ComputeAndPushTexture2D();
        m_localToTexture2D.GetResultAsTexture2D(out m_result);
        m_onTextureComputed.Invoke(m_result);


    }

    private void SetLocalTextureLocalQuadZone()
    {
        m_localToTexture2D.SetQuadrilateralFromFoursZeroLocalPointsXZ(
        m_irregularQuadUsed.m_quadWorkspace.m_zeroLocalSpace.m_downLeft,
        m_irregularQuadUsed.m_quadWorkspace.m_zeroLocalSpace.m_downRight,
        m_irregularQuadUsed.m_quadWorkspace.m_zeroLocalSpace.m_topLeft,
        m_irregularQuadUsed.m_quadWorkspace.m_zeroLocalSpace.m_topRight);

    }
}
