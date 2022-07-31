using UnityEngine;
using UnityEngine.UI;

public class UIMainMenu : MonoBehaviour
{
    #region Fields

    [SerializeField] private Image _moveTutorial;

    #endregion

    #region Public Methods

    public void Show(bool show)
    {
        if (show)
        {
            gameObject.SetActive(true);
        }
        else
        {
            Game.Instance.State = GameState.Playing;
            gameObject.SetActive(false);
        }
    }

    #endregion
}
