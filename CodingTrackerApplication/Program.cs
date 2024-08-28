// See https://aka.ms/new-console-template for more information
using CodingTrackerApplication;
using System.Configuration;
using System.Collections.Specialized;
using DatabaseLayer;

DatabaseConfiguration dbConfig = new();

 DatabaseLayer.CodingSessionRepository codingSessionDb =  new DatabaseLayer.CodingSessionRepository(dbConfig);

CodingSession cd = new CodingSession(20, DateTime.Now, DateTime.Now.AddMinutes(100));
CodingSession cd2 = new CodingSession(20, DateTime.Now, DateTime.Now.AddMinutes(1444));
CodingSession cd3 = new CodingSession(20, DateTime.Now, DateTime.Now.AddMinutes(100442));
CodingSession cd4 = new CodingSession(20, DateTime.Now, DateTime.Now.AddMinutes(1024240));


int returnedCodSessionValue = await codingSessionDb.AddCodingSessionAsync(cd4);

CodingSession getFromDb = await codingSessionDb.GetByCodingSessionIdAsync(returnedCodSessionValue);

CodingSession cd5Update = new CodingSession(20, DateTime.Now.AddHours(-20000), DateTime.Now.AddDays(-1));
cd5Update.CodingSessionId = returnedCodSessionValue;
int returnEdition = await codingSessionDb.UpdateCodingSessionAsync(cd5Update);






IEnumerable<CodingSession> listCodingSession = await codingSessionDb.GetAllAsync();
 //int rows = await repo.DeleteProductAsync(20);
 
  returnedCodSessionValue =  await codingSessionDb.AddCodingSessionAsync(cd);
Console.WriteLine("Hello, World!");
