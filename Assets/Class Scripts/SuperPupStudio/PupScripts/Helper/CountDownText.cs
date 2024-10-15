using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SuperPupSystems.Helper
{
    public class CountDownText : MonoBehaviour
    {
        public string textHeader = "Timeleft : ";
        public Timer timer;
        public float time { get { return timer.timeLeft; } }
        public float startTime = 30.0f;
        public TMP_Text text;
        void Start()
        {
            if (timer == null)
            {
                Debug.LogError("Player, your " + gameObject.name + " is up!");
            }
            StartCountDownTimer();
        }

        void Update()
        {
            text.text = textHeader + Mathf.Ceil(time);
        }

        public void StartCountDownTimer()
        {
            timer.StartTimer(startTime, false);
        }

        public void AddTime(float time)
        {
            timer.AddTime(time);
        }
    }
}