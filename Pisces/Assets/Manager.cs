using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour {

    //variables
    public float numStudents = 0;
    public int lakeFish = 350;
    public int currentTurn = 0;
    public int maxCatchableFish;

    //variables that are logged during each turn
    public int m_fishToEat = 0;
    public int m_fishToExpand = 0;
    public int m_fishToInvest = 0;

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

        //divide students by 1.6(?) and round down to find how many fish each group can catch
        maxCatchableFish = (int)Mathf.Floor(numStudents*.6f);//change this magic number to the real formula's value later

        ToggleTurnInterface(true);
        FindNextTurn();
    }

    private void ToggleStartInterface(bool active)
    {
        StartButton.SetActive(active);
        ZodiacPanels.SetActive(active);

    }

    private void ToggleTurnInterface(bool active)
    {
        TurnPanel.SetActive(active);

    }

    public void FindNextTurn()
    {
        Debug.Log(currentTurn);
        if(currentTurn >= 12)
        {
            Debug.Log("all turns taken, starting next round");
            currentTurn = 0;
        }
        if (currentTurn < 13)
        {
            currentTurn++;
            foreach (Transform child in Zodiacs.transform)
            {
                if (child.GetComponent<Zodiac>().turnOrder == currentTurn)
                {
                    child.GetComponent<Zodiac>().TakeTurn();
                    TurnNameText.GetComponent<Text>().text = child.name;
                }
            }
        }
    }

    public void SubmitButtonClicked()
    {

        foreach (Transform child in Zodiacs.transform)
        {
            if (child.GetComponent<Zodiac>().turnOrder == currentTurn)
            {
                child.GetComponent<Zodiac>().TurnSubmission(m_fishToEat, m_fishToInvest, m_fishToExpand);
                EatInputField.GetComponent<InputField>().text = "";//clearing input fields
                InvestInputField.GetComponent<InputField>().text = "";
                ExpandInputField.GetComponent<InputField>().text = "";
                break;
            }
        }
    }

    public void LogEat(string fish)//reads the input field during turns so that the proper zodiac group can retrieve the variable
    {
        if (fish == null || fish == string.Empty)
        {
            m_fishToEat = 0;
            return;
        }
        m_fishToEat = int.Parse(fish);
    }

    public void LogExpand(string fish)
    {
        if (fish == null || fish == string.Empty)
        {
            m_fishToExpand = 0;
            return;
        }
        m_fishToExpand = int.Parse(fish);
    }

    public void LogInvest(string fish)
    {
        if (fish == null || fish == string.Empty)
        {
            m_fishToInvest = 0;
            return;
        }
        m_fishToInvest = int.Parse(fish);
    }

    void FinalTurnComplete()
    {
        //calculate new number of fish in lake, new number of fish in ponds
    }
}
