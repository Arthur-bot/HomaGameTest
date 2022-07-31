using UnityEngine;

public class Level : MonoBehaviour
{
    #region Fields
    
    private int _index;

    #endregion
    
    #region Properties

    public static Level Current { get; private set; }

    public int Index => _index < 0
        ? _index = int.Parse(name) - 1
        : _index;

    public int CurrentGemInLevel { get; set; }
    
    public int Multiplier { get; set; }
    
    #endregion
    
    #region Public Methods

    public static void Load(Level level)
    {
        DestroyCurrent();

        var map = Instantiate(level);
        map.name = level.name;
    }
    
    public static void DestroyCurrent()
    {
        if (Current != null)
        {
            DestroyImmediate(Current.gameObject);
            Current = null;
        }
    }

    public void EndLevel(bool victory)
    {
        var game = Game.Instance;
        game.State = victory ? GameState.Victory : GameState.Defeat;
        
        if (victory)
        {
            game.AddGem(Multiplier * CurrentGemInLevel);
        }
    }

    #endregion
    
    #region Unity Event Functions

    protected void Awake()
    {
        if (Current != null)
        {
            Destroy(gameObject);
            return;
        }

        Current = this;
    }

    #endregion
}