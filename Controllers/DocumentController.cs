using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models;
[ApiController]
[Route("api")]
public class DocumentController : ControllerBase
{

    [HttpGet("document")]
    public IActionResult ProcessWord([FromBody] SnipWord snipWord)
    {
        if (snipWord == null || string.IsNullOrEmpty(snipWord.DocumentPath))
        {
            return BadRequest("Document path is required.");
        }

        string path = snipWord.DocumentPath.Replace("\\", "/");
        string documentText = string.Empty;

        try
        {
            using (StreamReader sr = new StreamReader(path))
            {
                documentText = sr.ReadToEnd();
            }
        }
        catch (Exception ex)
        {
            return BadRequest($"Error reading document: {ex.Message}");
        }

        var snippets = GetSnippets(documentText, snipWord.Word);

        return Ok(new { snippets });
    }

    private List<string> GetSnippets(string text, string word)
    {
        var snippets = new List<string>();
        if (string.IsNullOrEmpty(word)) return snippets;

        string lowerText = text.ToLower();
        string lowerWord = word.ToLower();
        int wordLength = word.Length;
        int snippetLength = 100; // Length of snippet before and after the word

        int index = lowerText.IndexOf(lowerWord);
        while (index != -1)
        {
            int start = Math.Max(0, index - snippetLength);
            int end = Math.Min(text.Length, index + wordLength + snippetLength);
            string snippet = text.Substring(start, end - start);

            // Highlight the word in the snippet
            snippet = snippet.Replace(word, $"<b>{word}</b>", StringComparison.OrdinalIgnoreCase);

            snippets.Add(snippet);

            index = lowerText.IndexOf(lowerWord, index + wordLength);
        }

        return snippets;
    }
}