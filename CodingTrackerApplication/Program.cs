// See https://aka.ms/new-console-template for more information
using CodingTrackerApplication;
using System.Configuration;
using System.Collections.Specialized;
using DatabaseLayer;

DatabaseConfiguration dbConfig = new();
CodingSessionRepository codingSessionDb =  new DatabaseLayer.CodingSessionRepository(dbConfig);

//for now we are using id 20.
Controller controller = new(20, codingSessionDb);

// Create TEST
 DateTime exStartTime = DateTime.Now;
 DateTime exEndTime = DateTime.Now.AddDays(2);
int returnedCodSessionValue = await controller.CreateCodingSession(exStartTime,exEndTime);
CodingSessionED getFromDb = await codingSessionDb.GetByCodingSessionIdAsync(returnedCodSessionValue);

CodingSessionED cd5Update = new CodingSessionED(20, DateTime.Now.AddHours(-20000), DateTime.Now.AddDays(-1));
cd5Update.CodingSessionId = returnedCodSessionValue;
int returnEdition = await codingSessionDb.UpdateCodingSessionAsync(cd5Update);






IEnumerable<CodingSessionED> listCodingSession = await codingSessionDb.GetAllAsync();
 //int rows = await repo.DeleteProductAsync(20);
 
  //returnedCodSessionValue =  await codingSessionDb.AddCodingSessionAsync(cd);
Console.WriteLine("Hello, World!");
