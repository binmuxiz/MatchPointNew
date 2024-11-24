
namespace Data
{
    public class VoiceData
    {
        public string sessionId;
        public string userId;
        public string timestamp;
        public byte[] audioData;
        public string fileName;

        public VoiceData(string sessionId, string userId, string timestamp, byte[] audioData, string fileName)
        {
            this.sessionId = sessionId;
            this.userId = userId;
            this.timestamp = timestamp;
            this.audioData = audioData;
            this.fileName = fileName;
        }

    }
}