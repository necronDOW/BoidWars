using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject focusOn;
    public float height = 5.0f;
    public float smoothing = 0.05f;

    Vector3 currentPos;

    // cam shake stuff
    private float shakeIntensity;
    private float shakeDecay;

    Vector3 _originPosition;
    Quaternion _originRotation;

    private void Start()
    {
        if (!focusOn)
            Debug.LogWarning(this.name + " (Camera) has no focus point.");
        else SetFocus(focusOn);
    }

    private void Update()
    {
        if (shakeIntensity > 0)
        {
            transform.position = currentPos + Random.insideUnitSphere * shakeIntensity;
            transform.rotation = new Quaternion
                (_originRotation.x + Random.Range(-shakeIntensity, shakeIntensity) * 0.2f,
                _originRotation.y + Random.Range(-shakeIntensity, shakeIntensity) * 0.2f,
                _originRotation.z,
                _originRotation.w);
            shakeIntensity -= shakeDecay;
        }
    }

    public void Shake(float intensity, float decay)
    {
        _originPosition = transform.position;
        _originRotation = transform.rotation;
        shakeIntensity = intensity;
        shakeDecay = decay;
    }
    private void FixedUpdate()
    {
        if (focusOn)
            MoveTo(focusOn.transform.position);
    }

    public void SetFocus(GameObject o)
    {
        focusOn = o;

        Vector3 target = focusOn.transform.position;

        transform.eulerAngles = new Vector3(90f, 0, 0);
        transform.position = new Vector3(target.x, height, target.z);
    }

    private void MoveTo(Vector3 target)
    {
        Vector3 velocity = Vector3.zero;
        Vector3 focusPosition = new Vector3(target.x, transform.position.y, target.z);

        currentPos = Vector3.SmoothDamp(transform.position, focusPosition, ref velocity, smoothing);
        transform.position = currentPos;
    }
}
