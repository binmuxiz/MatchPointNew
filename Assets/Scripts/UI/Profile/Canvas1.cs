using TMPro;
using UnityEngine;

namespace UI.Profile
{
    public class Canvas1 : MonoBehaviour, IDetailNextButton
    {
        public TMP_InputField[] inputFields;
        
        private string[] subjects = new string[] {"이름","생년월일(YYYY-MM-DD)","성별(여/남)","주소" };

        private void Start()
        {
            Clear();
        }

        public void DetailNextButton()
        {
            Data.Profile request = ProfileInput.Instance.profile;
            
            request.user.name = inputFields[0].text;
            request.user.birthDate = inputFields[1].text;
            request.user.gender = inputFields[2].text;
            request.user.address  = inputFields[3].text;
        
            Clear();
        }

        void Clear()
        {
            for (int i = 0; i < inputFields.Length; i++)
            {
                inputFields[i].text = "";
                inputFields[i].GetComponentInChildren<TMP_Text>().text = subjects[i]; 
            }
        }
    }
}
