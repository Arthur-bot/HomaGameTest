using System.Collections;
using DG.Tweening;
using UnityEngine;

public enum GameState
{
    Playing,
    Victory,
    Defeat,
    Menu
}

public class Game : Singleton<Game>
{
    #region Fields

    [SerializeField] private PlayerCharacter _playerPrefab;
    
    private GameState _state;

    private int _gem;


    #endregion

    #region Properties

    public int CurrentLevelIndex => Level.Current.Index;
    
    private int Gem
    {
        get => _gem;
        set
        {
            if (_gem == value) return;

            _gem = value;
            GameUI.Instance.GemText.text = _gem.ToString();
        }
    }

    public GameState State
    {
        get => _state;
        set
        {
            if (_state == value) return;

            _state = value;

            StateChanged?.Invoke(this);
        }
    }

    public PlayerCharacter CurrentPlayer { get; private set; }

    #endregion
    
    #region Event
    
    public delegate void EventHandler(Game sender);
    public event EventHandler StateChanged;

    #endregion

    #region Public Methods

    public void LoadLevel(int index)
    {
        var levels = GameResources.Instance.Levels;
        
        if (index >= levels.Count)
        {
            index = 0;
        }
        
        var level = levels[index];
        
        StartCoroutine(Coroutine());
        IEnumerator Coroutine()
        {
            yield return GameUI.Instance.Fade(true).WaitForCompletion();

            yield return null;

            Level.Load(level);

            if (CurrentPlayer != null)
            {
                Destroy(CurrentPlayer.gameObject);
            }

            CurrentPlayer = Instantiate(_playerPrefab, Vector3.zero, Quaternion.identity, transform);
            TargetCamera.Instance.Target = CurrentPlayer.transform;
            GameUI.Instance.PlayerInput.Player = CurrentPlayer;

            GameUI.Instance.Fade(false).SetDelay(0.5f); 
            
            State = GameState.Menu;
        }
    }

    public void AddGem(int count)
    {
        Gem += count;

        if (State == GameState.Playing)
        {
            Level.Current.CurrentGemInLevel += count;
        }
    }

    #endregion

    #region Unity Event Functions

    protected void Start()
    {
        LoadLevel(0);
        
        Gem = 0;
        State = GameState.Menu;
    }

    #endregion
}