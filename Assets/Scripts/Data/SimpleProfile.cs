using System;
using System.Text;
using Newtonsoft.Json;

namespace Data
{
    [Serializable]
    public class SimpleProfile
    {
        public string name;
        public string gender;
        public string similarity;
        public string summary;

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"otherName: {name}");
            sb.Append($"gender: {gender}");
            sb.Append($"similarity: {similarity}");
            sb.Append($"profileSummary: {summary}");

            return sb.ToString();
        }
    }
}
