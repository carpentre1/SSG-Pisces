using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour {

    #region variables
    public float numStudents = 0;
    public int numTeams = 12;
    public int lakeFish = 350;
    public int currentTurn = 0;
    public int maxCatchableFish;
    public int currentYear = 1;
    public int lakeCapacity;//(players*60) / (8/1) / .3/2
    public string firstToDie = "";

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
    public int m_fishToEatFromPond = 0;
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
    public GameObject PondInputField;
    public GameObject TurnNameText;
    public GameObject TurnResultsText;
    public GameObject YearText;
    public GameObject YearInfoPanel;
    public GameObject YearInfoText;
    public GameObject TurnInfoPanel;
    public GameObject TurnInfoText;
    public GameObject EndingText;
    public GameObject DeathText;
    #endregion


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
        if(numTeams <= 1)//if there's only one team left
        {
            foreach (Transform child in Zodiacs.transform)
            {
                if(!child.GetComponent<Zodiac>().hasLost)
                {
                    Debug.Log(child.name + " is the only team remaining! The game has ended.");
                    TurnPanel.SetActive(false);
                    EndingText.SetActive(true);
                    EndingText.GetComponent<Text>().text = child.name + " is the only team remaining! The game has ended.\n" + firstToDie + " was the first team to die.\n" +
                        "How did the tragedy of the commons affect this game? Did any groups opt not to take as many fish as possible each turn? Why?\n" +
                        "If you were to design a game with limited resources, what would you do to account for the instinctually selfish nature of each player?";
                    return;
                }
            }
        }
        if(currentTurn >= 12)//if all turns were taken, the year has ended
        {
            RegrowPondFish();
            RegrowLakeFish();

            currentTurn = 0;
            currentYear++;
            YearText.GetComponent<Text>().text = "Year " + currentYear;
        }

        //go to the next person's turn
        currentTurn++;
        EatInputField.GetComponent<InputField>().Select();//focus the "eat" field each turn;

        if(currentYear==1)//can't use the pond during the first year
        {
            InvestInputField.SetActive(false);
            ExpandInputField.SetActive(false);
        }
        else//after the first year, let them use the pond
        {
            InvestInputField.SetActive(true);
            ExpandInputField.SetActive(true);
        }

        foreach (Transform child in Zodiacs.transform)
        {
            if (child.GetComponent<Zodiac>().turnOrder == currentTurn)
            {
                child.GetComponent<Zodiac>().TakeTurn();
                if(child.GetComponent<Zodiac>().hasLost) { break; }//don't unintentionally display info for a dead team
                TurnNameText.GetComponent<Text>().text = child.name;

                //update the turn info panel on the right hand side
                int fishNeeded = child.GetComponent<Zodiac>().numStudentsInZodiac * 4;
                TurnInfoText.GetComponent<Text>().text = "You must eat " + fishNeeded + " fish to survive.\nYou can catch " + maxCatchableFish + " fish.\n\nMax size of pond: " +
                    child.GetComponent<Zodiac>().sizeOfPond + "\nFish in pond: " + child.GetComponent<Zodiac>().fishInPond;
            }
        }

    }

    void RegrowPondFish()//regrows everyone's pond fish at the end of the year
    {
        foreach(Transform child in Zodiacs.transform)
        {
            int pondFish = child.GetComponent<Zodiac>().fishInPond;//use a more concise variable for readability
            pondFish = (int)(pondFish * 1.3);//pond fish regrow by 30% of their current amount
            child.GetComponent<Zodiac>().fishInPond = Math.Min(pondFish, child.GetComponent<Zodiac>().sizeOfPond);
        }
    }

    void RegrowLakeFish()//regrows the fish in the lake at the end of the year
    {
        float growthFish = GROWTH_RATE * lakeFish;//the amount the fish would increase at 50% capacity
        float fishRatio = (float)lakeFish / (float)lakeCapacity;//the capacity ratio that influences how much the fish grow
        int extraFish = Convert.ToInt32(growthFish - growthFish * fishRatio);//the actual amount of fish that will grow
        lakeFish = lakeFish + extraFish;//the final value for how much fish we'll have at the end of the year due to growth

        YearInfoPanel.SetActive(true);
        YearInfoText.GetComponent<Text>().text = lakeFishCaughtThisYear + " fish were caught last year.\n" + extraFish +
            " fish grew up at the end of the year.\n" + lakeFish + " lake fish remained at the end of the year.";
        lakeFishCaughtThisYear = 0;
    }

    public void SubmitButtonClicked()//when the current team clicks the button to submit how many fish they will catch
    {

        foreach (Transform child in Zodiacs.transform)
        {
            if (child.GetComponent<Zodiac>().turnOrder == currentTurn)
            {
                child.GetComponent<Zodiac>().TurnSubmission(m_fishToEat, m_fishToInvest, m_fishToExpand, m_fishToEatFromPond);
                EatInputField.GetComponent<InputField>().text = "";//clearing input fields
                InvestInputField.GetComponent<InputField>().text = "";
                ExpandInputField.GetComponent<InputField>().text = "";
                PondInputField.GetComponent<InputField>().text = "";

                TurnResultsText.GetComponent<Text>().text = z_name + " ate " + z_eat + " fish,\nexpanded using " + z_expand + " fish,\nand invested " + z_invest + " fish.";


                break;
            }
        }
    }

    public void ReportResults(int eat, int expand, int invest, int pondEat, string name)//zodiacs call this function to give information about what they're doing with their fish
    {
        z_name = name; z_eat = eat + pondEat; z_expand = expand; z_invest = invest;
        lakeFishCaughtThisYear += (eat + expand + invest);//record all the fish caught so you can report it at end of year
    }

    public void ReportDeath(int studentsDied, Zodiac Team)
    {
        DeathText.SetActive(true);
        if(studentsDied == 1)
        {
            DeathText.GetComponent<Text>().text = studentsDied + " " + Team.name + " student has died!";
        }
        else
        {
            DeathText.GetComponent<Text>().text = studentsDied + " " + Team.name + " students have died!";
        }
        if(Team.hasLost)
        {
            DeathText.GetComponent<Text>().text += " " + Team.name + " has lost!";
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
    public void LogPond(string fish)
    {
        if (fish == null || fish == string.Empty)
        {
            m_fishToEatFromPond = 0;
            return;
        }
        m_fishToEatFromPond = int.Parse(fish);
    }

    void FinalTurnComplete()
    {
        //calculate new number of fish in lake, new number of fish in ponds
    }
}
