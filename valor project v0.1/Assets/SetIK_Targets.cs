using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class SetIK_Targets : MonoBehaviour
{
    [SerializeField] TwoBoneIKConstraint leftHandIk;
    [SerializeField] TwoBoneIKConstraint rightHandIK;
    [SerializeField] RigBuilder rigBuilder;

    public void SetTargets(Transform LHT, Transform RHT)
    {
        leftHandIk.data.target = LHT;
        rightHandIK.data.target = RHT;
        rigBuilder.Build();
    }
}
