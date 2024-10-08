﻿
using CodingTrackerApplication;
using System.Configuration;
using System.Collections.Specialized;
using DatabaseLayer;
using System.Diagnostics;
using Spectre.Console;

//load database configuration (address, login, password, table)
DatabaseConfiguration dbConfig = new();

//pass dBConfig object into each database repository class
CodingSessionRepository codingSessionDb = new DatabaseLayer.CodingSessionRepository(dbConfig);
ApplicationUserRepository applicationUserDb = new ApplicationUserRepository(dbConfig);

//Checks if there is an existing user. An ApplicationUser can have multiple CodingSession assigned to them.
//Default user is 1.

ApplicationUserED activeUser = await applicationUserDb.GetByUserId(1);

//if there's no existing user, create an ApplicationUserED and insert into database.
if (activeUser == null)
{
    ApplicationUserED user = new ApplicationUserED(1, "DefaultUser");
    await applicationUserDb.AddApplicationUser(user);

    activeUser = await applicationUserDb.GetByUserId(1);
}
Controller controller = new(activeUser.UserId, codingSessionDb);

bool codingSessionInProgress = false;
CodingSessionStopwatch codingSession = null;

while (true)
{
    AnsiConsole.Clear();
    string[] continueOrExitMenuChoices = new string[2];
    continueOrExitMenuChoices[0] = "Continue";
    continueOrExitMenuChoices[1] = "Exit Application";


    string[] menuChoices = new string[10];
    menuChoices[9] = "Exit";
    menuChoices[0] = "View all Coding Sessions and statistics.";
    menuChoices[1] = "Add a Coding Session you've completed, with a Start Time and an End Time";
    if (codingSessionInProgress)
    {
        menuChoices[2] = "End Current Coding Session";
    }
    else
    {
        menuChoices[2] = "Start a new Live Coding Session";
    }

    menuChoices = menuChoices.Where(choice => !string.IsNullOrEmpty(choice)).ToArray();

    // HUD START
    var mainPanel = new Panel("Coding Tracker.\nYou will be able to track how much time was spent coding or start a new session tracker in real time.");
    mainPanel.Expand = true;
    AnsiConsole.Write(mainPanel);
    AnsiConsole.WriteLine();

    var menuSelection = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
        .AddChoices(menuChoices));

    //Shows all coding sessions associated with User
    if (menuSelection.Equals(menuChoices[0]))
    {
        var codingSessionList = await controller.GetAllCodingSessionById(activeUser.UserId);
        Dictionary<string, TimeSpan> statistics = controller.GetStatisticsList(codingSessionList);
        if (codingSessionList.Count() > 0)
        {
            var table = new Table();
            table.AddColumn("Coding Session Id");
            table.AddColumn("Start Time");
            table.AddColumn("End Time");
            table.AddColumn("Session Duration (DD-HH-MM-SS)");

            if (!codingSessionList.Equals(null))
            {
                foreach (var cs in codingSessionList)
                {
                    table.AddRow(cs.CodingSessionId.ToString(), cs.StartTime.ToString(), cs.EndTime.ToString(), cs.SessionDuration.ToString(@"dd\:hh\:mm\:ss"));
                }

            }
            AnsiConsole.Write(table);

            var statisticsPanel = new Panel($"Last 24 hours: {statistics.GetValueOrDefault("1DayTotal")}" +
                $"\nLast week: {controller.FormatTimeSpan(statistics.GetValueOrDefault("7DayTotal"))}    Average per day: {controller.FormatTimeSpan(statistics.GetValueOrDefault("7DayAverage"))}" +
                $"\nLast month: {controller.FormatTimeSpan(statistics.GetValueOrDefault("30DayTotal"))}    Average per day: {controller.FormatTimeSpan(statistics.GetValueOrDefault("30DayAverage"))}" +
                $"\nLast year: {controller.FormatTimeSpan(statistics.GetValueOrDefault("365DayTotal"))}    Average per day: {controller.FormatTimeSpan(statistics.GetValueOrDefault("365DayAverage"))}");
            statisticsPanel.Expand = true;
            AnsiConsole.Write(statisticsPanel);
        }
        else
        {
            AnsiConsole.WriteLine("Found no coding sessions. Start a new one!");
            AnsiConsole.WriteLine();
        }
        if (Continue(continueOrExitMenuChoices))
        {
            continue;
        }
        else
        {
            break;
        }

    }
    else if (menuSelection.Equals(menuChoices[1]))
    {
        string input = AnsiConsole.Prompt(new TextPrompt<string>("Enter Start Time in the format YYYY-MM-DD HH:MM."));
        DateTime startTime;
        DateTime endTime;
        bool success = false;

        while (!(success = DateTime.TryParse(input, out startTime)))
        {
            AnsiConsole.WriteLine("Invalid format. Try again.");
            input = AnsiConsole.Prompt(new TextPrompt<string>("Enter Start Time in the format YYYY-MM-DD HH:MM."));
        }
        success = false;

        input = AnsiConsole.Prompt(new TextPrompt<string>("Enter End Time in the format YYYY-MM-DD HH:MM."));
        while (!(success = DateTime.TryParse(input, out endTime)))
        {
            AnsiConsole.WriteLine("Invalid format. Try again.");
            input = AnsiConsole.Prompt(new TextPrompt<string>("Enter End Time in the format YYYY-MM-DD HH:MM."));
        }
        try
        {
            await controller.CreateCodingSession(startTime, endTime);
            AnsiConsole.WriteLine("Success! Session was added!");
        }
        catch (Exception e)
        {
            AnsiConsole.WriteLine("Error, transaction failed.");
            AnsiConsole.WriteLine(e.Message);
        }
        AnsiConsole.WriteLine();
        AnsiConsole.WriteLine();
        if (Continue(continueOrExitMenuChoices))
        {
            continue;
        }
        else
        {
            break;
        }

    }
    else if (!codingSessionInProgress && menuSelection.Equals(menuChoices[2]))
    {
        codingSession = new CodingSessionStopwatch();
        codingSession.StartCodingSessionStopWatch();
        codingSessionInProgress = true;
        AnsiConsole.WriteLine("Started new coding session.");
        AnsiConsole.WriteLine(DateTime.Now.ToString());
        if (Continue(continueOrExitMenuChoices))
        {
            continue;
        }
        else
        {
            break;
        }
    }
    else if (codingSessionInProgress && menuSelection.Equals(menuChoices[2]))
    {
        if (codingSession is not null)
        {
            codingSession.StopCodingSessionStopWatch();
            codingSessionInProgress = false;
            try
            {
                await controller.CreateCodingSession(codingSession.StartTime, codingSession.EndTime);
                AnsiConsole.WriteLine($"Session that started at {codingSession.StartTime} ended at {codingSession.EndTime}");
                AnsiConsole.WriteLine($"Duration: {codingSession.SessionDuration}");
                AnsiConsole.WriteLine("Success! Session was added!");
            }
            catch (Exception e)
            {
                AnsiConsole.WriteLine("Error, transaction failed.");
                AnsiConsole.WriteLine(e.Message);
            }
            if (Continue(continueOrExitMenuChoices))
            {
                continue;
            }
            else
            {
                break;
            }
        }
        else
        {
            AnsiConsole.WriteLine("No coding session in progress");
        }
    }
    else if (menuSelection.Equals(menuChoices[3]))
    {
        break;
    }
}

bool Continue(string[] choiceArray)
{
    var exitSelection = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
        .AddChoices(choiceArray));
    if (exitSelection.Equals(choiceArray[0]))
    {
        return true;
    }
    else
    {
        return false;
    }
}
  