using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Regions : MonoBehaviour
{
    private const int size = 12;
    private string[] regionsName = new string[size];
    private string[] regionsShortName = new string[size];
    public Regions()
    {
        regionsName[0] = "Asia";          regionsShortName[0] = "asia";
        regionsName[1] = "Australia";     regionsShortName[1] = "au";
        regionsName[2] = "Canada";        regionsShortName[2] = "cae";
        regionsName[3] = "Europe";        regionsShortName[3] = "eu";
        regionsName[4] = "India";         regionsShortName[4] = "in";
        regionsName[5] = "Japan";         regionsShortName[5] = "jp";
        regionsName[6] = "Russia";        regionsShortName[6] = "ru";
        regionsName[7] = "Russia, East";  regionsShortName[7] = "rue";
        regionsName[8] = "South America"; regionsShortName[8] = "sa";
        regionsName[9] = "South Korea";   regionsShortName[9] = "kr";
        regionsName[10] = "USA, East";    regionsShortName[10] = "us";
        regionsName[11] = "USA, West";    regionsShortName[11] = "usw";
    }

    public string GetRegionShortName(string name)
    {
        for (int i = 0; i < size; i++)
            if (regionsName[i] == name)
                return regionsShortName[i];
        return "";
    }

    public string GetRegionName(string name)
    {
        int endString = name.Length;
        for (int i = 0; i != name.Length; i++)
            if (name[i] == '/')
                endString = i;
        for (int i = 0; i<size; i++)
            if (regionsShortName[i] == name.Substring(0, endString))
                return regionsName[i];
        return "";
    }
}
