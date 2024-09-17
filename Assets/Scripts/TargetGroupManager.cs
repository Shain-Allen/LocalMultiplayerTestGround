using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class TargetGroupManager : MonoBehaviour
{
    private CinemachineTargetGroup _targetGroup;
    
    private void Awake()
    {
        _targetGroup = GetComponent<CinemachineTargetGroup>();
    }

    private void Start()
    {
        PlayerInputManager.instance.onPlayerJoined += OnPlayerJoined;
    }

    private void OnPlayerJoined(PlayerInput newPlayer)
    {
        _targetGroup.AddMember(newPlayer.transform, 1, 1);
    }
}
