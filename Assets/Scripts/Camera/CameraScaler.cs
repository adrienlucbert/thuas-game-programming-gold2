using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraScaler : MonoBehaviour
{
    private Camera __camera = null;
    private Camera _camera
    {
        get
        {
            if (this.__camera == null)
                this.__camera = this.GetComponent<Camera>();
            return this.__camera;
        }
    }

    public void Fit(float width, float height)
    {
        float halfSize = Mathf.Max(height, width / this._camera.aspect) / 2f;
        this._camera.orthographicSize = halfSize;
    }
}
