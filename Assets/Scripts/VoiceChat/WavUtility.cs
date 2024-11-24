using System;
using System.IO;

namespace VoiceChat
{
    public static class WavUtility
    {
        public static byte[] ConvertAudioDataToWav(float[] audioData, int sampleRate, int channels)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                // WAV 헤더 작성
                int byteRate = sampleRate * channels * 2; // 16비트(2바이트) 샘플
                int fileSize = 36 + audioData.Length * 2;

                // RIFF 헤더
                WriteString(memoryStream, "RIFF");
                WriteInt(memoryStream, fileSize);
                WriteString(memoryStream, "WAVE");

                // fmt 서브청크
                WriteString(memoryStream, "fmt ");
                WriteInt(memoryStream, 16); // 서브청크 크기
                WriteShort(memoryStream, 1); // 오디오 포맷 (1 = PCM)
                WriteShort(memoryStream, (short)channels);
                WriteInt(memoryStream, sampleRate);
                WriteInt(memoryStream, byteRate);
                WriteShort(memoryStream, (short)(channels * 2)); // 블록 얼라인
                WriteShort(memoryStream, 16); // 비트 수

                // data 서브청크
                WriteString(memoryStream, "data");
                WriteInt(memoryStream, audioData.Length * 2);

                // 오디오 데이터 작성
                byte[] bytesData = new byte[audioData.Length * 2];
                int rescaleFactor = 32767; // to convert float to Int16

                for (int i = 0; i < audioData.Length; i++)
                {
                    short value = (short)(audioData[i] * rescaleFactor);
                    byte[] bytes = BitConverter.GetBytes(value);
                    bytes.CopyTo(bytesData, i * 2);
                }

                memoryStream.Write(bytesData, 0, bytesData.Length);

                return memoryStream.ToArray();
            }
        }

        private static void WriteString(Stream stream, string value)
        {
            byte[] bytes = System.Text.Encoding.ASCII.GetBytes(value);
            stream.Write(bytes, 0, bytes.Length);
        }

        private static void WriteInt(Stream stream, int value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            stream.Write(bytes, 0, bytes.Length);
        }

        private static void WriteShort(Stream stream, short value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            stream.Write(bytes, 0, bytes.Length);
        }
    }
}