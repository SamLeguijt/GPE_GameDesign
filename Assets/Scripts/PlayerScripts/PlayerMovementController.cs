using System.Collections;
using Unity.VisualScripting;
using UnityEngine;



public class PlayerMovementController : MonoBehaviour
{
    public delegate void PlayerMoveLanesHandler(Lane targetLane);
    public delegate void PlayerJumpHandler();

    public event PlayerMoveLanesHandler StartLaneSwapEvent;
    public event PlayerMoveLanesHandler StopLaneSwapEvent;

    public event PlayerJumpHandler StartJumpEvent;
    public event PlayerJumpHandler StopJumpEvent;

    public bool AllowedToMove { get; private set; } = true;
    public Lane CurrentLane { get; private set; }

    [Header("References")]
    [SerializeField] private Transform player = null;
    [SerializeField] private PlayerInput inputController = null;
    [SerializeField] private PlayerHealthController playerHealthController = null;
    [SerializeField] private LaneManager laneManager = null;

    [Header("Movement settings")]
    [SerializeField] private float laneSwapSpeed = 3f;

    private Coroutine swapLanesCoroutine = null;
    private bool isSwappingLanes = false;

    private void OnEnable()
    {
        if (inputController == null)
        {
            Debug.LogWarning($"InputController reference is missing on {this}, please assign and re run to use movement logic.");
            return;
        }

        inputController.MoveLeftInput += OnLeftMoveInputReceivedEvent;
        inputController.MoveRightInput += OnRightMoveInputReceivedEvent;

        playerHealthController.PlayerDeathEvent += OnPlayerDeathEvent;
    }

    private void OnDisable()
    {
        inputController.MoveLeftInput -= OnLeftMoveInputReceivedEvent;
        inputController.MoveRightInput-= OnRightMoveInputReceivedEvent;

        playerHealthController.PlayerDeathEvent -= OnPlayerDeathEvent;
    }

 
    private void Start()
    {
        player.position = new Vector3(CurrentLane.Position.x, player.position.y, player.position.z);
        CurrentLane = laneManager.MiddleLane;
    }


    private void OnLeftMoveInputReceivedEvent()
    {
        if (!AllowedToMove)
            return;

        if (isSwappingLanes || CurrentLane.LaneType == LaneEnum.Left)
            return;

        Lane targetLane = CurrentLane;

        switch (CurrentLane.LaneType)
        {
            case LaneEnum.Middle:
                targetLane = laneManager.LeftLane;
                break;
            case LaneEnum.Right:
                targetLane = laneManager.MiddleLane;
                break;
            default:
                break;
        }

        if (targetLane.LaneType != CurrentLane.LaneType)
            MoveToLane(targetLane);
    }

    private void OnRightMoveInputReceivedEvent()
    {
        if (isSwappingLanes || CurrentLane.LaneType == LaneEnum.Right)
            return;

        Lane targetLane = CurrentLane;

        switch (CurrentLane.LaneType)
        {
            case LaneEnum.Left:
                targetLane = laneManager.MiddleLane;
                break;
            case LaneEnum.Middle:
                targetLane = laneManager.RightLane;
                break;
            default:
                break;
        }

        if (targetLane.LaneType != CurrentLane.LaneType)
            MoveToLane(targetLane);
    }

    private void MoveToLane(Lane targetLane)
    {
        if (!AllowedToMove)
            return;

        if (swapLanesCoroutine != null)
            StopCoroutine(swapLanesCoroutine);

        swapLanesCoroutine = StartCoroutine(MoveLaneCoroutine(targetLane));
    }

    private IEnumerator MoveLaneCoroutine(Lane targetLane)
    {
        float elapsedTime = 0;
        float maxTime = 10;

        StartLaneSwapEvent?.Invoke(targetLane);
        isSwappingLanes = true;

        while (elapsedTime < maxTime)
        {
            // Keep the players' Y pos to 
            Vector2 targetPos = new Vector3(targetLane.Position.x, player.position.y);

            player.position = Vector2.MoveTowards(player.position, targetPos, laneSwapSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;

            if (player.position.x == targetLane.Position.x)
            {
                break;
            }

            yield return null;
        }

        CurrentLane = targetLane;
        StopLaneSwapEvent?.Invoke(targetLane);
        isSwappingLanes = false;
    }

    private void OnPlayerDeathEvent()
    {
        AllowedToMove = false;
    }
}

