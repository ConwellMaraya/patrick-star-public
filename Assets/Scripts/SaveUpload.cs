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
    public static async Task UploadJsonFileAsync(string filePath, string documentName, string supabaseUrl, string apiKey, string tableName)
    {
        UnityEngine.Debug.LogError("Running");
        var jsonString = await File.ReadAllTextAsync(filePath);

        var payload = new
        {
            name = documentName,
            data = Newtonsoft.Json.JsonConvert.DeserializeObject<object>(jsonString)
        };

        using var client = new HttpClient();
        client.BaseAddress = new Uri(supabaseUrl);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
        client.DefaultRequestHeaders.Add("apikey", apiKey);
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(payload));
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        var response = await client.PostAsync($"/rest/v1/{tableName}", content);

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
