using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    //class = type = blueprint
    //instance = object = what we created from blueprint

    //Singleton;
    //      1. Only a Single Instance of the class can exist
    //      2. We want to provide easy global access to the instance

    public static GameOver Instance;
    //static-one shared copy of all instances of the class

    //references to our win/lose text game objects

    public GameObject winText;
    public GameObject loseText;

    public bool loseWhenTimesUp = true;
    public float transitionDelay = 5f;

    public UnityEvent OnWin;
    public UnityEvent OnLose;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    public void Win()
    {
        //TODO
        print("We Win");

        //notify anybody listening that we died
        if (OnWin != null)
        {
            OnWin.Invoke();
        }

        //"We win" Message Appears
        winText.SetActive(true);

        //Stop Timer.  Question mark is a null reference check if something, like the timer, exists
        TimeLimit.Instance?.StopTimer();

        //start our coroutine
        StartCoroutine(ReturnToMenuCoroutine());

        //record that we beat this level in LevelManager
        string sceneName = SceneManager.GetActiveScene().name;

        if (!LevelManager.levelsBeaten.Contains(sceneName))
            LevelManager.levelsBeaten.Add(sceneName);
    }

    public void Lose()
    {
        //TODO
        print("Game Over");

        //notify anybody listening that we died
        if (OnLose != null)
        {
            OnLose.Invoke();
        }

        //"Game Over" message appears
        loseText.SetActive(true);

        //Stop Timer.  Question mark is a null reference check if something, like the timer, exists
        TimeLimit.Instance?.StopTimer();

        //start our coroutine
        StartCoroutine(ReturnToMenuCoroutine());
    }

    IEnumerator ReturnToMenuCoroutine()
    {
        //Wait 5 seconds
        yield return new WaitForSeconds(transitionDelay);



        //Main Menu appears

        SceneManager.LoadScene("Title Scene");

    }

    public void TimesUp()
    {

        //check if player loses when time expires (DEFENDER)

        if (loseWhenTimesUp == true)
        {
            Lose();
        }
        else //otherwise, determine winner from scoring (COMBAT)
        {
            bool won = PointManager.Instance.DidPlayerWin();
            if (won)
            {
                Win();
            }
            else
            {
                Lose();
            }
        }

        //TODO: if wins, store results for menu

    }
}
