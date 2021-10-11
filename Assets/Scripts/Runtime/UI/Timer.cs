using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace HuntroxGames.LD49
{
    public class Timer : MonoBehaviour
    {
        private TextMeshProUGUI text;
        private float timer;
        void Start()
        {
            text = GetComponent<TextMeshProUGUI>();
        }

        void Update()
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
                var ti = System.TimeSpan.FromSeconds(timer);
                text.text = string.Format("Will be Availabe in {0}:{1}", ti.Minutes, ti.Seconds);
			}else
			{
                text.text = "";
            }
        }

        public void SetTimer(float time)
		{
            timer = time;
        }
    }
}
