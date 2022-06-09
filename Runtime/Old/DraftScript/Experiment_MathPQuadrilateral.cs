using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Experiment_MathPQuadrilateral : MonoBehaviour
{
    public string m_source= "https://math.stackexchange.com/a/104595/1065770";
    public Transform m_worldPoint;
    public IrregularQuadrilateralsMono m_target;
    //https://math.stackexchange.com/questions/13404/mapping-irregular-quadrilateral-to-a-rectangle
    public Vector2 m_p;
    public Vector2 m_p0DL;
    public Vector2 m_p1DR;
    public Vector2 m_p2TR;
    public Vector2 m_p3TL;


    public Vector2 m_n0;
    public Vector2 m_n1;
    public Vector2 m_n2;
    public Vector2 m_n3;

    public float dU0;
    public float dU1;
    public float dV0;
    public float dV1;

    [Range(0f,1f)]
    public float u;
    [Range(0f, 1f)]
    public float v;

    public int m_width=16;
    public int m_height=16;
    public int m_x;
    public int m_y;


    void Update()
    {
        Eloi.E_RelocationUtility.GetWorldToLocal_Point(m_worldPoint.position, m_target.m_quadWorkspace.m_originPosition, m_target.m_quadWorkspace.m_origineDirection, out Vector3 localPoint);
        m_p = new Vector2(localPoint.x, localPoint.z);
        Vector3 lpFlat = localPoint;
        lpFlat.y = 0;
        Debug.DrawLine(Vector3.zero, localPoint, Color.red, Time.deltaTime);
        Debug.DrawLine(lpFlat, localPoint, Color.red, Time.deltaTime);
        m_p0DL = new Vector2(m_target.m_quadWorkspace.m_zeroLocalSpace.m_downLeft.x, m_target.m_quadWorkspace.m_zeroLocalSpace.m_downLeft.z);
        m_p1DR = new Vector2(m_target.m_quadWorkspace.m_zeroLocalSpace.m_downRight.x, m_target.m_quadWorkspace.m_zeroLocalSpace.m_downRight.z);
        m_p2TR = new Vector2(m_target.m_quadWorkspace.m_zeroLocalSpace.m_topRight.x, m_target.m_quadWorkspace.m_zeroLocalSpace.m_topRight.z);
        m_p3TL = new Vector2(m_target.m_quadWorkspace.m_zeroLocalSpace.m_topLeft.x, m_target.m_quadWorkspace.m_zeroLocalSpace.m_topLeft.z);

        m_n0 = -Vector2.Perpendicular   (m_p3TL - m_p0DL);
        m_n1 = Vector2.Perpendicular    (m_p1DR - m_p0DL);
        m_n2 = Vector2.Perpendicular    (m_p2TR - m_p1DR);
        m_n3 = -Vector2.Perpendicular   (m_p2TR - m_p3TL);

        dU0 = Vector2.Dot((m_p - m_p0DL), m_n0);
        dV0 = Vector2.Dot((m_p - m_p0DL), m_n1);
        dU1 = Vector2.Dot((m_p - m_p2TR), m_n2);
        dV1 = Vector2.Dot((m_p - m_p3TL), m_n3);

        DrawNormalized(m_n0.normalized * dU0, m_p);
        DrawNormalized(m_n1.normalized * dV0, m_p);
        if (dU0 + dU1 == 0)
            u = 0;
        else
            u = dU0 / (dU0 + dU1);

        if (dV0 + dV1 == 0)
            v = 0;
        else
            v = dV0 / (dV0 + dV1);

        m_x = (int)((u * m_width)  );
        m_y = (int)((v * m_height) );

    }

    private void DrawNormalized(Vector2 n, Vector2 p )
    {
        Vector3 nv = new Vector3(n.x, 0, n.y);
        Vector3 pv = new Vector3(p.x, 0, p.y); 

        Debug.DrawLine((pv-nv), pv, Color.cyan, Time.deltaTime);
    }
}
