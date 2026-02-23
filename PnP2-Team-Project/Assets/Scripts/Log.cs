using System.Collections.Generic;
using UnityEngine;

public class Log : MonoBehaviour
{
    [SerializeField] GameObject logEntry;
    Stack<GameObject> logStack;

   
    void Start()
    {
        display();
    }

    public void addFish()
    {
        GameObject temp = Instantiate(logEntry);
        logStack.Push(temp);
    }

    void display()
    {
        for (int i = 0; i < logStack.Count; i++)
        {
            
        }
    }

}
