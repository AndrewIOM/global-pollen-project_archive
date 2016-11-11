using System.Collections.Generic;

namespace GlobalPollenProject.App.Validation
{
    public class AppServiceResult<TModel> : AppServiceResultBase
    {
        public TModel Result { get; private set; }

        internal void AddResult(TModel result)
        {
            Result = result;
        }
    }

    public class AppServiceResult : AppServiceResultBase { }

    public abstract class AppServiceResultBase
    {
        public AppServiceResultBase()
        {
            IsValid = true;
            _messages = new List<AppServiceMessage>();
        }

        private List<AppServiceMessage> _messages;
        public bool IsValid { get; private set; }

        public void AddError(string key, string errorMessage, AppServiceMessageType type)
        {
            var message = new AppServiceMessage(key, errorMessage, type);
            _messages.Add(message);
            if (type == AppServiceMessageType.Error) IsValid = false;
        }

        public List<AppServiceMessage> Messages
        {
            get
            {
                return _messages;
            }
        }

        public class AppServiceMessage
        {
            public AppServiceMessage(string key, string message, AppServiceMessageType messageType)
            {
                Message = message;
                MessageType = messageType;
                Key = key;
            }

            public string Message { get; private set; }
            public string Key { get; private set; }
            public AppServiceMessageType MessageType { get; private set; }
        }

    }

    public enum AppServiceMessageType
    {
        Info,
        Warning,
        Error,
    }

}