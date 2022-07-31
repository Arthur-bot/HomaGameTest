using System.Collections;
using UnityEngine;

public class CollectableCube : MonoBehaviour
{
    #region Constantes

    #endregion

    #region Fields

    private Collider _collider;

    #endregion

    #region Properties

    public bool IsCollectable { get; set; } = true;

    public bool SimulateGravity { get; set; }

    public float MinY { get; set; }

    #endregion

    #region Public Methods

    public IEnumerator Throw(Transform endTarget, Vector3 endLocalOffset, float jumpHeight, float duration)
    {
        transform.parent = endTarget;
        var startPosition = transform.position;
        var percent = 0f;

        while (percent < 1f)
        {
            var endPosition = endTarget.TransformPoint(endLocalOffset);

            transform.position = Vector3.LerpUnclamped(startPosition, endPosition, percent)
                              - jumpHeight * 4f * percent * (percent - 1) * Vector3.up;

            yield return null;

            percent += Time.deltaTime / duration;
        }

        transform.position = endTarget.TransformPoint(endLocalOffset);
        _collider.isTrigger = false;
    }
    

    #endregion

    #region Unity Event Functions

    protected void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    protected void Update()
    {
        if (SimulateGravity)
        {
            var position = transform.localPosition + Vector3.down * 9.71f * Time.deltaTime;
            position.y = position.y < MinY ? MinY : position.y;

            transform.localPosition = position;
        }
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (IsCollectable && other.TryGetComponent<PlayerCharacter>(out var player))
        {
            player.AddItem(this);
        }
    }

    #endregion
}