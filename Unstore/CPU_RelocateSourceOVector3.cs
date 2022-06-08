using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPU_RelocateSourceOVector3 : WorldToLocalRelocationOfPointsMono
{
    public Quaternion m_rotationOfOrigin;
    public Vector3 m_positionOfOrigin;

    public Vector3 [] received;
    public Vector3 [] relocated;


    public override void SetOrigine(in Quaternion worldRotation, in Vector3 worldPosition)
    {
        m_rotationOfOrigin = worldRotation;
        m_positionOfOrigin = worldPosition;
    }
    public override void SetPoints(in Vector3[] points)
    {
        received = points;
    }

    [ContextMenu("Refresh Info")]
    public void ComputeInfo()
    {
        ComputeInfo(null);
    }
    public override void ComputeInfo(System.Action finishedCallBack)
    {
        if (received.Length != relocated.Length)
            relocated = new Vector3[received.Length];

        for (int i = 0; i < received.Length; i++)
        {
            Eloi.E_RelocationUtility.GetWorldToLocal_Point(in received[i], in m_positionOfOrigin, in m_rotationOfOrigin, out relocated[i]);
        }
        if (finishedCallBack != null)
            finishedCallBack.Invoke();
    }

    public override void GetRelocatedPoint(out Vector3[] points)
    {
        points = relocated;
    }
}

public interface IWorldToLocalRelocationOfPoints {
    public void SetOrigine(in Quaternion worldRotation, in Vector3 worldPosition);
    public void SetPoints(in Vector3[] points);
    public void ComputeInfo(System.Action finishedCallBack);
    public void GetRelocatedPoint(out Vector3[] points);
}
public abstract class WorldToLocalRelocationOfPointsMono : MonoBehaviour, IWorldToLocalRelocationOfPoints
{
    public abstract void ComputeInfo(Action finishedCallBack);
    public abstract void GetRelocatedPoint(out Vector3[] points);
    public abstract void SetOrigine(in Quaternion worldRotation, in Vector3 worldPosition);
    public abstract void SetPoints(in Vector3[] points);
 
}
