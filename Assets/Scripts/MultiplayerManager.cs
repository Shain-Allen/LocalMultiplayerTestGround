using Singletons;
using UnityEngine.InputSystem;

public class MultiplayerManager : Singleton<MultiplayerManager>
{
    private PlayerInputManager _inputManager;
    
    public PlayerInputManager InputManager
    {
        get
        {
            if (_inputManager == null)
            {
                _inputManager = GetComponent<PlayerInputManager>();
            }
            return _inputManager;
        }
    }
}
