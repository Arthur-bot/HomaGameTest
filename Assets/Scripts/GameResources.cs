using System.Collections.Generic;
using UnityEngine;

public class GameResources : Singleton<GameResources>
{
    #region Fields

    [SerializeField] private List<Level> _levels;

    #endregion

    #region Properties

    public List<Level> Levels => _levels;

    #endregion
}