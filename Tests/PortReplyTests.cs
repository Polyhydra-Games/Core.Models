using System.Collections.Specialized;
using NUnit.Framework;

namespace PolyhydraGames.Core.Models.Test;

[TestFixture]
public class PortReplyTests
{
    [Test]
    public void NewPortReply_HasEmptyDefaults()
    {
        var r = new PortReply();
        Assert.That(r.Method, Is.EqualTo(""));
        Assert.That(r.Headers, Is.Empty);
        Assert.That(r.Query, Is.Empty);
        Assert.That(r.Content, Is.Null);
        Assert.That(r.Data, Is.Null);
    }

    [Test]
    public void Generic_ContentAndData_HoldTypedValues()
    {
        var r = new PortReply<int> { Content = 3, Data = 4 };
        Assert.That(r.Content, Is.EqualTo(3));
        Assert.That(r.Data, Is.EqualTo(4));
    }

    [Test]
    public void ToString_Empty_HasBlankHeadersAndQuery()
    {
        var r = new PortReply { Method = "ping" };
        Assert.That(r.ToString(), Is.EqualTo("Method: ping \nHeaders:  \nQuery:  "));
    }

    [Test]
    public void ToString_JoinsHeadersAndQuery()
    {
        var r = new PortReply { Method = "m" };
        r.Headers["a"] = "1";
        r.Headers["b"] = "2";
        r.Query["q"] = "x";
        Assert.That(r.ToString(), Is.EqualTo("Method: m \nHeaders: a:1, b:2 \nQuery: q:x "));
    }

    [Test]
    public void StringToPortReply_ParsesMethodWithoutQuery()
    {
        var r = "/status".ToPortReply();
        Assert.That(r.Method, Is.EqualTo("status"));
        Assert.That(r.Query, Is.Empty);
        Assert.That(r.Headers, Is.Empty);
    }

    [Test]
    public void StringToPortReply_ParsesQuery()
    {
        var r = "/go?a=1&b=2".ToPortReply();
        Assert.That(r.Method, Is.EqualTo("go"));
        Assert.That(r.Query["a"], Is.EqualTo("1"));
        Assert.That(r.Query["b"], Is.EqualTo("2"));
    }

    [Test]
    public void StringToPortReply_CopiesHeaders()
    {
        var headers = new NameValueCollection { { "X-Test", "yes" } };
        var r = "/go".ToPortReply(headers);
        Assert.That(r.Headers["X-Test"], Is.EqualTo("yes"));
    }

    [Test]
    public void StringToPortReply_EmptyString_Throws()
    {
        // Current behavior: "" has no leading slash so [1..] on an empty segment throws.
        Assert.Throws<ArgumentOutOfRangeException>(() => "".ToPortReply());
    }
}
