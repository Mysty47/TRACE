using TMPro;
using UnityEngine;

namespace Script.Texts
{
    public class StartingTimer : MonoBehaviour
    {
        public TextMeshProUGUI timerText;
        public float timer;
        
        void Update()
        {
            timer = Time.time;

            int minutes = Mathf.FloorToInt(timer / 60);
            float seconds = timer % 60;

            timerText.text = $"{minutes:00}:{seconds:00.00}";
        }
    }
}