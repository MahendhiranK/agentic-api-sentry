using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;

namespace AgenticApiSentry.OpenApi
{
    public static class OpenApiLoader
    {
        public static async Task<OpenApiDocument> LoadAsync(string pathOrUrl)
        {
            Stream stream;
            if (pathOrUrl.StartsWith("http", StringComparison.OrdinalIgnoreCase))
            {
                stream = await new HttpClient().GetStreamAsync(pathOrUrl);
            }
            else
            {
                stream = File.OpenRead(pathOrUrl);
            }

            using (stream)
            {
                var reader = new OpenApiStreamReader();
                var doc = reader.Read(stream, out var diag);
                if (diag?.Errors?.Count > 0)
                {
                    Console.Error.WriteLine($"OpenAPI warnings/errors: {string.Join("; ", diag.Errors.Select(e => e.Message))}");
                }
                return doc;
            }
        }
    }
}
