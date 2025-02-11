namespace DevKid.src.Application.Constant
{
    public class RoleConst
    {
        public const string ADMIN = "ADMIN";
        public const int ADMIN_ID = 1;
        public const string MANAGER = "MANAGER";
        public const int MANAGER_ID = 2;
        public const string STUDENT = "STUDENT";
        public const int STUDENT_ID = 3;
        public Dictionary<string, int> RoleId = new()
        {
            {ADMIN, ADMIN_ID},
            {MANAGER, MANAGER_ID},
            {STUDENT, STUDENT_ID}
        };
        public static int GetRoleId(string roleName)
        {
            return new RoleConst().RoleId[roleName];
        }
    }
}
