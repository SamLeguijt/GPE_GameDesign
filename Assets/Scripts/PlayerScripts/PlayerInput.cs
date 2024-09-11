using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public delegate void PlayerInputHandler();
    public event PlayerInputHandler MoveLeftInput;
    public event PlayerInputHandler MoveRightInput;
    public event PlayerInputHandler ShootInput;
    public event PlayerInputHandler SwitchColorRightInput;
    public event PlayerInputHandler SwitchColorLeftInput;

    public bool IsInputActive { get; private set; } = false;

    [SerializeField] private List<KeyCode> leftMoveKeys = new List<KeyCode>();
    [SerializeField] private List<KeyCode> rightMoveKeys = new List<KeyCode>();
    [SerializeField] private List<KeyCode> shootKeys = new List<KeyCode>();
    [SerializeField] private List<KeyCode> leftColorSwitchKeys = new List<KeyCode>();
    [SerializeField] private List<KeyCode> rightColorSwitchKeys = new List<KeyCode>();

    private void Start()
    {
        GameManager.Instance.GameStartedEvent += OnStartGame;
    }
    private void OnDisable()
    {
        GameManager.Instance.GameStartedEvent -= OnStartGame;
    }

    private void OnStartGame()
    {
        StartCoroutine(EnableInputRoutine());
    }

    private IEnumerator EnableInputRoutine()
    {
        yield return new WaitForEndOfFrame();

        IsInputActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (leftMoveKeys.Count > 0)
        {
            for (int i = 0; i < leftMoveKeys.Count; i++)
            {
                if (Input.GetKeyDown(leftMoveKeys[i]))
                {
                    MoveLeftInput?.Invoke();
                }
            }
        }

        if (rightMoveKeys.Count > 0)
        {
            for (int i = 0; i < rightMoveKeys.Count; i++)
            {
                if (Input.GetKeyDown(rightMoveKeys[i]))
                {
                    MoveRightInput?.Invoke();
                }
            }
        }

        if (shootKeys.Count > 0)
        {
            for (int i = 0; i < shootKeys.Count; i++)
            {
                if (Input.GetKeyDown(shootKeys[i]))
                {
                    ShootInput?.Invoke();
                }
            }
        }


        if (leftColorSwitchKeys.Count > 0)
        {
            for (int i = 0; i < leftColorSwitchKeys.Count; i++)
            {
                if (Input.GetKeyDown(leftColorSwitchKeys[i]))
                {
                    SwitchColorLeftInput?.Invoke();
                }
            }
        }


        if (rightColorSwitchKeys.Count > 0)
        {
            for (int i = 0; i < rightColorSwitchKeys.Count; i++)
            {
                if (Input.GetKeyDown(rightColorSwitchKeys[i]))
                {
                    SwitchColorRightInput?.Invoke();
                }
            }
        }
    }
}
