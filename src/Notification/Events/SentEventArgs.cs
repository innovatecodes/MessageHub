using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Notification.Events
{
    public class SentEventArgs : EventArgs  
    {
        public SentEventArgs()
        {
            
        }
    }
}

//public class EmailSentEventArgs : EventArgs
//{
//    public EmailRequest Request { get; set; }
//    public string Message { get; set; }

//    public EmailSentEventArgs(EmailRequest request, string message)
//    {
//        Request = request;
//        Message = message;
//    }
//}
//}