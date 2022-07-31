using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIEndScreenPopup : MonoBehaviour
{
    #region Fields

    [SerializeField] private TextMeshProUGUI _header;
    [SerializeField] private GameObject _gemGameObject;
    [SerializeField] private TextMeshProUGUI _gemCount;
    [SerializeField] private Button _nextButton;
    [SerializeField] private Button _retryButton;

    #endregion

    #region Public Methods

    public void Show(bool victory, int gem = 0)
    {
        _header.text = victory ? "Level\n<b><color=green>completed" : "Level\n<b><color=red>Failed";
        _nextButton.gameObject.SetActive(victory);
        _retryButton.gameObject.SetActive(!victory);
        
        _gemCount.text = $"+{gem}";
        _gemGameObject.SetActive(gem > 0);
        
        gameObject.SetActive(true);
    }

    #endregion

    #region Protected Methods

    protected void Awake()
    {
        var game = Game.Instance;
        
        _nextButton.onClick.AddListener(() =>
        {
            game.LoadLevel(game.CurrentLevelIndex + 1);
            gameObject.SetActive(false);
        });
        
        _retryButton.onClick.AddListener(() =>
        {
            game.LoadLevel(game.CurrentLevelIndex);
            gameObject.SetActive(false);
        });
    }

    #endregion
}
