using UnityEngine;

public class Obstacle : MonoBehaviour
{
    #region Unity Event Functions

    protected void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<CollectableCube>(out var cube))
        {
            Game.Instance.CurrentPlayer.RemoveItem(cube);
        }
    }

    #endregion
}
