using System;
using System.Collections.Generic;
using Singletons;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class SplitScreenCameraManager : Singleton<SplitScreenCameraManager>
{
    private PlayerInputManager _inputManager;
    private List<PlayerInput> _playerInputs;

    private new void Awake()
    {
        base.Awake();
        _playerInputs = new List<PlayerInput>();
    }
    
    private void OnEnable()
    {
        _inputManager = MultiplayerManager.Instance.InputManager;
        _inputManager.onPlayerJoined += OnPlayerJoined;
        _inputManager.onPlayerLeft += playerLeaving => { _playerInputs.Remove(playerLeaving); ArrangeCameras(); };
    }

    private void OnDisable()
    {
        _inputManager.onPlayerJoined -= OnPlayerJoined;
    }

    private void OnPlayerJoined(PlayerInput newPlayerInput)
    {
        _playerInputs.Add(newPlayerInput);
        ConfigurePlayerCamera(newPlayerInput);

        ArrangeCameras();
    }

    private void ConfigurePlayerCamera(PlayerInput newPlayerInput)
    {
        CinemachineBrain newPlayerCinemachineBrain = newPlayerInput.transform.parent.GetComponentInChildren<CinemachineBrain>();
        CinemachineCamera newPlayerCinemachineCamera = newPlayerInput.transform.parent.GetComponentInChildren<CinemachineCamera>();
        CinemachineInputAxisController newPlayerCinemachineAxisController = newPlayerInput.transform.parent.GetComponentInChildren<CinemachineInputAxisController>();

        OutputChannels newPlayerCameraChannel =  (OutputChannels)(1 << _inputManager.playerCount);

        newPlayerCinemachineBrain.ChannelMask = newPlayerCameraChannel;
        newPlayerCinemachineCamera.OutputChannel = newPlayerCameraChannel;
        newPlayerCinemachineAxisController.PlayerIndex = newPlayerInput.playerIndex;
    }
    
    private void ArrangeCameras()
    {
        for (int i = 0; i < _playerInputs.Count; i++)
        {
            Camera playerCamera = _playerInputs[i].transform.parent.GetComponentInChildren<Camera>();

            switch (_playerInputs.Count)
            {
                case 1:
                    playerCamera.rect = new Rect(0, 0, 1, 1); // Full screen
                    break;
                case 2:
                    if (i == 0)
                        playerCamera.rect = new Rect(0, 0, 0.5f, 1); // Left half
                    else
                        playerCamera.rect = new Rect(0.5f, 0, 0.5f, 1); // Right half
                    break;
                case 3:
                    if (i == 0)
                        playerCamera.rect = new Rect(0, 0.5f, 0.5f, 0.5f); // Top-left
                    else if (i == 1)
                        playerCamera.rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f); // Top-right
                    else
                        playerCamera.rect = new Rect(0f, 0, 1f, 0.5f); // Bottom-center
                    break;
                case 4:
                    if (i == 0)
                        playerCamera.rect = new Rect(0, 0.5f, 0.5f, 0.5f); // Top-left
                    else if (i == 1)
                        playerCamera.rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f); // Top-right
                    else if (i == 2)
                        playerCamera.rect = new Rect(0, 0, 0.5f, 0.5f); // Bottom-left
                    else
                        playerCamera.rect = new Rect(0.5f, 0, 0.5f, 0.5f); // Bottom-right
                    break;
                default:
                    Debug.LogWarning("Only supports up to 4 players for now.");
                    break;
            }
        }
    }
}
