using UnityEngine;
using System.Collections;

public class RangeTest : AriaBehaviour
{
	void Start ()
    {
        string logMsg = "";

        foreach (int i in Range(4, 12))
        {
            logMsg += i + " ";
        }
        Debug.Log(logMsg);

        logMsg = "";
        foreach (int i in Range(5))
        {
            logMsg += i + " ";
        }
        Debug.Log(logMsg);

        logMsg = "";
        foreach (int i in Range(-5))
        {
            logMsg += i + " ";
        }
        Debug.Log(logMsg);

        logMsg = "";
        foreach (int i in Range(-5, -1))
        {
            logMsg += i + " ";
        }
        Debug.Log(logMsg);

        logMsg = "";
        foreach (int i in Range(8, 1, 2))
        {
            logMsg += i + " ";
        }
        Debug.Log(logMsg);

        logMsg = "";
        foreach (int i in Range(4, 12, 3))
        {
            logMsg += i + " ";
        }
        Debug.Log(logMsg);

        logMsg = "";
        foreach (int i in Range(4, 14, 3))
        {
            logMsg += i + " ";
        }
        Debug.Log(logMsg);
    }
}
