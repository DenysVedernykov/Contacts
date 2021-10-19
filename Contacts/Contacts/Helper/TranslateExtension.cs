using Contacts.Services.SettingsManager;
using System;
using System.Globalization;
using System.Reflection;
using System.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Contacts.Helper
{
    [ContentProperty("Text")]
    public class TranslateExtension : IMarkupExtension
    {
        readonly CultureInfo _CultureInfo;
        public TranslateExtension()
        {
            _CultureInfo = new CultureInfo(new SettingsManager().Lang);
        }

        public string Text { get; set; }
        public string Content { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Text == null)
                return "";

            ResourceManager resmgr = new ResourceManager("Contacts.Resource", typeof(TranslateExtension).GetTypeInfo().Assembly);
            
            var translation = resmgr.GetString(Text, _CultureInfo);

            if (translation == null)
            {
                translation = Text;
            }

            return translation;
        }
    }
}
