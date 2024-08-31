# CodingTracker  

Application for logging time spent in an activity.
The user adds their time spent 'coding' - or any other task - and the application records the date and duration to a database.    
Users can see the history and statistics over a period of time - 1 day, 7 days, 30 days, 365 days.   

The application utilizes:  
1. Dapper ORM
2. Spectre Console
3. Postgres Database    
4. Microsoft inbuilt ConfigurationManager

We have 2 tables: CODING_SESSION, APPLICATION_USER        
The application creates a new default user if there's none existing.   
Inserting into CODING_SESSION necessitates an existing user record in APPLICATION_USER.        
(Foreign Key)     
 ![console](https://github.com/user-attachments/assets/d2f1dff2-295b-48bc-9abb-299862f7309e)

![console2](https://github.com/user-attachments/assets/763b81ce-5e52-4343-8356-adbb27069ab8)  

--------
Code notes
Using "Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;"  for postgres snake_case >>> C# CamelCase mapping
