using System;
using Newtonsoft.Json;

namespace Data
{   
    [Serializable]
    public class Profile
    {
        public User user;
        [JsonProperty("ideal_partner")]
        public IdealType idealType;

    }
    
    [Serializable]
    public class User
    {
               
        public string name;         // 이름
        [JsonProperty("birth_date")]
        public string birthDate;
        public string gender;       // 성별 
        public string address;      // 주소

        public string education;    // 학력
        public string divorced;   // 돌싱/자녀 여부
        public string mbti;         // mbti
        public string religion;       // 종교
        public string smoking;      // 흡연
        public string drinking;     // 음주
        public int height;          // 키
        [JsonProperty("body_type")]
        public string bodyType;     // 체형
        
        public string[] personality;  // 성격
        public string[] appearance;   // 외모
        public string[] skills;        // 능력
        public string[] hobbies;        // 피트니스, 야외활동, 문화생활, 자기관리
    }


    [Serializable]
    public class IdealType
    {
        public string[] personality;  
        public string[] appearance;   
        public string[] skills;       
        public string[] hobbies;
    }
}