using UnityEngine;

public class HeadBobController : MonoBehaviour
{
    [SerializeField] private bool _enabled = true;

    [SerializeField, Range(0, 0.1f)] private float _amplitude = 0.015f;
    [SerializeField, Range(0, 30f)] private float _frequency = 10.0f;

    [SerializeField] private Transform _camera = null;
    [SerializeField] private Transform _cameraHolder = null;

    private float _toggleSpeed = 3.0f;
    private Vector3 _startPos;

    private Vector3 _lastPos;
    private Vector3 _velocity;

    private void Awake()
    {
        _startPos = _camera.localPosition;
        _lastPos = transform.position; // запомняме първа позиция
    }

    void Update()
    {
        if (!_enabled) return;

        // смятаме скоростта ръчно
        _velocity = (transform.position - _lastPos) / Time.deltaTime;
        _lastPos = transform.position;

        CheckMotion();
        ResetPosition();
        _camera.LookAt(FocusTarget());
    }

    private Vector3 FootStepMotion()
    {
        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Sin(Time.time * _frequency) * _amplitude;
        pos.x += Mathf.Cos(Time.time * _frequency / 2) * _amplitude * 2;
        return pos;
    }

    private void CheckMotion()
    {
        float speed = new Vector3(_velocity.x, 0, _velocity.z).magnitude;

        if (speed < _toggleSpeed) return;
        if (!IsGrounded()) return;

        PlayMotion(FootStepMotion());
    }

    private void PlayMotion(Vector3 motion)
    {
        _camera.localPosition += motion;
    }

    private void ResetPosition()
    {
        if (_camera.localPosition == _startPos) return;
        _camera.localPosition = Vector3.Lerp(_camera.localPosition, _startPos, Time.deltaTime * 5f);
    }

    private Vector3 FocusTarget()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + _cameraHolder.localPosition.y, transform.position.z);
        pos += _cameraHolder.forward * 15.0f;
        return pos;
    }

    private bool IsGrounded()
    {
        // проста проверка дали player-а е на земята
        return Physics.Raycast(transform.position, Vector3.down, 1.2f);
    }
}
