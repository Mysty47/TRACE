using TMPro;
using UnityEngine;
using UnityEditor;


namespace Script.Texts
{
    public class StartingTimer : MonoBehaviour
    {
        public TextMeshProUGUI timerText;
        public float timer;
        
        void Start()
        {
            timer = 0f;
        }
        void Update()
        {
            timer += Time.deltaTime;

            int minutes = Mathf.FloorToInt(timer / 60);
            float seconds = timer % 60;

            timerText.text = $"{minutes:00}:{seconds:00.00}";
        }
    }
}