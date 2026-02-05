namespace MOHPortal.Core.Umbraco.NotificationHooks
{
    internal class NotificationStateManager
    {
        private ExecutionState _executionState = ExecutionState.Normal;
        public ExecutionState State => _executionState;   
        
        public void SupressNotifications() 
        {
            _executionState = ExecutionState.Suppressed;
        }

        public void ResetState()
        {
            _executionState = ExecutionState.Normal;
        }
    }
    
    internal enum ExecutionState
    {
        Normal,
        Suppressed
    }
}
