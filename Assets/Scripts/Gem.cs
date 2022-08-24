using UnityEngine;

public class Gem : MonoBehaviour
{
    #region Unity Event Functions

    protected void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<PlayerCharacter>(out _)) return;
        
        Game.Instance.AddGem(1);
        Destroy(gameObject);
    }

    #endregion
}
