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
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public class SupabaseGoogleSignIn : MonoBehaviour
{
    // Start is called before the first frame update
    private string RedirectUrl = "http://*:3000/";
    [SerializeField] UI_FadeScreen fadeScreen;

    public TMP_Text ErrorText;
    private Supabase.Client supabaseClient;

    [SerializeField] private bool _doSignIn;
    [SerializeField] private bool _doSignOut;
    [SerializeField] private bool _doExchangeCode;

    private HttpListener? httpListener;
    private string _pkce;
    private string _token;
    public GameObject LoginDetails;

    


    async void Start()
    {
        supabaseClient = await initializeSupabase();
    }

    private async Task<Supabase.Client> initializeSupabase()
    {
        string supabaseURL = "https://ziluhantkwazbkwbypzv.supabase.co";
        string supabaseKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InppbHVoYW50a3dhemJrd2J5cHp2Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3NDc2NDA0NjMsImV4cCI6MjA2MzIxNjQ2M30.-6JEJ8nraBFnAGQRTsqa7fLjkgedTJs_IBloRnsKsPw";

        var clientOption = new Supabase.SupabaseOptions
        {
            AutoRefreshToken = true,
            AutoConnectRealtime = true
           
        };
        var client = new Supabase.Client(supabaseURL, supabaseKey, clientOption);
        await client.InitializeAsync();
        Debug.Log("SUPABASE WORKS");
        return client;
    }

    public void signIn()
    {
        _doSignIn = true;
    }

    public void signOut()
    {
        
        _doSignOut = true;
    }


    private async void Update()
    {
        if (_doSignOut)
        {
            _doSignOut = false;
            await supabaseClient.Auth.SignOut();
        }

        if (_doExchangeCode)
        {
            Debug.Log("WHERE MY ROSEMARY GOES");
            _doExchangeCode = false;
            await PreformExchangeCode();
        }

        if (_doSignIn)
        {
            Debug.Log("OH BUT LOVE GROWS");
            _doSignIn = false;
            await PreformSignIn();
        }
    }

    private void StartLocalWebServer()
    {
        if (httpListener == null)
        {
            httpListener = new HttpListener();
            httpListener.Prefixes.Add(RedirectUrl);
            httpListener.Start();
            if (httpListener != null)
                Debug.Log("EXISTS");
            httpListener.BeginGetContext(new AsyncCallback(IncomingHttpRequest),httpListener);
        }
    }

    private void IncomingHttpRequest(IAsyncResult result)
    {
        Debug.Log("IncomingHttpRequest");

        HttpListener httpListener;
        HttpListenerContext httpContext;
        HttpListenerRequest httpRequest;
        HttpListenerResponse httpResponse;
        string responseString;

        // Get back the reference to our http listener
        httpListener = (HttpListener)result.AsyncState;

        // Fetch the context object
        httpContext = httpListener.EndGetContext(result);

        // The context object has the request object for us, that holds details about the incoming request
        httpRequest = httpContext.Request;

        _token = httpRequest.QueryString.Get("code");

        // Build a response to send an "ok" back to the browser for the user to see
        httpResponse = httpContext.Response;
        responseString = "<html><body><b>DONE!</b><br>(You can close this tab/window now)</body></html>";
        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);

        // Send the output to the client browser
        httpResponse.ContentLength64 = buffer.Length;
        System.IO.Stream output = httpResponse.OutputStream;
        output.Write(buffer, 0, buffer.Length);
        output.Close();

        httpListener.Stop();
        httpListener = null;
        _doExchangeCode = true;
    }

    private async Task PreformSignIn()
    {
        try
        {

            var providerAuth = await supabaseClient.Auth.SignIn(Constants.Provider.Google, new SignInOptions
            {
                FlowType = Constants.OAuthFlowType.PKCE,
            });


            _pkce = providerAuth.PKCEVerifier;
            StartLocalWebServer();
            Application.OpenURL(providerAuth.Uri.ToString());
            Debug.LogError("Got It");
                   
        }

        catch (GotrueException goTrueException)
        {
            ErrorText.text = $"{goTrueException.Reason} {goTrueException.Message}";
            Debug.LogError(goTrueException.Message);
        }

        catch (Exception e)
        {
            ErrorText.text = $"Error {e.Message}";
            Debug.LogError(e.Message);
        }
    }

    private async Task PreformExchangeCode()
    {
        
        try
        {
            var session = await supabaseClient.Auth.ExchangeCodeForSession(_pkce, _token);

            if (session != null)
            {
                Debug.Log(session.ExpiresIn);
                string userJson = JsonConvert.SerializeObject(session.User);
                Debug.Log($"User Json: {userJson}");
                string startstr = "\"full_name\":\"";
                string endstr = "\",\"iss\"";
                int start = userJson.IndexOf(startstr);
                int end = userJson.IndexOf(endstr);
                string res = userJson.Substring(start + startstr.Length, end - (start + startstr.Length));
                Debug.Log(res);
                LoginDetails.GetComponent<LoginStuffScript>()._pkce = _pkce;
                LoginDetails.GetComponent<LoginStuffScript>()._token = _token;
                LoginDetails.GetComponent<LoginStuffScript>().userSaveName = ReplaceWhitespace(res,"_");
                startstr = "\"id\":\"";
                endstr = "\",\"identities\":";
                start = userJson.IndexOf(startstr);
                end = userJson.IndexOf(endstr);
                res = userJson.Substring(start + startstr.Length, end - (start + startstr.Length));
                Debug.Log(res);
                LoginDetails.GetComponent<LoginStuffScript>().playerId = ReplaceWhitespace(res, "_");
                StartCoroutine(LoadSceneWithFadeEffect(1.5f));

            }
        }

        catch (GotrueException goTrueException)
        {
            ErrorText.text = $"{goTrueException.Reason} {goTrueException.Message}";
            Debug.LogError(goTrueException.Message);
        }
    }

    IEnumerator LoadSceneWithFadeEffect(float _delay)
    {
        fadeScreen.FadeOut();

        yield return new WaitForSeconds(_delay);

        SceneManager.LoadScene("MainMenu");
    }
    private static readonly Regex sWhitespace = new Regex(@"\s+");
    public static string ReplaceWhitespace(string input, string replacement)
    {
        return sWhitespace.Replace(input, replacement);
    }
}
