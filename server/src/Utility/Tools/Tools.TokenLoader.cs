namespace Thuai.Server.Utility;

public static partial class Tools
{
    public static class TokenLoader
    {
        public static string[] LoadTokens(Config.TokenSettings tokenSettings)
        {
            if (tokenSettings.LoadTokenFromEnv)
            {
                return LoadTokensFromEnv(tokenSettings.TokenLocation, tokenSettings.TokenDelimiter);
            }
            else
            {
                return LoadTokens(tokenSettings.TokenLocation, tokenSettings.TokenDelimiter);
            }
        }

        public static string[] LoadTokensFromEnv(string envVarName, char delimiter)
        {
            string? tokenString = Environment.GetEnvironmentVariable(envVarName)
                ?? throw new Exception($"Token environment variable {envVarName} not found.");
            string[] tokens = tokenString.Split(delimiter);

            if (tokens.Length == 0)
            {
                throw new Exception($"No tokens found in environment variable {envVarName}.");
            }

            return tokens;
        }

        public static string[] LoadTokens(string path, char delimiter)
        {
            if (File.Exists(path) == false)
            {
                throw new Exception($"Token file not found at {LogHandler.Truncate(path, 256)}.");
            }

            string[] tokens = File.ReadAllText(path).Split(delimiter);

            if (tokens.Length == 0)
            {
                throw new Exception($"No tokens found in {LogHandler.Truncate(path, 256)}.");
            }

            return tokens;
        }
    }
}
