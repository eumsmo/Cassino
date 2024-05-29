using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPPlayer : MonoBehaviour {
    public Direction facingDirection = Direction.Up;
    public Vector3 rotationAxis = new Vector3(0, 1, 0);

    void Start() {
        Rotate(facingDirection);
    }

    public void Rotate(Direction direction) {
        facingDirection = direction;
        Debug.Log("Rotating player to " + direction);

        switch (direction) {
            case Direction.Up:
                transform.rotation = Quaternion.Euler(rotationAxis * 0);
                break;
            case Direction.Right:
                transform.rotation = Quaternion.Euler(rotationAxis * 90);
                break;
            case Direction.Down:
                transform.rotation = Quaternion.Euler(rotationAxis * 180);
                break;
            case Direction.Left:
                transform.rotation = Quaternion.Euler(rotationAxis * 270);
                break;
        }
    }
}
