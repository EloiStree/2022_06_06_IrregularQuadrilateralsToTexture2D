using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Shader_LocalPointsInQuadToTextureWhite : MonoBehaviour
{
    
    //SHOULD BE SET FROM OUTSIDE INSTEAD
    public IrregularQuadrilateralsMono m_target;

    public ComputeShader m_irregularQuadrilToTextureShader;
    public List<Vector3> m_localPoints;
    public Vector3[] m_localdPointsArray;

    public int m_width=16;
    public int m_height=16;
    public RenderTexture m_renderTexture;
    public Eloi.ClassicUnityEvent_RenderTexture m_createdRenderer;


    public void SetLocalPoints(IEnumerable<Vector3> points) {
        m_localPoints = points.ToList();
    }

    public void Update()
    {
        Refresh();
    }

    [ContextMenu("SetRotationForward")]
    public void SetRotationForward() => Quaternion.LookRotation(Vector3.forward);

    void Refresh()
    {
        if (m_renderTexture == null) { 
            m_renderTexture = new RenderTexture(m_width, m_height,0);
            m_renderTexture.enableRandomWrite = true;
            Graphics.SetRandomWriteTarget(0, m_renderTexture);
            m_createdRenderer.Invoke(m_renderTexture);
        }
        if (m_localPoints == null || m_localPoints.Count <= 0)
            return;
        int kernel = m_irregularQuadrilToTextureShader.FindKernel("CSMain");

        int moduloOfGroup = m_localPoints.Count % 8;
        if (moduloOfGroup != 0)
        {
            for (int i = 0; i < (8 - moduloOfGroup); i++)
            {
                m_localPoints.Add(Vector3.zero);
            }
        }
        if (m_localdPointsArray.Length != m_localPoints.Count)
        {
            m_localdPointsArray = m_localPoints.ToArray();
         }

     
        ComputeBuffer bufferSet = new ComputeBuffer(m_localPoints.Count, sizeof(float) * 3);
        bufferSet.SetData(m_localPoints);
      
        m_irregularQuadrilToTextureShader.SetBuffer(kernel, "m_localPointsArray", bufferSet);
        m_irregularQuadrilToTextureShader.SetTexture(kernel, "Result", m_renderTexture);

        Vector2 m_p0DL = new Vector2(m_target.m_pointsRelocatedAtOnDLTLFlat.m_downLeft.x, m_target.m_pointsRelocatedAtOnDLTLFlat.m_downLeft.z);
        Vector2 m_p1DR = new Vector2(m_target.m_pointsRelocatedAtOnDLTLFlat.m_downRight.x, m_target.m_pointsRelocatedAtOnDLTLFlat.m_downRight.z);
        Vector2 m_p2TR = new Vector2(m_target.m_pointsRelocatedAtOnDLTLFlat.m_topRight.x, m_target.m_pointsRelocatedAtOnDLTLFlat.m_topRight.z);
        Vector2 m_p3TL = new Vector2(m_target.m_pointsRelocatedAtOnDLTLFlat.m_topLeft.x, m_target.m_pointsRelocatedAtOnDLTLFlat.m_topLeft.z);
        m_irregularQuadrilToTextureShader.SetFloats("m_p0DL", m_p0DL.x, m_p0DL.y);
        m_irregularQuadrilToTextureShader.SetFloats("m_p1DR", m_p1DR.x, m_p1DR.y);
        m_irregularQuadrilToTextureShader.SetFloats("m_p2TR", m_p2TR.x, m_p2TR.y);
        m_irregularQuadrilToTextureShader.SetFloats("m_p3TL", m_p3TL.x, m_p3TL.y);
        m_irregularQuadrilToTextureShader.SetInt("m_textureWidth", m_renderTexture.width);
        m_irregularQuadrilToTextureShader.SetInt("m_textureHeight", m_renderTexture.height);

        m_irregularQuadrilToTextureShader.Dispatch(kernel, m_localPoints.Count / 8, 1, 1);
        //RenderTexture.active = final;
        //m_renderTexture.ReadPixels(new Rect(0, 0, m_renderTexture.width, m_renderTexture.height), 0, 0);
        //m_renderTexture.Apply();

        //m_renderTexture.ReadPixels(new Rect(0, 0, m_renderTexture.width, m_renderTexture.height), 0, 0);
        //m_renderTexture.Apply();
        bufferSet.Dispose();
        m_createdRenderer.Invoke(m_renderTexture);
    }
}