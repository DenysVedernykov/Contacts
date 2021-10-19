namespace Contacts.Services.SettingsManager
{
    public interface ISettingsManager
    {
        bool Session { get; set; }
        string Login { get; set; }
        string Password { get; set; }
        string Sort { get; set; }
        bool NightTheme { get; set; }
        string Lang { get; set; }
    }
}
