using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zodiac : MonoBehaviour {

    //variables
    public int fishInPond;
    public int sizeOfPond;
    public int numStudentsInZodiac = 0;

    bool turnCompleted = false;
    bool itIsMyTurn = false;

    public GameObject gameManager;
    Manager manager;

    //per-turn variables
    int fishToCatch;
    int fishToEat;
    int fishToExpand;
    int fishToInvest;

    const int FISH_FOR_SURVIVAL = 4;

    enum ZodiacType { Aries = 1, Taurus = 2, Gemini = 3, Cancer = 4, Leo = 5, Virgo = 6, Libra = 7, Scorpio = 8, Sagittarius = 9, Capricorn = 10, Aquarius = 11, Pisces = 12 }
    public int turnOrder = 0;

    // Use this for initialization
    void Start() {
        manager = gameManager.GetComponent<Manager>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TakeTurn()//called when the manager decides it's their turn
    {
        itIsMyTurn = true;
        Debug.Log("It is " + this.name + "'s turn.");
    }

    public void TurnSubmission(int eat, int invest, int expand)//when they click the submit button during their turn
    {
        if(!itIsMyTurn)//if it's not their turn, don't let them submit
        {
            Debug.Log(this.name + " tried to submit when it wasn't their turn");
            return;
        }
        fishToEat = eat; fishToInvest = invest; fishToExpand = expand;//assign the input field values to this group

        fishToCatch = fishToEat + fishToExpand + fishToInvest;//catch the sum of desired fish to eat/expand/invest
        CheckFishQuantity();
        Debug.Log("caught:" + fishToCatch);
        PrioritizeFish();
        AnnounceTurnResults();
        RecordResults();
        itIsMyTurn = false;
        manager.FindNextTurn();
    }

    private void RecordResults()
    {
        //TODO:
        //check to see if anyone dies
        //add bonus eaten fish to score
        //expand pond size
        //increase invested fish size

        manager.ReportResults(fishToEat, fishToExpand, fishToInvest, this.name);

        manager.lakeFish -= (fishToEat + fishToCatch + fishToExpand);//remove caught fish from lake

        //erase the values now that we're done
        manager.m_fishToEat = 0; manager.m_fishToExpand = 0; manager.m_fishToInvest = 0;
        fishToEat = 0; fishToExpand = 0; fishToInvest = 0;
        fishToCatch = 0;
    }

    private void AnnounceTurnResults()
    {
        Debug.Log(this.name + " ate " + fishToEat + " fish.");
        Debug.Log(this.name + " expanded using " + fishToExpand + " fish.");
        Debug.Log(this.name + " invested " + fishToInvest + " fish.");
    }

    void CheckFishQuantity()//limits how many fish they can catch
    {
        if (manager.lakeFish < fishToCatch)
        {
            fishToCatch = manager.lakeFish;
        }
        if (fishToCatch > manager.maxCatchableFish)
        {
            fishToCatch = manager.maxCatchableFish;
        }
        Debug.Log(this.name + " caught " + fishToCatch);
    }
    void PrioritizeFish()//if they tried to eat/invest/expand more than they're allowed to catch, limit the values in order of eat>expand>invest
    {
        if(fishToCatch <= fishToEat)//if they don't have enough to eat the desired amount, eat everything that they could catch
        {
            fishToEat = fishToCatch;
        }
        fishToCatch -= fishToEat;//remove the fish eaten from the amount you have
        if (fishToCatch < fishToExpand)//if they have enough to eat, but lacking after...
        {
            fishToExpand = fishToCatch;
        }
        fishToCatch -= fishToExpand;
        if (fishToCatch < fishToInvest)//if they ate and expanded, but don't have enough to invest...
        {
            fishToInvest = fishToCatch;
        }
    }

    public void CheckStudents()//before starting the game, check the amount of students applied to each zodiac group
    {
        if(numStudentsInZodiac == 0)//if no number was assigned at game start
        {
            numStudentsInZodiac = 2;//just set it to a default value of 2
        }
    }

    public void AddStudents(string students)//edited by the start screen's input fields for each zodiac group
    {
        if (students == null || students == string.Empty)
        {
            numStudentsInZodiac = 0;
            return;
        }
        numStudentsInZodiac = int.Parse(students);
    }
}
