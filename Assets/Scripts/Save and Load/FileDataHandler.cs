using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

public class FileDataHandler 
{
    private string dataDirPath = "";
    private string dataFileName = "";
    private string codeWord = "pintados";
    public GameObject Login;


    public FileDataHandler(string _dataDirPath, string _dataFileName)
    {
        dataDirPath = _dataDirPath;
        dataFileName = _dataFileName;
    }

    public void Save(GameData _data)
    {
        Login = GameObject.Find("LoginStuff");
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = JsonConvert.SerializeObject(_data,Formatting.Indented);

            File.WriteAllText(fullPath, dataToStore);
            
        }

        catch(Exception e)
        {
            Debug.LogError("Error on trying to save data to file: " + fullPath + "\n" + e);
        }
    }

    public GameData Load()
    {
        string fullPath = dataDirPath + "/" + dataFileName;
        GameData loadData = null;

        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = File.ReadAllText(fullPath);

                Debug.LogError(dataToLoad);
                loadData = JsonConvert.DeserializeObject<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("Error on trying to load data from file:" + fullPath + "\n" + e);
            }
        }


        return loadData;

    }

    public void Delete()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        if(File.Exists(fullPath))
            File.Delete(fullPath);
    }

    private string EncryptDecrypt(string _data)
    {
        string modifiedData = "";

        for (int i = 0; i < _data.Length; i++)
        {
            modifiedData += (char)(_data[i] ^ codeWord[i % codeWord.Length]);
        }

        return modifiedData;

    }

    private static readonly Regex sWhitespace = new Regex(@"\s+");
    public static string ReplaceWhitespace(string input, string replacement)
    {
        return sWhitespace.Replace(input, replacement);
    }
}
