using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : Singleton<GameUI>
{
    #region Fields
    
    [SerializeField] private UIPlayerInput _playerInput;
    [SerializeField] private Image _fade;
    [SerializeField] private UIEndScreenPopup _endScreen;
    [SerializeField] private UIMainMenu _mainMenu;
    [SerializeField] private TextMeshProUGUI _gemText;

    private Game _game;
    
    #endregion

    #region Properties

    public UIPlayerInput PlayerInput => _playerInput;

    public UIEndScreenPopup EndScreen => _endScreen;
    
    public UIMainMenu MainMenu => _mainMenu;
    
    public TextMeshProUGUI GemText => _gemText;

    #endregion
    
    #region Public Methods

    public TweenerCore<Color, Color, ColorOptions> Fade(bool isIn, float duration = 0.3f)
        => _fade.DOFade(isIn ? 1f : 0f, duration)
            .SetEase(isIn ? Ease.OutSine : Ease.InQuart);

    #endregion
    
    #region Unity Event Functions

    protected override void OnAwake()
    {
        _game = Game.Instance;
        _game.StateChanged += GameOnStateChanged;
        
        Fade(false, 0.5f).From(1f);
    }

    protected override void OnDestroy()
    {
        _game.StateChanged -= GameOnStateChanged;
    }

    #endregion
    
    #region Event Handler

    private void GameOnStateChanged(Game sender)
    {
        switch (sender.State)
        {
            case GameState.Playing:
                break;
            case GameState.Victory:
                _endScreen.Show(true, Level.Current.Multiplier * Level.Current.CurrentGemInLevel);
                break;
            case GameState.Defeat:
                _endScreen.Show(false);
                break;
            case GameState.Menu:
                _mainMenu.Show(true);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    #endregion
}
