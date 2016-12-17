using System.Collections.Generic;

namespace GlobalPollenProject.App.Validation
{
    public class PagedAppServiceResult<TModel> : AppServiceResultBase
    {
        public PagedAppServiceResult() { }
        public PagedAppServiceResult(List<TModel> result, int currentPage, int pageCount, int pageSize)
        {
            AddResult(result, currentPage, pageCount, pageSize);
        }

        public IList<TModel> Result { get; private set; }
        public int CurrentPage { get; private set; }
        public int PageCount { get; private set; }
        public int PageSize { get; private set; }

        public void AddResult(List<TModel> result, int currentPage, int pageCount, int pageSize)
        {
            Result = result;
            CurrentPage = currentPage;
            PageCount = pageCount;
            PageSize = pageSize;
        }
    }

    public class AppServiceResult<TModel> : AppServiceResultBase
    {
        public AppServiceResult() { }

        public AppServiceResult(TModel result)
        {
            this.Result = result;
        }

        public TModel Result { get; private set; }

        public void AddResult(TModel result)
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

        public void AddMessage(string key, string errorMessage, AppServiceMessageType type)
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