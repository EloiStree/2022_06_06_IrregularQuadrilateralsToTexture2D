using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shader_FlushToColorTargetTexture : MonoBehaviour
{
    public Color m_targetColor =Color.red;
    public ComputeShader m_shaderToSetColor;


    public void SetColorOnTexture(Texture2D texture)
    {
        int kernel = m_shaderToSetColor.FindKernel("CSMain");
        m_shaderToSetColor.SetTexture(kernel, "Result", texture);
        m_shaderToSetColor.SetFloat("m_backgroundColorRed", m_targetColor.r);
        m_shaderToSetColor.SetFloat("m_backgroundColorGreen", m_targetColor.g);
        m_shaderToSetColor.SetFloat("m_backgroundColorBlue", m_targetColor.b);
        m_shaderToSetColor.SetFloat("m_backgroundColorAlpha", m_targetColor.a);
        m_shaderToSetColor.Dispatch(kernel, texture.width / 8, texture.height / 8, 1);
    }
    public void SetColorOnTexture(RenderTexture texture)
    {
        int kernel = m_shaderToSetColor.FindKernel("CSMain");
        m_shaderToSetColor.SetTexture(kernel, "Result", texture);
        m_shaderToSetColor.SetFloat("m_backgroundColorRed", m_targetColor.r);
        m_shaderToSetColor.SetFloat("m_backgroundColorGreen", m_targetColor.g);
        m_shaderToSetColor.SetFloat("m_backgroundColorBlue", m_targetColor.b);
        m_shaderToSetColor.SetFloat("m_backgroundColorAlpha", m_targetColor.a);
        m_shaderToSetColor.Dispatch(kernel, texture.width / 8, texture.height / 8, 1);
    }
}
