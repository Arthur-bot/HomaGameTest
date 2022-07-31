using System.Collections;
using UnityEngine;

public class TargetCamera : Singleton<TargetCamera>
{
    #region Fields

    [SerializeField] private Vector3 _offset;
    [SerializeField] private float _smoothSpeed;

    #endregion

    #region Properties

    public Transform Target { get; set; }

    #endregion

    #region Unity Event Functions

    protected void LateUpdate()
    {
        if (Target == null) return;

        var targetPosition = Target.position + _offset;
        var position = Vector3.Lerp(transform.position, targetPosition, _smoothSpeed);
        position.x = 0;

        transform.position = position;
    }

    #endregion

    #region Private Methods

    

    #endregion
}
