using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Newtonsoft.Json.Linq;

public class ServerSaveHandling
{
    public static async Task UploadJsonFileAsync(string filePath, string documentName, string supabaseUrl, string apiKey, string tableName,string guid)
    {
        UnityEngine.Debug.LogError("Running");
        var jsonString = await File.ReadAllTextAsync(filePath);
        Guid temp = new Guid(guid);

        var payload = new
        {
            id = temp,
            created_at = DateTime.UtcNow.ToString("o"),
            saveGame = Newtonsoft.Json.JsonConvert.DeserializeObject<object>(jsonString)
        };

        using var client = new HttpClient();
        client.BaseAddress = new Uri($"{supabaseUrl}/rest/v1/");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
        client.DefaultRequestHeaders.Add("apikey", apiKey);

        var request = new HttpRequestMessage(HttpMethod.Delete, $"{tableName}?id=eq.{temp}");
        var response = await client.SendAsync(request);

        UnityEngine.Debug.LogError($"Status: {response.StatusCode}");
        var deleteContent = await response.Content.ReadAsStringAsync();
        UnityEngine.Debug.LogError(deleteContent);

        client.BaseAddress = new Uri(supabaseUrl);
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(payload));
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        response = await client.PostAsync($"/rest/v1/{tableName}", content);

        if (response.IsSuccessStatusCode)
        {
            UnityEngine.Debug.LogError("✅ JSON file uploaded successfully.");
        }
        else
        {
            string error = await response.Content.ReadAsStringAsync();
            UnityEngine.Debug.LogError($"❌ Upload failed: {response.StatusCode}\n{error}");
        }
    }

    public static async Task DownloadRowAsJsonAsync(string filePath, string documentName, string supabaseUrl, string apiKey, string table, string uuid)
    {
        string column = "id";

        string url = $"{supabaseUrl}/rest/v1/{table}?{column}=eq.{uuid}&select=saveGame";
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Clear();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
        client.DefaultRequestHeaders.Add("apikey", apiKey);
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        HttpResponseMessage response = await client.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            string jsonData = await response.Content.ReadAsStringAsync();

            try
            {
                var array = JArray.Parse(jsonData);
                var saveGameJson = array[0]?["saveGame"];

                if (saveGameJson != null)
                {
                    // Write only the value of saveGame, as JSON
                    UnityEngine.Debug.LogError($"✅ Saved raw saveGame JSON from server to {filePath}");
                    File.WriteAllText(filePath, saveGameJson.ToString());
                    
                }
                else
                {
                    UnityEngine.Debug.LogError("⚠️ No saveGame data found in row.");
                }
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError("❌ Failed to parse saveGame JSON: " + ex.Message);
            }
        }
        else
        {
            UnityEngine.Debug.LogError($"❌ Failed to fetch: {response.StatusCode}");
        }
  
    }
}
