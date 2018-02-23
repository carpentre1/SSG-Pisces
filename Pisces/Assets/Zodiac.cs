using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zodiac : MonoBehaviour {

    //variables
    public int fishInPond;
    public int sizeOfPond;
    public int numStudentsInZodiac = 0;

    bool turnCompleted = false;

    //per-turn variables
    int fishToCatch;
    int fishToEat;
    int fishToExpand;
    int fishToInvest;

    const int FISH_FOR_SURVIVAL = 4;

    enum ZodiacType { Aries=1, Taurus=2, Gemini=3, Cancer=4, Leo=5, Virgo=6, Libra=7, Scorpio=8, Sagittarius=9, Capricorn=10, Aquarius=11, Pisces=12}
    public int turnOrder = 0;

    // Use this for initialization
    void Start() {

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TakeTurn()
    {
        while(!turnCompleted)
        {
            //wait for turn to complete
        }
        Debug.Log(this.name + " took their turn.");
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
