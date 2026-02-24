using System;
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
    double totalGameMinutes;

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
    public event System.Action OnMinuteChanged;
    public double TotalMinutes => totalGameMinutes;




    void Awake()
    {
        instance = this;

        year = startingYear;
        month = startingMonth;

        SetMaxDay();

        day = startingDay;
        hour = startingHour;
        minute = 0;
        second = 0f;

        totalGameMinutes = ConvertDateToTotalMinutes();
    }
    void Update()
    {
        float dt = Time.deltaTime;
        if (dt <= 0f)
            return;

        second += dt * secondsPerRealSecond;

        while (second >= 60f)
        {
            second -= 60f;
            AddGameMinutes(1);
        }

        UpdateClockHands();
    }
    double ConvertDateToTotalMinutes()
    {
        int totalDays = (startingDay - 1);

        for (int i = 0; i < (int)startingMonth; i++)
        {
            totalDays += GetDaysInMonth((Month)i);
        }

        double minutes =
            (totalDays * 1440.0) +
            (startingHour * 60.0) +
            minute;

        return minutes;
    }
    public void AddGameMinutes(double minutesToAdd)
    {
        if (minutesToAdd <= 0)
            return;

        int oldWholeMinute = (int)System.Math.Floor(totalGameMinutes);

        totalGameMinutes += minutesToAdd;

        int newWholeMinute = (int)System.Math.Floor(totalGameMinutes);

        for (int m = oldWholeMinute + 1; m <= newWholeMinute; m++)
        {
            OnMinuteChanged?.Invoke();
        }

        RecalculateDateTimeFromTotal();
    }
    void RecalculateDateTimeFromTotal()
    {
        double remainingMinutes = totalGameMinutes;

        int totalDays = (int)(remainingMinutes / 1440.0);
        remainingMinutes %= 1440.0;

        hour = (int)(remainingMinutes / 60.0);
        minute = (int)(remainingMinutes % 60.0);

        int workingDayCount = totalDays;

        month = Month.January;

        while (true)
        {
            int daysInMonth = GetDaysInMonth(month);

            if (workingDayCount < daysInMonth)
            {
                day = workingDayCount + 1;
                break;
            }

            workingDayCount -= daysInMonth;

            if (month == Month.December)
            {
                month = Month.January;
                year++;
            }
            else
            {
                month++;
            }
        }
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
    int GetDaysInMonth(Month m)
    {
        switch (m)
        {
            case Month.January:
                return 31;
            case Month.Febuary:
                return 28;
            case Month.March:
                return 31;
            case Month.April:
                return 30;
            case Month.May:
                return 31;
            case Month.June:
                return 30;
            case Month.July:
                return 31;
            case Month.August:
                return 31;
            case Month.September:
                return 30;
            case Month.October:
                return 31;
            case Month.November:
                return 30;
            case Month.December:
                return 31;
            default:
                return 31;
        }
    }
    /// <summary>
    /// since I changed to a running minute count and then converting
    /// that into the different date info, this method is no longer used
    /// </summary>
    void TimeChange()
    {
        if (second >= 60f)
        {
            second -= 60f;
            minute++;
        }
        if (minute >= 60)
        {
            minute -= 60;
            hour++;
        }
        if (hour >= 24)
        {
            hour -=24;
            day++;
        }
        if (countMonths)
        {
            if (day > maxDay)
            {
                day = 1;
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
    public double GetTotalMinutes()
    {
        return totalGameMinutes;
    }

}
