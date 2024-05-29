using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct DirectionStatus {
    public bool isDown;
    public bool isUp;
    public bool isLeft;
    public bool isRight;
    public bool isInteract;

    public bool isMoving() {
        return isDown || isUp || isLeft || isRight || isInteract;
    }

    public Direction GetDirection() {
        if (isDown) return Direction.Down;
        if (isUp) return Direction.Up;
        if (isLeft) return Direction.Left;
        if (isRight) return Direction.Right;
        if (isInteract) return Direction.Interact;

        return Direction.None;
    }
}

public interface IState {
    void Enter();
    void Execute();
    void Exit();
}

public class IdleState : IState {
    PlayerBattle player;

    public IdleState(PlayerBattle player) {
        this.player = player;
    }

    public void Enter() { }

    public void Execute() { }

    public void Exit() { }
}

public class MoveState : IState {
    PlayerBattle player;

    public MoveState(PlayerBattle player) {
        this.player = player;
    }

    public void Enter() { }

    public void Execute() {
        // check position of player and target tile
        // move player to target tile

        if (player.currentTile == player.targetTile || player.targetTile == null) {
            player.SetState(player.idleState);
            return;
        }

        Collider collider = player.targetTile.transform.GetChild(0).GetComponent<Collider>();
        Vector3 topOfCollider = collider.bounds.center + player.targetTile.transform.up * collider.bounds.extents.y;

        // move player to target tile
        Vector3 direction = topOfCollider - player.transform.position;
        player.transform.position += direction.normalized * player.speed * Time.deltaTime;

        if (Vector3.Distance(player.transform.position, topOfCollider) < player.stoppingDistance) {
            player.currentTile = player.targetTile;
            player.transform.position = topOfCollider;
        }
    }

    public void Exit() { }
}

public class PlayerBattle : MonoBehaviour {
    IState currentState;
    DirectionStatus input;
    public Tile currentTile;
    public Tile targetTile;

    public IState idleState, moveState;

    [Header("Player Move Settings")]
    public float speed = 5;
    public float stoppingDistance = 0.1f;


    void Start() {
        idleState = new IdleState(this);
        moveState = new MoveState(this);

        SetState(idleState);
    }

    public void SetState(IState state) {
        currentState?.Exit();
        currentState = state;
        currentState?.Enter();
    }

    void Update() {
        input = GetDirectionStatus();
        currentState?.Execute();

        if (!input.isMoving()) return;

        Direction direction = input.GetDirection();
        Move(direction);
    }

    DirectionStatus GetDirectionStatus() {
        DirectionStatus status = new DirectionStatus();
        status.isDown = Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S);
        status.isUp = Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W);
        status.isLeft = Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A);
        status.isRight = Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D);
        status.isInteract = Input.GetKeyDown(KeyCode.Space);

        return status;
    }

    public void Move(Direction direction) {
        targetTile = currentTile.GetConnection(direction);
        if (targetTile != null) {
            SetState(moveState);
        }
    }

    public void Teleport(Tile tile) {
        currentTile = tile;
        targetTile = null;

        Collider collider = tile.transform.GetChild(0).GetComponent<Collider>();
        Vector3 topOfCollider = collider.bounds.center + tile.transform.up * collider.bounds.extents.y;

        transform.position = topOfCollider;

        SetState(idleState);
    }
}
