using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Shader_LocalPointsInQuadToTextureWhite : MonoBehaviour
{
    public Vector2 m_p0DL ;
    public Vector2 m_p1DR ;
    public Vector2 m_p2TR ;
    public Vector2 m_p3TL ;

    
    public ComputeShader m_irregularQuadrilToTextureShader;
    public Shader_FlushToColorTargetTexture m_setColorOfTexture;
    public List<Vector3> m_localPoints;

    public int m_width=16;
    public int m_height=16;
    public RenderTexture m_renderTexture;
    public Eloi.ClassicUnityEvent_RenderTexture m_createdRenderer;

    public Texture2D m_createdTexture2D;
    public Eloi.ClassicUnityEvent_Texture2D m_createdRendererAsTexture;


    public void SetLocalPoints(IEnumerable<Vector3> points) {
        m_localPoints = points.ToList();

    }
    public void SetQuadrilateralFromFoursZeroLocalPointsXZ(Vector3 downLeft, Vector3 downRight, Vector3 topLeft, Vector3 topRight)
    {

        m_p0DL = new Vector2(downLeft.x, downLeft.z);
        m_p1DR = new Vector2(downRight.x, downRight.z);
        m_p2TR = new Vector2(topRight.x, topRight.z);
        m_p3TL = new Vector2(topLeft.x, topLeft.z);

    }
    public void SetQuadrilateralFromFoursZeroLocalPoints(Vector2 downLeft, Vector2 downRight, Vector2 topLeft, Vector2 topRight)
    {

        m_p0DL = downLeft;
        m_p1DR = downRight;
        m_p2TR = topRight;
        m_p3TL = topLeft;

    }

    [ContextMenu("Compute Given Data")]
    public void ComputeAndPushTexture2D()
    {
        if (m_p0DL == Vector2.zero &&
            m_p1DR == Vector2.zero &&
            m_p2TR == Vector2.zero &&
            m_p3TL == Vector2.zero)
            return;

        if (m_renderTexture == null) { 
            m_renderTexture = new RenderTexture(m_width, m_height,0);
            m_renderTexture.enableRandomWrite = true;
            Graphics.SetRandomWriteTarget(0, m_renderTexture);
            m_createdRenderer.Invoke(m_renderTexture);
        }
        if (m_localPoints == null || m_localPoints.Count <= 0)
            return;
        if(m_setColorOfTexture)
            m_setColorOfTexture.SetColorOnTexture(m_renderTexture);
        int kernel = m_irregularQuadrilToTextureShader.FindKernel("CSMain");

        int moduloOfGroup = m_localPoints.Count % 8;
        if (moduloOfGroup != 0)
        {
            for (int i = 0; i < (8 - moduloOfGroup); i++)
            {
                m_localPoints.Add(Vector3.zero);
            }
        }
       
     
        ComputeBuffer bufferSet = new ComputeBuffer(m_localPoints.Count, sizeof(float) * 3);
        bufferSet.SetData(m_localPoints);
      
        m_irregularQuadrilToTextureShader.SetBuffer(kernel, "m_localPointsArray", bufferSet);
        m_irregularQuadrilToTextureShader.SetTexture(kernel, "Result", m_renderTexture);

        m_irregularQuadrilToTextureShader.SetFloats("m_p0DL", m_p0DL.x, m_p0DL.y);
        m_irregularQuadrilToTextureShader.SetFloats("m_p1DR", m_p1DR.x, m_p1DR.y);
        m_irregularQuadrilToTextureShader.SetFloats("m_p2TR", m_p2TR.x, m_p2TR.y);
        m_irregularQuadrilToTextureShader.SetFloats("m_p3TL", m_p3TL.x, m_p3TL.y);
        m_irregularQuadrilToTextureShader.SetInt("m_textureWidth", m_renderTexture.width);
        m_irregularQuadrilToTextureShader.SetInt("m_textureHeight", m_renderTexture.height);

        m_irregularQuadrilToTextureShader.Dispatch(kernel, m_localPoints.Count / 8, 1, 1);
       

        bufferSet.Dispose();
        

        m_countTest++;
        m_createdRenderer.Invoke(m_renderTexture);
        Eloi.E_Texture2DUtility.RenderTextureToTexture2D(
            m_renderTexture, out m_createdTexture2D);
        m_createdRendererAsTexture.Invoke(m_createdTexture2D);
    }

    public void GetResultAsTexture2D(out Texture2D result)
    {
        result = m_createdTexture2D;
    }
    public ulong m_countTest;
}