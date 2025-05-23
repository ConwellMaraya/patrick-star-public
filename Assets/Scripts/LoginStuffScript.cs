using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Supabase.Gotrue;
using Supabase.Gotrue.Exceptions;
using TMPro;
using System.Text.Encodings.Web;
using System.ComponentModel;
using UnityEngine.Windows;

public class LoginStuffScript : MonoBehaviour
{
    public static LoginStuffScript Instance;
    public string _pkce;
    public string _token;
    public string userSaveName;
    public string playerId;
    public string projectUrl;
    public string apiKey;
    public string tableName = "saveStorage";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Prevent duplicates
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
