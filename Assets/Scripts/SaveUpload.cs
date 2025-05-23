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

public class SaveUpload
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
        client.BaseAddress = new Uri(supabaseUrl);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
        client.DefaultRequestHeaders.Add("apikey", apiKey);
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var request = new HttpRequestMessage(HttpMethod.Delete, $"{tableName}?id=eq.{temp}");
        var response = await client.SendAsync(request);

        UnityEngine.Debug.LogError($"Status: {response.StatusCode}");
        var deleteContent = await response.Content.ReadAsStringAsync();
        UnityEngine.Debug.LogError(deleteContent);

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
}
