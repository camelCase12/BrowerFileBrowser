//Chase Brower, 2023

using System.Net;
using System.Net.Security;

namespace BrowerFileBrowser.Models;

public class CustomHttpHandler : HttpMessageHandler
{
    private readonly HttpClient _defaultHttpClient;

    public CustomHttpHandler()
    {
        var handler = new HttpClientHandler();
        if (handler.SupportsAutomaticDecompression)
        {
            handler.AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate;
        }

        // Configure handler to bypass certificate validation conditionally
        handler.ServerCertificateCustomValidationCallback = (message, certificate, chain, sslPolicyErrors) =>
        {
            return IsVideoStreamRequest(message.RequestUri);
        };

        _defaultHttpClient = new HttpClient(handler);
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // Check if the URL matches the pattern for video streaming
        if (IsVideoStreamRequest(request.RequestUri))
        {
            return await HandleVideoStreamRequest(request.RequestUri, cancellationToken);
        }

        // For all other requests, just forward them
        return await _defaultHttpClient.SendAsync(request, cancellationToken);
    }

    private bool IsVideoStreamRequest(Uri uri)
    {
        // Implement your logic to check if the URI is for video streaming
        // Example: check for a specific path or query parameter
        return uri.AbsolutePath.Contains("/streamvideo");
    }

    private async Task<HttpResponseMessage> HandleVideoStreamRequest(Uri uri, CancellationToken cancellationToken)
    {
        // Implement your logic to stream the video file
        // Example: Open the video file, read it in chunks, and return the content
        var response = new HttpResponseMessage(HttpStatusCode.OK);

        // Example: Set the response content to a stream of the video file
        response.Content = new StreamContent(new FileStream("C:\\Users\\chase\\Videos\\2023-07-04 14-12-58.mp4", FileMode.Open, FileAccess.Read));

        return response;
    }
}
