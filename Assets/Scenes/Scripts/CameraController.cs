using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GridManager _gridManager;
    [SerializeField] private float cameraOffset;
    [SerializeField] private float padding = 2;
    [SerializeField] private float aspectRatio = 0.625f;

    private void Start()
    {
        _gridManager = FindObjectOfType<GridManager>();
        if (_gridManager != null)
            SetCameraPosition(_gridManager.width - 1, _gridManager.height - 1);
    }

    private void SetCameraPosition(float x, float y)
    {
        Vector3 tempPos = new Vector3(x / 2, y / 2, cameraOffset);
        transform.position = tempPos;
        if(_gridManager.width > _gridManager.height)
            Camera.main.orthographicSize = ((float)_gridManager.width / 2 + padding) / aspectRatio + 1;  
        else
            Camera.main.orthographicSize = (float)_gridManager.height / 2 + padding;  
    }
}
