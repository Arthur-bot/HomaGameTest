using TMPro;
using UnityEngine;

public class FPSInfos : MonoBehaviour

{
    #region Fields

    [SerializeField] private TextMeshProUGUI _fpsText;
    [SerializeField] private float _updateInterval = 0.5f;

    private float _accum;
    private int _frames;
    private float _timeleft;

    #endregion

    #region Unity Events

    protected void Start()
    {
        _timeleft = _updateInterval;

        if (!Debug.isDebugBuild)
        {
            gameObject.SetActive(false);
        }
    }

    protected void Update()
    {
        _timeleft -= Time.deltaTime;
        _accum += Time.timeScale / Time.deltaTime;
        _frames++;

        if (!(_timeleft <= 0.0)) return;

        var fps = _accum / _frames;
        if (!float.IsNaN(fps))
        {
            _fpsText.text = fps.ToString("FPS: 00");
            _fpsText.color = fps < 30
                ? Color.yellow
                : fps < 15
                    ? Color.red
                    : Color.black;
        }

        _timeleft = _updateInterval;
        _accum = 0f;
        _frames = 0;
    }

    #endregion
}