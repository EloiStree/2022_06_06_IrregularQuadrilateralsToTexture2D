using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IrregularQuadrilaterals_PointsToTexture2D_V0 : IrregularQuadrilaterals_PointsToTexture2D
{
    public Vector3[] m_givenPoints;

    public int m_width=100;
    public int m_height=100;

    public Texture2D m_outputTexture;

    public override void Compute(Action finishedCallBack)
    {
        if (m_outputTexture == null || m_outputTexture.width!= m_width || m_outputTexture.height!= m_height)
        {
            m_outputTexture = new Texture2D(m_width, m_height);
        }
        if (m_givenPoints == null)
        {
            m_givenPoints = new Vector3[0];
        }


        if (finishedCallBack != null)
            finishedCallBack.Invoke();
    }

    public override void GetRenderedTexture(out Texture2D renderedTexture)
    {
        renderedTexture = m_outputTexture;
    }

    public override void SetPointsToCompute(in Vector3[] points)
    {
        m_givenPoints = points;
    }
}


















public abstract class IrregularQuadrilaterals_PointsToTexture2D : MonoBehaviour
{


    public abstract void SetPointsToCompute(in Vector3[] points);
    public abstract void Compute(System.Action finishedCallBack);
    public abstract void GetRenderedTexture(out Texture2D renderedTexture);

}
