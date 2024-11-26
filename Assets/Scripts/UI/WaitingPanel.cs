using System;
using TMPro;
using UnityEngine;

namespace UI
{
    public class WaitingPanel: MonoBehaviour
    {
        [Header("Player Counter Members")]
        public GameObject counterMembers;
        public TMP_Text currentPlayerCnt;
        public TMP_Text maxPlayerCnt;
        
        [Header("Timer Members")]
        public GameObject timer;
        public TMP_Text timerText;


        
        public void ActiveCounterMembers()
        {
            counterMembers.SetActive(true);
            timer.SetActive(false);
        }
        
        public void ActiveTimer()
        {
            counterMembers.SetActive(false);
            timer.SetActive(true);
        }
        

        
        
        

        public void SetMaxPlayerCnt(int cnt)
        {
            this.maxPlayerCnt.text = cnt.ToString();
        }
        
        public void SetCurrentPlayerCnt(int cnt)
        {
            this.currentPlayerCnt.text = cnt.ToString();
        }


        public void SetCounterCnt(int cnt)
        {
            this.timerText.text = cnt.ToString();
        }
    }
}