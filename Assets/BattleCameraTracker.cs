using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using static Cinemachine.CinemachineTargetGroup;

public class BattleCameraTracker : MonoBehaviour
{


    [SerializeField] private CinemachineTargetGroup m_targetGroup;

    private void OnEnable()
    {

        Brain.ins.EventManager.gridCompleted.AddListener((grid) => UpdateCameraTracking(grid.limits));


    }

    private void UpdateCameraTracking(Limits newLimits)
    {
        m_targetGroup.m_Targets = new CinemachineTargetGroup.Target[0];

        m_targetGroup.AddMember(newLimits.BottomLeft, 0.5f, 1f);
        m_targetGroup.AddMember(newLimits.TopRight, 0.5f, 1f);
    }


}
