using System.Text;
using Newtonsoft.Json;

namespace Data
{
    public class SimpleProfile
    {
        [JsonProperty("other_name")] public string name;
        [JsonProperty("other_gender")] public string gender;
        [JsonProperty("similarity")] public string similarity;
        [JsonProperty("profile_summary")] public string summary;

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
