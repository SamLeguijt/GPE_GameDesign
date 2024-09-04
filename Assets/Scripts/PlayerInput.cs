using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public delegate void PlayerInputHandler();
    public event PlayerInputHandler MoveLeftInput;
    public event PlayerInputHandler MoveRightInput;
    public event PlayerInputHandler ShootInput;

    [SerializeField] private List<KeyCode> leftMoveKeys = new List<KeyCode>();
    [SerializeField] private List<KeyCode> rightMoveKeys = new List<KeyCode>();
    [SerializeField] private List<KeyCode> shootKeys = new List<KeyCode>();

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < leftMoveKeys.Count; i++)
        {
            if (Input.GetKeyDown(leftMoveKeys[i]))
            {
                MoveLeftInput?.Invoke();
            }
        }

        for (int i = 0; i < rightMoveKeys.Count; i++)
        {
            if (Input.GetKeyDown(rightMoveKeys[i]))
            {
                MoveRightInput?.Invoke();
            }
        }

        for (int i = 0; i < shootKeys.Count; i++)
        {
            if (Input.GetKeyDown(shootKeys[i]))
            {
                ShootInput?.Invoke();
            }
        }
    }
}
