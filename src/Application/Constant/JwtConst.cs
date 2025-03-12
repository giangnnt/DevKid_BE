namespace DevKid.src.Application.Constant
{
    public static class JwtConst
    {
        public const int ACCESS_TOKEN_EXP = 60 * 60; // 1h
        public const int REFRESH_TOKEN_EXP = 3600 * 24 * 30; // 30 days
    }
}
