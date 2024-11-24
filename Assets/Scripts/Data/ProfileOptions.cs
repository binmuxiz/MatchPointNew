using System.Collections.Generic;

namespace Data
{
    public static class ProfileOptions
    {
        public static readonly string[] Education = new string[]
        {
            "고등학교 졸업", "전문대 재학 중", "전문대 졸업", "대학교 재학 중", "대학교 졸업", "대학원 재학 중", "대학원 졸업", "기타"
        };

        public static readonly string[] Height = new string[200 - 140 + 1];
        

        public static readonly string[] BodyType = new string[] { "마른", "슬림탄탄", "보통", "통통한", "살짝볼륨", "글래머" };

        public static readonly string[] Devorced = new string[] { "돌싱 아님", "돌싱/자녀 없음", "돌싱/자녀 비양육", "돌싱/자녀 양육" };

       
        public static readonly string[] MBTI = new string[]
        {
            "INTJ", "INTP", "ENTJ", "ENTP",
            "INFJ", "INFP", "ENFJ", "ENFP",
            "ISTJ", "ISFJ", "ESTJ", "ESFJ",
            "ISTP", "ISFP", "ESTP", "ESFP"
        };

        public static readonly string[] Religion = new string[] { "무교", "기독교", "불교", "천주교", "기타" };
       
        public static readonly string[] Smoking = new string[] { "비흡연", "담배 끊는 중", "가끔 펴요", "술 마실 때만", "전자담배", "흡연" };
   
        public static readonly string[] Drink = new string[] { "마시지 않음", "어쩔 수 없을 때만", "가끔 마심", "어느정도 즐김", "좋아하는 편", "술고래" };


        //hobby
    
        public static readonly string[] Exercising = new string[]
        {
            "골프", "농구", "러닝", "서핑", "수영", "스키/스노우보드/", "스킨스쿠버", "야구", "요가", "운동", "자전거",
            "축구", "크로스핏", "클라이밍", "테니스", "프리다이빙", "필라테스", "헬스"
        };

        public static readonly string[] OutActivity = new string[]
        {
            "낚시", "드라이브", "등산", "맛집 투어", "산책",
            "스포츠 관람", "여행", "캠핑", "파인다이닝"
        };

        public static readonly string[] CultureArt = new string[]
        {
            "게임", "공연관람", "공예", "그림그리기", "글쓰기", "노래", "댄스", "덕질", "독서",
            "드라마", "사진 촬영", "술", "악기 연주", "애니메이션", "영화 감상", "예능", "와인",
            "요리", "웹툰", "위스키", "음악 감상", "전시회 관람", "커피"
        };

        public static readonly string[] Life = new string[]
        {
            "반려동물", "봉사활동", "뷰티", "쇼핑", "외국어 공부", "인테리어",
            "자기 계발", "자기 관리", "자동차", "재테크", "패션", "SNS"
        };

    
    
        //Attract
        public static readonly string[] Personnality = new string[]
        {
            "공감능력 좋은", "귀여운", "긍정적인",
            "다정한", "대화가 잘 통하는", "동갑을 선호", "듬직한", "리액션이 좋은", "말을 예쁘게 하는", "배려심이 많은", "분위기 메이커", "생활력이 좋은", "섬세한", "센스있는",
            "솔직한", "술친구 할 수 있는",
            "애교가 많은", "어른스러운", "여사친/남사친 없음", "연락이 잘 되는", "연상을 선호", "연하를 선호",
            "예의가 바른", "유머러스한", "이야기를 잘 듣는", "자기계발을 꾸준히 하는", "자기관리를 잘 하는", "자신감이 있는",
            "잘 웃는", "장난기가 많은", "적극적인", "집돌이/집순이", "취미가 비슷한", "털털한", "티키타카 잘 되는",
            "편안하게 해주는", "표현을 잘하는", "허세있는", "활발한"
        };

        public static readonly string[] Appearance = new string[]
        {
            "강아지상", "고양이상", "곰상", "공룡상", "늑대상",
            "다람쥐상", "사막여우상", "사슴상", "토끼상", "웃는게 예쁜", "머리숱이 많은", "머릿결이 좋은", "눈썹이 짙은",
            "눈이 큰", "무쌍", "코가 오똑한", "입술이 도톰한", "치열이 고른", "목소리가 좋은", "보조개가 있는",
            "피부가 깨끗한", "동안", "비율이 좋은", "어깨가 직각인", "어깨가 넓은", "손이 예쁜", "복근이 있는",
            "골반/치골이 멋진", "힙업", "허벅지가 튼튼한", "다리가 예쁜"
        };

        public static readonly string[] Ability = new[]
        {
            "건물주", "게임을 잘 하는", "경제적 여유가 있는", "고기를 잘 굽는", "노래를 잘 하는", "두뇌가 섹시한", "맛집 리스트를 갖고 있는", "뭐든 잘 먹는", "사진을 잘 찍는",
            "연봉이 높은",
            "외국어를 잘 하는", "요리를 잘 하는", "운동을 좋아하는", "워라벨이 좋은", "자가를 보유",
            "자취를 하는", "체력이 좋은", "춤을 잘 추는", "패션 센스가 좋은"
        };

        public static void SetBaseProfileList(List<string[]> list)
        {
            list.Add(Education);
            list.Add(Height);
            list.Add(BodyType);
            list.Add(Devorced);
        }

        public static void SetBaseProfileList2(List<string[]> list)
        {
            list.Add(MBTI);
            list.Add(Religion);
            list.Add(Smoking);
            list.Add(Drink);
        }
        
        public static void SetHobbyInList(List<string[]> hobbyInList)
        {
            hobbyInList.Add(Exercising);
            hobbyInList.Add(OutActivity);
            hobbyInList.Add(CultureArt);
            hobbyInList.Add(Life);
        }
    
        public static void SetPaaInList(List<string[]> paaInList)
        {
            paaInList.Add(Personnality);
            paaInList.Add(Appearance);
            paaInList.Add(Ability);
        }
    }
}
