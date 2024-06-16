using System.Collections.Generic;

namespace Core.Model
{
    public class SnippetSentence
    {
        public int ID { get; set; }
        public string WordSearch { get; set; }
        public List<string> Sentences { get; set; }
    }
}