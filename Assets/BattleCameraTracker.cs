using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using static Cinemachine.CinemachineTargetGroup;

public class BattleCameraTracker : MonoBehaviour
{
    [SerializeField] private CinemachineTargetGroup m_targetGroup;
    [SerializeField] private Transform m_bl = null;
    [SerializeField] private Transform m_tr = null;
    [SerializeField] private ParticleSystem m_battleVFX = null;

    private void OnEnable()
    {
        Brain.ins.EventManager.gridInitialised.AddListener((grid) => UpdateCameraTracking(grid));
        Brain.ins.EventManager.gridCompleted.AddListener((grid) => FinaliseCamera(grid));

        //Initialise targets
        m_targetGroup.m_Targets = new CinemachineTargetGroup.Target[0];

        m_targetGroup.AddMember(m_bl, 0.5f, 1f);
        m_targetGroup.AddMember(m_tr, 0.5f, 1f);
    }

    private void UpdateCameraTracking(TrackGrid newGrid)
    {
        m_bl.transform.position = new Vector3(0, 0, 0);
        m_tr.transform.position = new Vector3(newGrid.GridDims.x, 0, newGrid.GridDims.y);
    }

    private void FinaliseCamera(TrackGrid grid)
    {
        m_battleVFX.Play();
    }
}
