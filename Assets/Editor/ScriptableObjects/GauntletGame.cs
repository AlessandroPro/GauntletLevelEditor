using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GauntletGame : ScriptableObject
{
    public List<GauntletLevel> levels = new List<GauntletLevel>();
    public List<string> strings = new List<string>();

    // Move the level at levelIndex up/down the level list
    // The levels load in their index order
    public int changeLevelOrder(int levelIndex, bool moveUp)
    {
        if(levelIndex < 0 || levelIndex >= levels.Count)
        {
            return levelIndex;
        }

        GauntletLevel tempLevel;
        int moveIndex = 0;

        if (moveUp)
        {
            moveIndex = levelIndex - 1;
            if (moveIndex < 0)
            {
                return levelIndex;
            }
        }
        else
        {
            moveIndex = levelIndex + 1;
            if (moveIndex >= levels.Count)
            {
                return levelIndex;
            }
        }

        tempLevel = levels[moveIndex];
        levels[moveIndex] = levels[levelIndex];
        levels[levelIndex] = tempLevel;

        return moveIndex;
    }
}
