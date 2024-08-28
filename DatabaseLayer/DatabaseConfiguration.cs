using System.Configuration;
using System.Collections.Specialized;
using System.Reflection;
using Npgsql;
using System.Data;


namespace DatabaseLayer
{

    public class DatabaseConfiguration {
        private string _hostname {  get; set; }
        private string _port { get; set; }
        private string _username {  get; set; }
        private string _password { get; set; }
        private string _database {  get; set; }

        public string Hostname {  get { return _hostname; } }
        public string Port { get { return _port; } }
        public string Username {  get { return _username; } }
        public string Password { get { return _password; } }
        public string Database { get { return _database; } }

        public DatabaseConfiguration() {
            try
            {
                ExeConfigurationFileMap configMap = new();
                configMap.ExeConfigFilename = "app.config";
                Configuration config = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);

                KeyValueConfigurationCollection settings = config.AppSettings.Settings;

                _hostname = settings["hostname"].Value;
                _port = settings["port"].Value;
                _username = settings["username"].Value;
                _password = settings["password"].Value;
                _database = settings["database"].Value;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        } 
        public NpgsqlConnection CreateConnection()
        {
            string connectionString = $"Host={Hostname};Port={Port};Username={Username};Password={Password};Database={Database};";
            NpgsqlConnection conn = new NpgsqlConnection(connectionString) ;
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
            return conn;
        }
   
    }
 

 
}