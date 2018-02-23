using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zodiac : MonoBehaviour {

    //[HideInInspector]
    public int pondFish;
    public int numStudentsInZodiac = 0;

    enum ZodiacType { Aries=1, Taurus=2, Gemini=3, Cancer=4, Leo=5, Virgo=6, Libra=7, Scorpio=8, Sagittarius=9, Capricorn=10, Aquarius=11, Pisces=12}

    // Use this for initialization
    void Start() {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void CheckStudents()
    {

    }

    public void AddStudents(string students)
    {
        numStudentsInZodiac = int.Parse(students);
    }
}
