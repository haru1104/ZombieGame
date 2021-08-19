using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace haruroad.szd.singleplayer
{
    public class Timer : MonoBehaviour
    {
        private int time = 60;
        private Text text;
        // Start is called before the first frame update
        public void Awake()
        {
            text = GetComponent<Text>();
            time = 60;
        }

        private void Start()
        {
            StartCoroutine("StartTime");
        }
        
        IEnumerator StartTime()
        {
            yield return new WaitForSeconds(1f);
            time--;
            text.text = time.ToString(); ;
            StartCoroutine("StartTime");
        }

    }
}
