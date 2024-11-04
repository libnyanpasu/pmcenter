namespace pmcenter.CallbackActions
{
    public class CallbackActionResult
    {
        public bool ShowAsAlert;
        public string Status;
        public bool Succeeded;

        public CallbackActionResult(string status, bool showAsAlert = false, bool succeeded = true)
        {
            Status = status;
            Succeeded = succeeded;
            ShowAsAlert = showAsAlert;
        }
    }
}
