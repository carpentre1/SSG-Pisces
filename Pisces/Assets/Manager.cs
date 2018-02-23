using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {

    //[HideInInspector]
    public int numStudents = 0;
    public int lakeFish;

    //public Zodiac zodiacscript;

    public GameObject Zodiacs;
    public GameObject ZodiacPanels;
    public GameObject Aries, Taurus, Gemini, Cancer, Leo, Virgo, Libra, Scorpio, Sagittarius, Capricorn, Aquarius, Pisces;

    void BeginGame()
    {
        foreach (Transform child in Zodiacs.transform)
        {
            //child.GetComponent<Zodiac>().CheckStudents();
        }
    }

    void FindNextTurn()
    {
        //find gameobject whose zodiac = turn number, use their TakeTurn(), then check if FinalTurnComplete()
    }

    void FinalTurnComplete()
    {
        //calculate new number of fish in lake, new number of fish in ponds
    }
}
