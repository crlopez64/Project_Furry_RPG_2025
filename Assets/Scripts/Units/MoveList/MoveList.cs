using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that makes a Move List, be it full of Attacks, or full of Skills.
/// </summary>
public class MoveList
{
    protected readonly List<Attack> moveList;

    public MoveList()
    {
        moveList = new List<Attack>();
    }

    /// <summary>
    /// Return a Move List.
    /// </summary>
    /// <returns></returns>
    public List<Attack> GetMoveList()
    {
        return moveList;
    }

}
