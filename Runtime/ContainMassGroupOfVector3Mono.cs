using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public abstract class ContainMassGroupOfVector3Mono : MonoBehaviour, IContainMassGroupOfVector3
{

    public abstract void GetVector3Ref(out Vector3[] points);
}

public interface IContainMassGroupOfVector3 {
    public void GetVector3Ref(out Vector3[] points);
}