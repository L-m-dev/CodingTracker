
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

//await controller.CreateCodingSession(DateTime.Now, DateTime.Now.AddDays(20));
//await controller.CreateCodingSession(DateTime.Now.AddHours(3), DateTime.Now.AddDays(55));

bool codingSessionInProgress = false;
CodingSessionStopwatch codingSession = null;

//should return 9
CodingSessionED cdTest = new CodingSessionED(1, DateTime.Now.AddHours(-12), DateTime.Now.AddHours(-3));
// should return 3
CodingSessionED cdTest2 = new CodingSessionED(1, DateTime.Now.AddHours(-27), DateTime.Now.AddHours(-21));
//should return 0
CodingSessionED cdTest3 = new CodingSessionED(1, DateTime.Now.AddHours(-277), DateTime.Now.AddHours(-217));
TimeSpan ts = controller.CalculateSessionDuration(cdTest, 1);
TimeSpan ts2 = controller.CalculateSessionDuration(cdTest2, 1);
TimeSpan ts3 = controller.CalculateSessionDuration(cdTest3, 1);

List<CodingSessionED> list = new List<CodingSessionED>();
list.Add(cdTest);
list.Add(cdTest2);
list.Add(cdTest3);
//should be 12.
var resultSum = controller.GetStatisticsList(list);

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
    var mainPanel = new Panel("Hello, this is the Coding Tracker.\nYou will be able to track how much time was spent in an activity or start a new recording in real time.");
    mainPanel.Expand = true;
    AnsiConsole.Write(mainPanel);


    var menuSelection = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
        .AddChoices(menuChoices));

    //Shows all coding sessions associated with User
    if (menuSelection.Equals(menuChoices[0]))
    {
        var codingSessionList = await controller.GetAllCodingSessionById(activeUser.UserId);
        var table = new Table();
        table.AddColumn("Coding Session Id");
        table.AddColumn("Start Time");
        table.AddColumn("End Time");
        table.AddColumn("Session Duration (HH-MM-SS)");

        if (!codingSessionList.Equals(null))
        {
            foreach (var cs in codingSessionList)
            {
                table.AddRow(cs.CodingSessionId.ToString(), cs.StartTime.ToString(), cs.EndTime.ToString(), cs.SessionDuration.ToString(@"hh\:mm\:ss"));
            }

        }
        AnsiConsole.Write(table);

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
        string input = AnsiConsole.Prompt(new TextPrompt<string>("Enter date in the format YYYY-MM-DD HH:MM"));
        DateTime startTime;
        DateTime endTime;
        bool success = false;

        while (!(success = DateTime.TryParse(input, out startTime)))
        {
            AnsiConsole.WriteLine("Invalid format. Try again.");
            input = AnsiConsole.Prompt(new TextPrompt<string>("Enter date in the format YYYY-MM-DD HH:MM"));
        }
        success = false;

        input = AnsiConsole.Prompt(new TextPrompt<string>("Enter date in the format YYYY-MM-DD HH:MM"));
        while (!(success = DateTime.TryParse(input, out endTime)))
        {
            AnsiConsole.WriteLine("Invalid format. Try again.");
            input = AnsiConsole.Prompt(new TextPrompt<string>("Enter date in the format YYYY-MM-DD HH:MM"));
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
{

}
//int returnedCodSessionValue = await controller.CreateCodingSession(exStartTime, exEndTime);

//CodingSessionED getFromDb = await codingSessionDb.GetByCodingSessionIdAsync(returnedCodSessionValue);

CodingSessionED cd5Update = new CodingSessionED(20, DateTime.Now.AddHours(-20000), DateTime.Now.AddDays(-1));
//cd5Update.CodingSessionId = returnedCodSessionValue;
int returnEdition = await codingSessionDb.UpdateCodingSessionAsync(cd5Update);






IEnumerable<CodingSessionED> listCodingSession = await codingSessionDb.GetAllAsync();
//int rows = await repo.DeleteProductAsync(20);

//returnedCodSessionValue =  await codingSessionDb.AddCodingSessionAsync(cd);
Console.WriteLine("Hello, World!");
