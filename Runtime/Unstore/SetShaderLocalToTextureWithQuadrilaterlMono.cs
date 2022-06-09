using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetShaderLocalToTextureWithQuadrilaterlMono : MonoBehaviour
{
    public IrregularQuadrilateralsMono m_source;
    public Shader_LocalPointsInQuadToTextureWhite m_shaderController;
        
    void Update()
    {
        IrregularQuadrilateralsVector3 iw = m_source.m_quadWorkspace.m_zeroLocalSpace;
        Vector2 dl = new Vector2(iw.m_downLeft.x, iw.m_downLeft.z);
        Vector2 dr = new Vector2(iw.m_downRight.x, iw.m_downRight.z);
        Vector2 tr = new Vector2(iw.m_topRight.x, iw.m_topRight.z);
        Vector2 tl = new Vector2(iw.m_topLeft.x, iw.m_topLeft.z);
        m_shaderController.SetQuadrilateralFromFoursZeroLocalPoints(dl, dr, tl, tr);
    }
}
