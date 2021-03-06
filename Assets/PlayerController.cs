﻿using UnityEngine;
using System.Collections;

public class PlayerController : AriaBehaviour
{
    public Camera cam;
    public Rigidbody rb;
    public Animator animator;

    public float offsetMoveSpeed;
    public float maxSpeed;
    public float acceleration;
    public float currentSpeed;

    public float moveThreshold;
    public float moveInputThreshold;
    public float jumpStrength;
    public float jumpInputThreshold;

    public float maxTargetPlayerPositionOffset;
    private Vector3 targetPlayerPos;

    public Vector3 cameraOffset;
    private Vector3 targetCameraPos;
    private Vector3 currentCameraVelocity;
    private bool moving;

    void Start ()
    {
        if (cam == null)
        {
            cam = Camera.main;
        }
        if (rb != null)
        {
            throw new MissingComponentException("Missing rigid body");
        }

        targetCameraPos = targetPlayerPos = rb.position;
        targetCameraPos += cameraOffset;

        cam.transform.position = targetCameraPos;
    }

    Touch t;
    Vector2 tPosOrigin;
    void Update ()
    {
        targetPlayerPos.y = rb.position.y;
        targetPlayerPos.z = rb.position.z;

        bool jumpStart = false;
        if (Input.GetButton("Horizontal"))
        {
            targetPlayerPos.x += Input.GetAxisRaw("Horizontal") * offsetMoveSpeed * Time.deltaTime;
        }
        if (Input.touchCount > 0)
        {
            t = Input.GetTouch(0);

            // Grab the original touch position so we can measure sensitivity.
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                tPosOrigin = t.position;
            }
            // If the threashold is crossed, start handling left and right movement.
            float inputFullXDelta = tPosOrigin.x - t.position.x;
            if (Mathf.Abs(inputFullXDelta) > moveInputThreshold)
            {
                targetPlayerPos.x -= t.deltaPosition.x * 0.01f * offsetMoveSpeed;
            }
            // Test vertical touch delta for jump test.
            float inputFullYDelta = tPosOrigin.y - t.position.y;
            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                jumpStart = (inputFullYDelta > jumpInputThreshold) ? true : false;
            }
        }
        
        if (jumpStart)
        {
            rb.AddForce(Vector3.up * jumpStrength);
        }

        Vector3 offsetBefore = targetPlayerPos - rb.position;

        // Cap targetPlayerPos.
        if (offsetBefore.magnitude > maxTargetPlayerPositionOffset)
        {
            offsetBefore = offsetBefore.normalized * maxTargetPlayerPositionOffset;
            targetPlayerPos = offsetBefore + rb.position;
        }

        // Handle movement.
        if (!moving && offsetBefore.magnitude > moveThreshold)
        {
            moving = true;
        }

        if (moving)
        {
            rb.transform.rotation = Quaternion.LookRotation(offsetBefore.normalized);

            currentSpeed += (currentSpeed < maxSpeed) ? acceleration * Time.deltaTime : 0;
            Vector3 rbPos = rb.position;
            rbPos.x += offsetBefore.normalized.x * currentSpeed * Time.deltaTime;
            rb.transform.position = rbPos;
        }

        Vector3 offsetAfter = targetPlayerPos - rb.position;
        if (moving && (offsetAfter.magnitude < 0.1f || Vector3.Dot(offsetAfter.normalized, offsetBefore.normalized) < 0))
        {
            moving = false;

            currentSpeed = 0;
        }

        if(animator != null)
        {
            animator.SetFloat("Move Speed", currentSpeed);
        }

        targetCameraPos = targetPlayerPos;
        targetCameraPos += cameraOffset;

        cam.transform.position = Vector3.SmoothDamp(cam.transform.position, targetCameraPos, ref currentCameraVelocity, 0.3f);
	}

    void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawLine(targetPlayerPos, rb.position);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(targetPlayerPos, 0.2f);
    }
}
