using System;

public static class Config
{
    public static readonly string _base_api_url = "http://localhost:5203/api/";
    static Config()
    {
        if (string.IsNullOrEmpty(_base_api_url))
        {
            throw new InvalidOperationException("'_base_api_url' environment variable is not set.");
        }
    }
}