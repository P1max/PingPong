using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFitter : MonoBehaviour
{
    [SerializeField] private float _targetAspectX = 16f;
    [SerializeField] private float _targetAspectY = 9f;

    private int _lastWidth;
    private int _lastHeight;
    private Camera _camera;

    void Awake()
    {
        _camera = GetComponent<Camera>();
        _lastWidth = Screen.width;
        _lastHeight = Screen.height;
        Apply();
    }

    void Update()
    {
        if (Screen.width != _lastWidth || Screen.height != _lastHeight)
        {
            _lastWidth = Screen.width;
            _lastHeight = Screen.height;
            Apply();
        }
    }

    void Apply()
    {
        var targetAspect = _targetAspectX / _targetAspectY;
        var windowAspect = (float)Screen.width / Screen.height;
        var scaleHeight = windowAspect / targetAspect;

        var rect = _camera.rect;

        if (scaleHeight < 1f)
        {
            rect.width = 1f;
            rect.height = scaleHeight;
            rect.x = 0f;
            rect.y = (1f - scaleHeight) * 0.5f;
        }
        else
        {
            var scaleWidth = 1f / scaleHeight;
            
            rect.width = scaleWidth;
            rect.height = 1f;
            rect.x = (1f - scaleWidth) * 0.5f;
            rect.y = 0f;
        }

        _camera.rect = rect;
    }
}