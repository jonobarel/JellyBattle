using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
/// <summary>
/// Class for a camera that zooms in and out to accommodate both players in view
/// </summary>
public class MultipleTargetCamera : ColorFightersBase
{
    [Header("Follow Settings")]
    [SerializeField] private Vector3 cameraOffset;
    [SerializeField] private List<Transform> targets;
    [SerializeField] private float smoothTime = .5f;
    [SerializeField] private float minZoom = 40f;
    [SerializeField] private float maxZoom = 10f;
    [SerializeField] private float limitZoom = 50f;
    private float minCameraY = 3f;
    private float maxCameraY = 8f;
    private float maxCameraX = 10f;
    private float minCameraX = -10f;

    public List<Transform> Targets
    {
        get { return targets; }
    }

    private Vector3 velocity;
    private Camera gameCamera;

    void Start() {
        gameCamera = GetComponent<Camera>();
    }
    void LateUpdate() {
        MoveCamera();
        ZoomCamera();
    }

    void MoveCamera() {
        if (targets.Count == 0) {
            return;
        }
        Vector3 centerPoint = GetCenterPoint();
        Vector3 newPosition = centerPoint + cameraOffset;
        newPosition.y = Mathf.Clamp(newPosition.y, minCameraY, maxCameraY);
        newPosition.x = Mathf.Clamp(newPosition.x, minCameraX, maxCameraX);
        
        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
    }

    void ZoomCamera() {
        float newZoom = Mathf.Lerp(maxZoom, minZoom, GetGreatestDistance() / limitZoom);
        gameCamera.fieldOfView = Mathf.Lerp(gameCamera.fieldOfView, newZoom, Time.deltaTime);
    }

    private Bounds EncapsulateBounds() {
        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++) {
            bounds.Encapsulate(targets[i].position);
        }

        return bounds;
    }
    private Vector3 GetCenterPoint() {
        if (targets.Count == 1) {
            return targets[0].position;
        }

        var bounds = EncapsulateBounds();

        return bounds.center;
    }

    private float GetGreatestDistance() {

        var bounds = EncapsulateBounds();

        return bounds.size.x;

    }
}
