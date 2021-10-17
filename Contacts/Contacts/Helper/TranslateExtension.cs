using Contacts.Services.SettingsManager;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Contacts.Helper
{
    [ContentProperty("Text")]
    public class TranslateExtension : IMarkupExtension
    {
        readonly CultureInfo ci;
        private string ResourceId;
        public TranslateExtension()
        {
            ci = new CultureInfo(new SettingsManager().Lang);
            ResourceId = "Contacts.Resource";
        }

        public string Text { get; set; }
        public string Content { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Text == null)
                return "";

            ResourceManager resmgr = new ResourceManager(ResourceId, typeof(TranslateExtension).GetTypeInfo().Assembly);
            
            var translation = resmgr.GetString(Text, ci);

            if (translation == null)
            {
                translation = Text;
            }

            return translation;
        }
    }
}
