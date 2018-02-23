using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {

    //variables
    public int numStudents = 0;
    public int lakeFish;
    public int currentTurn = 0;

    //all the elements that hook into game manager
    public GameObject Zodiacs;
    public GameObject ZodiacPanels;
    public GameObject StartButton;
    public GameObject Aries, Taurus, Gemini, Cancer, Leo, Virgo, Libra, Scorpio, Sagittarius, Capricorn, Aquarius, Pisces;

    public GameObject TurnPanel;
    public GameObject EatInputField;
    public GameObject InvestInputField;
    public GameObject ExpandInputField;
    public GameObject TurnNameText;

    //


    public void BeginGame()
    {
        numStudents = 0;//reset the number of students before starting a new game
        foreach (Transform child in Zodiacs.transform)//add up all the students in each group
        {
            child.GetComponent<Zodiac>().CheckStudents();
            numStudents += child.GetComponent<Zodiac>().numStudentsInZodiac;
        }
        Debug.Log(numStudents + " students total.");
        ToggleStartInterface(false);
        FindNextTurn();
    }

    private void ToggleStartInterface(bool active)
    {
        StartButton.SetActive(active);
        ZodiacPanels.SetActive(active);

    }

    void FindNextTurn()
    {
        while (currentTurn < 13)
        {
            currentTurn++;
            foreach (Transform child in Zodiacs.transform)
            {
                if (child.GetComponent<Zodiac>().turnOrder == currentTurn)
                {
                    child.GetComponent<Zodiac>().TakeTurn();
                }
            }
        }
        Debug.Log("all turns taken");
        //find gameobject whose zodiac = turn number, use their TakeTurn(), then check if FinalTurnComplete()
    }

    void FinalTurnComplete()
    {
        //calculate new number of fish in lake, new number of fish in ponds
    }
}
