namespace Squabble.Helpers
{
    public static class JwtHelpers
    {
        public const string Issuer = "Squabble";
        public const string Audience = "ApiUser";
        public const string Key = "&E)H@McQfTjWnZq4t7w!z%C*F-JaNdRg";

        //Lifespan of token in hours
        public const byte ExpiresIn = 12;

    }
}