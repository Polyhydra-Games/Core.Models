#pragma warning disable CA1861 // inline expected arrays are idiomatic in NUnit assertions
using System.Net;
using System.Net.Http;
using NUnit.Framework;
using PolyhydraGames.Core.Interfaces;
using PolyhydraGames.Core.Interfaces.Enum;
using PolyhydraGames.Core.Models.Serialization;

namespace PolyhydraGames.Core.Models.Test;

[TestFixture]
[NonParallelizable]
public class WebHelpersTests
{
    private sealed class Item { public string Name { get; set; } = ""; }

    private sealed class StubHandler : HttpMessageHandler
    {
        public string Body { get; init; } = "";
        public bool Fail { get; init; }
        public string? RequestedUrl { get; private set; }
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            RequestedUrl = request.RequestUri?.ToString();
            if (Fail) throw new HttpRequestException("offline stub");
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(Body) });
        }
    }

    [Test]
    public async Task GetItemsAsync_Json_ParsesList()
    {
        var handler = new StubHandler { Body = """[{"Name":"a"},{"Name":"b"}]""" };
        using var client = new HttpClient(handler);
        var items = (await client.GetItemsAsync<Item>("http://stub.invalid/x", Serialize.Json))!.ToList();
        Assert.That(items.Select(i => i.Name), Is.EqualTo(new[] { "a", "b" }));
        Assert.That(handler.RequestedUrl, Is.EqualTo("http://stub.invalid/x"));
    }

    [Test]
    public async Task GetItemsAsync_RequestFailure_ReturnsEmptyList()
    {
        using var client = new HttpClient(new StubHandler { Fail = true });
        var items = await client.GetItemsAsync<Item>("http://stub.invalid/x", Serialize.Json);
        Assert.That(items, Is.Empty);
    }

    [Test]
    public async Task GetItemsAsync_XmlWithoutProvider_SwallowsErrorAndReturnsEmpty()
    {
        var previous = XmlExtensions.Provider;
        XmlExtensions.Provider = null;
        try
        {
            using var client = new HttpClient(new StubHandler { Body = "<a/>" });
            var items = await client.GetItemsAsync<Item>("http://stub.invalid/x", Serialize.XML);
            Assert.That(items, Is.Empty);
        }
        finally { XmlExtensions.Provider = previous; }
    }

    [Test]
    public async Task GetItemAsync_Json_ParsesObject()
    {
        using var client = new HttpClient(new StubHandler { Body = """{"Name":"solo"}""" });
        var item = await client.GetItemAsync<Item>("http://stub.invalid/x", Serialize.Json);
        Assert.That(item!.Name, Is.EqualTo("solo"));
    }

    [Test]
    public async Task GetItemAsync_RequestFailure_ReturnsNull()
    {
        using var client = new HttpClient(new StubHandler { Fail = true });
        var item = await client.GetItemAsync<Item>("http://stub.invalid/x", Serialize.Json);
        Assert.That(item, Is.Null);
    }

    [Test]
    public async Task GetItemAsync_UndefinedEncoding_IsSwallowedAndReturnsNull()
    {
        // Current behavior: the ArgumentOutOfRangeException is caught by the blanket catch.
        using var client = new HttpClient(new StubHandler { Body = "{}" });
        var item = await client.GetItemAsync<Item>("http://stub.invalid/x", (Serialize)999);
        Assert.That(item, Is.Null);
    }
}
