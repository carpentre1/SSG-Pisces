using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour {

    #region variables
    public float numStudents = 0;
    public int lakeFish = 350;
    public int currentTurn = 0;
    public int maxCatchableFish;
    public int currentYear = 1;
    public int lakeCapacity;//(players*60) / (8/1) / .3/2
    //12p = 720 / 8 / .15

    public const float GROWTH_RATE = .3f;

    int lakeFishCaughtThisYear;


    //variables reported from each zodiac at turn end
    int z_eat;
    int z_expand;
    int z_invest;
    string z_name;

    //variables that are logged during each turn
    public int m_fishToEat = 0;
    public int m_fishToExpand = 0;
    public int m_fishToInvest = 0;
    #endregion


    #region game objects
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
    public GameObject TurnResultsText;
    public GameObject TurnCatchLimitText;
    public GameObject YearText;
    public GameObject YearInfoPanel;
    public GameObject YearInfoText;
    public GameObject TurnInfoPanel;
    public GameObject TurnInfoText;
    #endregion

    //TODO:
    //check for deaths. kill x players based on uneaten fish. skip teams that have 0 alive players. keep a list of dead teams near bottom of screen. end game when all dead.
    //create and hook up pond mechanics
    //add explanation of game in start screen
    //add images and pretty up the UI
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

        lakeCapacity = Convert.ToInt32((numStudents * 60) / 8 / .15);
        maxCatchableFish = (int)numStudents;
        lakeFish = Convert.ToInt32(lakeCapacity * .6);

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
        if(currentTurn >= 12)//if all turns were taken, the year has ended
        {

            float growthFish = GROWTH_RATE * lakeFish;//the amount the fish would increase at 50% capacity
            float fishRatio = (float)lakeFish / (float)lakeCapacity;//the capacity ratio that influences how much the fish grow
            int extraFish = Convert.ToInt32(growthFish - growthFish * fishRatio);//the actual amount of fish that will grow
            lakeFish = lakeFish + extraFish;//the final value for how much fish we'll have at the end of the year due to growth

            currentTurn = 0;
            currentYear++;
            YearText.GetComponent<Text>().text = "Year " + currentYear;
            YearInfoPanel.SetActive(true);
            YearInfoText.GetComponent<Text>().text = lakeFishCaughtThisYear + " fish were caught last year.\n" + extraFish + 
                " fish grew up at the end of the year.\n" + lakeFish + " lake fish remained at the end of the year.";
            lakeFishCaughtThisYear = 0;
        }
        if (currentTurn < 13)//go to the next person's turn
        {
            TurnCatchLimitText.GetComponent<Text>().text = "You can catch " + maxCatchableFish + " fish.";
            currentTurn++;
            EatInputField.GetComponent<InputField>().Select();//focus the "eat" field each turn;
            foreach (Transform child in Zodiacs.transform)
            {
                if (child.GetComponent<Zodiac>().turnOrder == currentTurn)
                {
                    child.GetComponent<Zodiac>().TakeTurn();
                    TurnNameText.GetComponent<Text>().text = child.name;

                    //update the turn info panel on the right hand side
                    int fishNeeded = child.GetComponent<Zodiac>().numStudentsInZodiac * 4;
                    TurnInfoText.GetComponent<Text>().text = "You must eat " + fishNeeded + " fish to survive.\nYou can catch " + maxCatchableFish + " fish.\n\nMax size of pond: " +
                        child.GetComponent<Zodiac>().sizeOfPond + "\nFish in pond: " + child.GetComponent<Zodiac>().fishInPond;
                }
            }
        }
    }

    public void SubmitButtonClicked()//when the current team clicks the button to submit how many fish they will catch
    {

        foreach (Transform child in Zodiacs.transform)
        {
            if (child.GetComponent<Zodiac>().turnOrder == currentTurn)
            {
                child.GetComponent<Zodiac>().TurnSubmission(m_fishToEat, m_fishToInvest, m_fishToExpand);
                EatInputField.GetComponent<InputField>().text = "";//clearing input fields
                InvestInputField.GetComponent<InputField>().text = "";
                ExpandInputField.GetComponent<InputField>().text = "";

                TurnResultsText.GetComponent<Text>().text = z_name + " ate " + z_eat + " fish,\nexpanded using " + z_expand + " fish,\nand invested " + z_invest + " fish.";


                break;
            }
        }
    }

    public void ReportResults(int eat, int expand, int invest, string name)//zodiacs call this function to give information about what they're doing with their fish
    {
        z_name = name; z_eat = eat; z_expand = expand; z_invest = invest;
        lakeFishCaughtThisYear += (eat + expand + invest);//record all the fish caught so you can report it at end of year
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
