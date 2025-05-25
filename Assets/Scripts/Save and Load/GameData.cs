using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int currency;

    public SerializableDictionary<string, bool> skillTree;
    public SerializableDictionary<string, int> inventory;
    public List<string> equipmentId;


    public SerializableDictionary<string, bool> checkpoints;
    public string closestCheckpointId;

    public float lostCurrencyX;
    public float lostCurrencyY;
    public int lostCurrencyAmount;

    public int[] levelArrangement;
    public int currLevel;
    public int levelCounter;

    public SerializableDictionary<string, float> volumeSettings;

    public GameData()
    {
        this.lostCurrencyX = 0;
        this.lostCurrencyY = 0;
        this.lostCurrencyAmount = 0;


        this.currency = 0;
        skillTree = new SerializableDictionary<string, bool>();
        inventory = new SerializableDictionary<string, int>();
        equipmentId = new List<string>();

        closestCheckpointId = string.Empty;
        checkpoints = new SerializableDictionary<string, bool>();

        volumeSettings= new SerializableDictionary<string, float>();

        levelArrangement  = new int[3];
        levelCounter = 0;
        currLevel = 0;
        
        for (int i = 0; i < 3; i++)
        {
            System.Random random = new System.Random();
            int levelNum = random.Next(3) + 1;
            if (!levelArrangement.Contains(levelNum))
            {
                levelArrangement[i] = levelNum;
            }
            else
            {
                i--;
            }
        }
    }
}
