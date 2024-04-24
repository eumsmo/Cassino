using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public float movementSpeed = 3f;
    public float gravity = 9.8f;
    CharacterController characterController;
    Vector3 input;

    void Awake() {
        characterController = GetComponent<CharacterController>();
    }

    void Update() {
        Matrix4x4 rotation = Matrix4x4.Rotate(Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0));
        input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        input = rotation.MultiplyVector(input);
    }

    void FixedUpdate() {
        if (!characterController.isGrounded) input.y -= gravity;
        characterController.Move(input * movementSpeed * Time.fixedDeltaTime);
    }
}
