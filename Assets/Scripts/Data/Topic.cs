using System;

namespace Data
{
    [Serializable]
    public class Topic
    {
        public string topic;

        public Topic(string topic)
        {
            this.topic = topic;
        }
    }
}