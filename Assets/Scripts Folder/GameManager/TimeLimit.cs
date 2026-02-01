using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TimeLimit : MonoBehaviour
{
    //Type coloring for variables
    //blue = 'value' type
    //teal = 'reference' type
    //
    //only 'reference' types can be null

    public static TimeLimit Instance;
    //static-one shared copy of all instances of the class

    public int gameTimer = 300;

    //Text numbers in Timer
    public Text timerText;

    public GameObject player;

    public GameObject TimesUpText;

    public bool notifyGameManager = true;

    public UnityEvent OnTimesUp;

    // Start is called before the first frame update
    void Start()
    {

        Instance = this;

        //assign the countdown in our text UI
        timerText.text = gameTimer.ToString();

        //start our coroutine
        StartCoroutine(TimeLimitCoroutine());

    }

   IEnumerator TimeLimitCoroutine()
    {
        //Countdown Hits '0' We Lose

        while (gameTimer > 0)
        {
            //wait 1 second
            yield return new WaitForSeconds(1f);

            //lower coundown by 1
            gameTimer = gameTimer - 1;

            //update our text UI
            timerText.text = gameTimer.ToString();
        }


        
      
        //activate TimesUp UI
        if(TimesUpText) TimesUpText.SetActive(true);

        //tell GameManager the time is up
        if(notifyGameManager)GameOver.Instance.TimesUp();

        if (OnTimesUp != null)
        {
            OnTimesUp.Invoke();
        }
    }

    public void StopTimer()
    {
        StopAllCoroutines();
    }

    private void OnDisable()
    {
        StopTimer();
    }

    public void AddTime(int seconds)
    {
        gameTimer = gameTimer + 5;

        //update our text UI
        timerText.text = gameTimer.ToString();
    }
}
