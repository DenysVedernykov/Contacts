using System;
using System.Collections.Generic;
using System.Text;

namespace Contacts.Services.SettingsManager
{
    public interface ISettingsManager
    {
        string Login { get; set; }
        string Password { get; set; }
    }
}
