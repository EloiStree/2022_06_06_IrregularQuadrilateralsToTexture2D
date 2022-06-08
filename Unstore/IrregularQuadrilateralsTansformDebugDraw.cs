using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IrregularQuadrilateralsTansformDebugDraw : MonoBehaviour
{
    public IrregularQuadrilateralsMono m_source;

    public Color m_horizontal = Color.yellow;
    public Color m_vertical = Color.yellow * 0.5f;

    public void Update()
    {
        DrawLineOf(m_source.m_pointsRelocatedAtOnDLTLFlat);
        DrawLineOf(m_source.m_pointsPosition);
    }

    private void DrawLineOf( IrregularQuadrilateralsVector3 quad)
    {
        Vector3 tl = quad.m_topLeft, dl = quad.m_downLeft;
        Vector3 tr = quad.m_topRight, dr = quad.m_downRight;

        float percent = 1f / (float)m_source.m_heigt;
        float halfPercent = percent * 0.5f;
        for (int i = 0; i < m_source.m_heigt; i++)
        {

            Vector3 left = Vector3.Lerp(dl, tl, percent * i);
            Vector3 right = Vector3.Lerp(dr, tr, percent * i);
            Debug.DrawLine(left, right, m_horizontal, Time.deltaTime);

        }

        percent = 1f / (float)m_source.m_width;
        halfPercent = percent * 0.5f;
        for (int i = 0; i < m_source.m_width; i++)
        {
            Vector3 top = Vector3.Lerp(tl, tr, percent * i);
            Vector3 down = Vector3.Lerp(dl, dr, percent * i);
            Debug.DrawLine(top, down, m_vertical, Time.deltaTime);

        }
    }
}
