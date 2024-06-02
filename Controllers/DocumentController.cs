using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api")]
public class DocumentController : ControllerBase
{

    [HttpPost("document")]
    public IActionResult ProcessWord([FromBody] SnipWord snipWord)
    {
        if (snipWord == null || string.IsNullOrEmpty(snipWord.Sentence) || string.IsNullOrEmpty(snipWord.Word))
        {
            return BadRequest("Both sentence and word are required.");
        }

        string highlightedSentence = HighlightWordInSentence(snipWord.Sentence, snipWord.Word);

        return Ok(new { highlightedSentence });
    }

    private string HighlightWordInSentence(string sentence, string word)
    {
        string lowerSentence = sentence.ToLower();
        string lowerWord = word.ToLower();
        int wordLength = word.Length;
        string result = "";
        int i = 0;

        while (i < sentence.Length)
        {
            if (i + wordLength <= sentence.Length &&
                (i == 0 || !char.IsLetterOrDigit(sentence[i - 1])) &&
                lowerSentence.Substring(i, wordLength) == lowerWord &&
                (i + wordLength == sentence.Length || !char.IsLetterOrDigit(sentence[i + wordLength])))
            {
                result += $"<strong>{sentence.Substring(i, wordLength)}</strong>";
                i += wordLength;
            }
            else
            {
                result += sentence[i];
                i++;
            }
        }

        return result;
    }
}