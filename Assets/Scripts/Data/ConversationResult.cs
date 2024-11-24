using System;

namespace Data
{
    [Serializable]
    public class ConversationResult
    {
        public string message;
        public Data data;
    }

    [Serializable]
    public class Data
    {
        public string summary;
        public string sentiment;
    }
}