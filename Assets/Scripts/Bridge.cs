using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour
{
    #region Constante

    private const float DelayFill = 0.1f;

    #endregion
    
    #region Fields

    [SerializeField] private List<Transform> _targets;

    private bool _fillCoroutineRunning;

    #endregion

    #region Unity Event Functions

    protected void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerCharacter>(out var player) && !_fillCoroutineRunning)
        {
            StartCoroutine(FillCoroutine(player));
        }
    }

    #endregion

    #region Private Methods

    private IEnumerator FillCoroutine(PlayerCharacter player)
    {
        _fillCoroutineRunning = true;
        
        foreach (var target in _targets)
        {
            player.ThrowCube(target);

            yield return new WaitForSeconds(DelayFill);
        }
    }

    #endregion
}
