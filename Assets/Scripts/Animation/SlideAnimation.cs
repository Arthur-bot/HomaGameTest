using DG.Tweening;
using UnityEngine;

public class SlideAnimation : MonoBehaviour
{
    [SerializeField] private float _endPositionX;
    [SerializeField] private float _duration;

    private Vector3 _startPosition;

    protected void Awake()
    {
        _startPosition = transform.localPosition;
    }

    protected void OnEnable()
    {
        transform.localPosition = _startPosition;
        transform.DOLocalMoveX(_endPositionX, _duration).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }
}