using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class SetIK_Targets : MonoBehaviour
{
    [SerializeField] TwoBoneIKConstraint leftHandIk;
    [SerializeField] TwoBoneIKConstraint rightHandIK;
    [SerializeField] RigBuilder rigBuilder;

    private void OnTransformChildrenChanged()
    {
        leftHandIk.data.target = GameObject.Find("left_hand_target").transform;
        rightHandIK.data.target = GameObject.Find("right_hand_target").transform;
        rigBuilder.Build();
    }
}
