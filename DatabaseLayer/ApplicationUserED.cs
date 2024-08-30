using System;

public class ApplicationUserED
{
    public int UserId {get;set;}
    public string? UserName {get;set;}
	public ApplicationUserED(int userId, string userName)
	{
        this.UserId = userId;
        this.UserName = userName;   
	}
    public ApplicationUserED() { }
}
