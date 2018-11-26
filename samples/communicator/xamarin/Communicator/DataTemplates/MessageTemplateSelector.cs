using Xamarin.Forms;

namespace Communicator.DataTemplates
{

    public class MessageTemplateSelector : DataTemplateSelector
    {
        public DataTemplate SenderTemplate { get; set; }
        public DataTemplate RecieverTemplate { get; set; }
        public DataTemplate EventTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {

            return ((Message)item).IsNotification ? EventTemplate : ((Message)item).IsSender ? SenderTemplate : RecieverTemplate;

        }
    }
}
