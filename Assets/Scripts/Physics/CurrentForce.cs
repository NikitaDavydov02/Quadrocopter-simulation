using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IForce
{
    //public List<Vector3> CurrentForceVector { get;}
    //public List<Vector3> AbsolutePointOfForceApplying { get;}
    public void CountForce(out List<Vector3> CurrentForceVectors, out List<Vector3> AbsolutePointsOfForceApplying);
}
