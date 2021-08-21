using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace haruroad.szd.singleplayer
{
    public class Timer : MonoBehaviour
    {
        public bool isTime;

        private int time = 10;
        private Text text;
        private WaitForSeconds waitTime = new WaitForSeconds(1.0f);
        public UiManager ui;
        public GameManager gm;
        // Start is called before the first frame update
        public void Awake()
        {
            text = GetComponent<Text>();
            time = 60;
        }
        private void OnEnable()
        {
            ReStart();
            StartCoroutine("StartTime");
        }
        IEnumerator StartTime()
        {
            while (true)
            {
                yield return waitTime;
                if (time <= 0)
                {
                    ui.istimeCheck = false;
                    gm.nextGame = false;
                    break;
                }
                else
                {
                    time--;
                }
                text.text = time.ToString();
            }
            Debug.LogError(isTime);
        }
        private void ReStart()
        {
            time = 10;
            isTime = false;
        }
    }
}
