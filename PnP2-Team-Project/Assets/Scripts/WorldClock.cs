using Unity.Burst;
using UnityEngine;

public class WorldClock : MonoBehaviour
{
    public static WorldClock instance;
    public enum Month
    {
        January,
        Febuary,
        March,
        April,
        May,
        June,
        July,
        August,
        September,
        October,
        November,
        December
    }
    
    [SerializeField] int startingYear = 2026;
    [SerializeField] Month startingMonth = Month.January;
    [SerializeField] int startingDay = 1;
    [SerializeField] int startingHour = 0;
    [SerializeField] int startingMinute = 0;
    [SerializeField] float startingSecond = 0f;

    [SerializeField] float secondsPerRealSecond = 60f;
    
    [SerializeField] bool countMonths = true;

    [SerializeField] Transform minuteHand;
    [SerializeField] Transform hourHand;

    int year;
    Month month;
    int maxDay;
    int day;
    int hour;
    int minute;
    float second;


[SerializeField] int minuteSpeed;


    

    void Awake()
    {
        instance = this;

        year = startingYear;
        month = startingMonth;
        SetMaxDay();
        day = Mathf.Clamp(startingDay, 1, maxDay);
        hour = Mathf.Clamp(startingHour, 0, 23);
        minute = Mathf.Clamp(startingMinute, 0, 59);
        second = Mathf.Clamp(startingSecond, 0f, 59.99f);
    }
    void Update()
    {
        float dt = Time.deltaTime;
        // pause clock when game is paused
        if (dt <= 0f)
        {
            return;
        }

        second += dt * secondsPerRealSecond;

        TimeChange();

        UpdateClockHands();
    }
    void UpdateClockHands()
    {
        if (minuteHand != null)
        {
            float minuteProgress = minute + (second / 60f);
            float minuteAngle = minuteProgress * 6f;
            minuteHand.localRotation = Quaternion.Euler(0f, minuteAngle, 0f);
        }
        if (hourHand != null)
        {
            // 360 degrees per 12 hours => 30 degrees per hour
            // include minutes for smooth movement
            float hour12 = (hour % 12) + ((minute + (second / 60f)) / 60f);
            float hourAngle = hour12 * 30f;
            hourHand.localRotation = Quaternion.Euler(0f, hourAngle, 0f);
        }
    }
    void SetMaxDay()
    {
        switch (month)
        {
            case Month.January:
                maxDay = 31;
                break;
            case Month.Febuary:
                maxDay = 28;
                break;
            case Month.March:
                maxDay = 31;
                break;
            case Month.April:
                maxDay = 30;
                break;
            case Month.May:
                maxDay = 31;
                break;
            case Month.June:
                maxDay = 30;
                break;
            case Month.July:
                maxDay = 31;
                break;
            case Month.August:
                maxDay = 31;
                break;
            case Month.September:
                maxDay = 30;
                break;
            case Month.October:
                maxDay = 31;
                break;
            case Month.November:
                maxDay = 30;
                break;
            case Month.December:
                maxDay = 31;
                break;
            default:
                maxDay= 31;
                break;
        }
    }
    void TimeChange()
    {
        if (second >= 60)
        {
            second = 0;
            minute++;
        }
        if (minute >= 60)
        {
            minute = 0;
            hour++;
        }
        if (hour > 24)
        {
            hour = 0;
            day++;
        }
        if (countMonths)
        {
            if (day > maxDay)
            {
                day = 0;
                month++;
                SetMaxDay();
            }

            if (month > Month.December)
            {
                month = Month.January;
                year++;
            }
        }

    }
}
