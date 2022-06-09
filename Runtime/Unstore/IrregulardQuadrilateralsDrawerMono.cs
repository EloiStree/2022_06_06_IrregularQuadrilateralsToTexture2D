using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IrregulardQuadrilateralsDrawerMono : MonoBehaviour
{
    public bool m_useDebugDraw;
    public IrregularQuadrilateralsMono m_target;
    public Color m_border;
    public Color m_diagonal;
    public Color m_resolutionLineHorizontal;
    public Color m_resolutionLineVertical;
    public uint m_width = 6;
    public uint m_heigt = 6;
   

    void Update()
    {
        if (m_useDebugDraw) { 
            IrregularQuadrilateralsUtility.DrawIrregularBorderAndDiagonal(in m_target.m_quadWorkspace.m_worldSpace, in m_border, in m_diagonal);
            IrregularQuadrilateralsUtility.DrawIrregularBorderAndDiagonal(in m_target.m_quadWorkspace.m_zeroLocalSpace, in m_border, in m_diagonal);
            IrregularQuadrilateralsUtility.DrawIrregularGrid(in m_target.m_quadWorkspace.m_worldSpace,
                in m_width, in m_heigt, in m_resolutionLineHorizontal, in m_resolutionLineVertical);
            IrregularQuadrilateralsUtility.DrawIrregularGrid(in m_target.m_quadWorkspace.m_zeroLocalSpace,
                in m_width, in m_heigt, in m_resolutionLineHorizontal, in m_resolutionLineVertical);
        }
    }
    private void Reset()
    {
        m_target = GetComponent<IrregularQuadrilateralsMono>();
    }
}
