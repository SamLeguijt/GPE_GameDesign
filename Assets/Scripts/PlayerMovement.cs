using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerInput inputController = null;
    [SerializeField] private float moveSpeed = 3f;
    private void OnEnable()
    {
        inputController.MoveLeftInput += OnMoveLeftInputReceivedEvent;
        inputController.MoveRightInput += OnMoveRightInputReceivedEvent;
    }

    private void OnDisable()
    {
        inputController.MoveLeftInput -= OnMoveLeftInputReceivedEvent;
        inputController.MoveRightInput -= OnMoveRightInputReceivedEvent;
    }

    private void OnMoveLeftInputReceivedEvent()
    {
        transform.position = new Vector3(transform.position.x - moveSpeed * Time.deltaTime, transform.position.y, transform.position.z);
    }
    
    private void OnMoveRightInputReceivedEvent()
    {
        transform.position = new Vector3(transform.position.x + moveSpeed * Time.deltaTime, transform.position.y, transform.position.z);
    }
}
