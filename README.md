# CodingTracker  

The user adds their time spent 'coding' - or any other task - and the application should store and calculate total time and categorize entries.   

The application should utilize:  
1. Dapper ORM
2. Spectre Console
3. Postgres
4. Microsoft inbuilt ConfigurationManager

Validation should be added, refusing empty entries or invalid dates.        
Additionally, a real time tracker could be added: the user starts() a Stopwatch and when he's finished coding, he calls stop() and a new record with startDate and stopDate and other calculated fields is added to the program.   
Users should be able to see Total Time and Daily Average Time over a period - month, week.    
Example:   
Week Summary (last 7 days): 
  Total: 459 minutes
  Average: 65.5 minutes per day [Note: 459/7] 

Nice to add: ability to set Coding Goals.



We have 2 tables: CODING_SESSION, APPLICATION_USER    
The application should get the active user.     
Inserting into CODING_SESSION necessitates an user that exists in APPLICATION_USER.    
(Foreign Key)
