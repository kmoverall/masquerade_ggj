using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform _cameraTarget;
    [SerializeField]
    private Camera _mainCamera;
    [SerializeField]
    private Camera _spriteCamera;
    [SerializeField]
    private float _zoomTime;
    [SerializeField]
    private float _baseFoV;
    [SerializeField]
    private float _zoomFoV;

    private Transform _target;
    private Vector3 _homeLocation = Vector3.zero;

    public bool IsZoomed => t > 0;

    private float t;

    private void Awake()
    {
        _homeLocation = _cameraTarget.position;
        _target = _cameraTarget;
    }

    public void ZoomOnTarget(Transform target)
    {
        _target = target;
        StartCoroutine(ZoomInCoroutine());
    }
    public void ReturnToHome()
    {
        StartCoroutine(ZoomOutCoroutine());
    }

    private void Update()
    {
        float x = Mathf.Lerp(_homeLocation.x, _target.position.x, t);
        float z = Mathf.Lerp(_homeLocation.z, _target.position.z, t);
        _cameraTarget.position = new Vector3(x, _homeLocation.y, z);

        _mainCamera.fieldOfView = Mathf.Lerp(_baseFoV, _zoomFoV, t);
        _spriteCamera.fieldOfView = Mathf.Lerp(_baseFoV, _zoomFoV, t);
    }

    private IEnumerator ZoomInCoroutine()
    {
        t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / _zoomTime;
            yield return null;
        }
        t = 1;
        Game.ZoomInComplete?.Invoke();
    }
    private IEnumerator ZoomOutCoroutine()
    {
        Game.ZoomOutStarted?.Invoke();
        t = 1;
        while (t > 0)
        {
            t -= Time.deltaTime / _zoomTime;
            yield return null;
        }
        t = 0;
    }
}
