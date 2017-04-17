using System.Collections.Generic;

namespace FileManagementConsole
{
    public class UserActions
    {
        private Queue<UserActionType> _userActions;

        public UserActions()
        {
            this._userActions = new Queue<UserActionType>();
        }

        public void AddUserAction(UserActionType actionType)
        {
            this._userActions.Enqueue(actionType);
        }

        public UserActionType GetUserAction()
        {
            return this._userActions.Count > 0 ? this._userActions.Dequeue() : UserActionType.NONE;
        }
    }
}
