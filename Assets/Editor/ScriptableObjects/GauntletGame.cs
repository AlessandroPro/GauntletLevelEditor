using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class GauntletGame : ScriptableObject
{
    public List<GauntletLevel> levels = new List<GauntletLevel>();
    public Player playerObject;
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

    public void saveGameData(string destFolder)
    {
        GauntletLevel.savedAssets = new HashSet<string>();

        Game.LevelList levelFiles = new Game.LevelList();

        int index = 0;
        foreach(var level in levels)
        {
            level.saveLevel(destFolder, index, playerObject);
            string levelPath = "../Assets/Levels/level_" + index.ToString() + ".json";
            levelFiles.levelFiles.Add(levelPath);
            index++;
        }

        var json = JsonConvert.SerializeObject(levelFiles);
        StreamWriter writer = new StreamWriter(destFolder + "/Levels/GameLevelList.json");
        writer.Write(json);
        writer.Close();
    }
}

