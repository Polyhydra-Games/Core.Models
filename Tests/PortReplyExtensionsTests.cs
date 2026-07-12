using System;
using System.Collections.Specialized;
using System.Net;
using System.Threading.Tasks;
using NUnit.Framework;

namespace PolyhydraGames.Core.Models.Tests;

public class PortReplyExtensionsTests
{
    [Test]
    public void ToPortReply_WhenRequestConstructionFails_ReturnsFaultedTask()
    {
        HttpListenerRequest request = null!;
        Task<PortReply>? task = null;

        Assert.DoesNotThrow(() => task = request.ToPortReply());
        Assert.ThrowsAsync<NullReferenceException>(async () => await task!);
    }

    [Test]
    public void ToPortReplyOfT_WhenRequestConstructionFails_ReturnsFaultedTask()
    {
        HttpListenerRequest request = null!;
        Task<PortReply<int>>? task = null;

        Assert.DoesNotThrow(() => task = request.ToPortReply<int>());
        Assert.ThrowsAsync<NullReferenceException>(async () => await task!);
    }

    [Test]
    public void ToPortReply_RedactsSensitiveHeadersAndKeepsNonSensitiveHeaders()
    {
        var headers = new NameValueCollection
        {
            ["Authorization"] = "Bearer top-secret",
            ["Cookie"] = "session=top-secret",
            ["X-Api-Key"] = "top-secret",
            ["X-Access-Token"] = "top-secret",
            ["User-Agent"] = "public-client/1.0"
        };

        var reply = "/status?verbose=true".ToPortReply(headers);

        Assert.That(() => reply.Headers["Authorization"], Is.EqualTo("[REDACTED]"));
        Assert.That(() => reply.Headers["Cookie"], Is.EqualTo("[REDACTED]"));
        Assert.That(() => reply.Headers["X-Api-Key"], Is.EqualTo("[REDACTED]"));
        Assert.That(() => reply.Headers["X-Access-Token"], Is.EqualTo("[REDACTED]"));
        Assert.That(() => reply.Headers["User-Agent"], Is.EqualTo("public-client/1.0"));
    }

    [Test]
    public void ToString_DoesNotRenderCapturedValues()
    {
        var reply = new PortReply
        {
            Method = "status",
            Headers = new()
            {
                ["Authorization"] = "Bearer top-secret",
                ["User-Agent"] = "public-client/1.0"
            },
            Query = new()
            {
                ["access_token"] = "top-secret",
                ["verbose"] = "true"
            }
        };

        var rendered = reply.ToString();

        Assert.That(() => rendered, Does.Contain("Authorization"));
        Assert.That(() => rendered, Does.Contain("User-Agent"));
        Assert.That(() => rendered, Does.Contain("access_token"));
        Assert.That(() => rendered, Does.Not.Contain("top-secret"));
        Assert.That(() => rendered, Does.Not.Contain("public-client/1.0"));
        Assert.That(() => rendered, Does.Not.Contain("true"));
    }
}
