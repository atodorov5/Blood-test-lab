using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodTestLab
{
    public class UserInfo
    {
        private int userID;

        public int UserID
        {
            get { return userID; }
        }

        public UserInfo(int userID)
        {
            this.userID = userID;
        }
    }

    public static class GlobalInfo
    {
        private static UserInfo currentUser;

        public static UserInfo CurrentUser
        {
            get { return currentUser; }
            set { currentUser = value; }
        }
    }
}
