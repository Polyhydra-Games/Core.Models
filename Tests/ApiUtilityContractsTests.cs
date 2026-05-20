using System;
using System.Collections.Generic;
using System.Text.Json;
using NUnit.Framework;
using PolyhydraGames.Core.Models.Api;

namespace PolyhydraGames.Core.Models.Tests;

public class ApiUtilityContractsTests
{
    [Test]
    public void ApiOperationResult_SerializesAndDeserializesSharedShape()
    {
        var result = new ApiOperationResult
        {
            Success = true,
            Message = "Saved",
            Errors =
            [
                new ApiValidationIssue
                {
                    Field = "title",
                    Code = "required",
                    Message = "Title is required."
                }
            ],
            Warnings = ["Using fallback sorting."],
            Metadata = new Dictionary<string, string>
            {
                ["operation"] = "save-show"
            }
        };

        var json = JsonSerializer.Serialize(result);
        var roundTrip = JsonSerializer.Deserialize<ApiOperationResult>(json);

        Assert.That(roundTrip, Is.Not.Null);
        Assert.That(roundTrip!.Success, Is.True);
        Assert.That(roundTrip.Message, Is.EqualTo("Saved"));
        Assert.That(roundTrip.Errors[0].Field, Is.EqualTo("title"));
        Assert.That(roundTrip.Warnings[0], Is.EqualTo("Using fallback sorting."));
        Assert.That(roundTrip.Metadata["operation"], Is.EqualTo("save-show"));
    }

    [Test]
    public void PagedResult_SerializesAndDeserializesSharedShape()
    {
        var result = new PagedResult<string>
        {
            Items = ["alpha", "beta", "gamma"],
            PageNumber = 2,
            PageSize = 3,
            TotalCount = 8
        };

        var json = JsonSerializer.Serialize(result);
        var roundTrip = JsonSerializer.Deserialize<PagedResult<string>>(json);

        Assert.That(roundTrip, Is.Not.Null);
        Assert.That(roundTrip!.Items, Has.Count.EqualTo(3));
        Assert.That(roundTrip.TotalPages, Is.EqualTo(3));
        Assert.That(roundTrip.HasPreviousPage, Is.True);
        Assert.That(roundTrip.HasNextPage, Is.True);
    }

    [Test]
    public void ApiReadinessSummary_SerializesAndDeserializesSharedShape()
    {
        var summary = new ApiReadinessSummary
        {
            Status = "ready",
            Message = "All checks passed.",
            CheckedAtUtc = DateTimeOffset.Parse("2026-05-20T12:00:00Z"),
            Checks =
            [
                new ApiReadinessCheck
                {
                    Name = "database",
                    Ok = true,
                    Status = "healthy"
                },
                new ApiReadinessCheck
                {
                    Name = "scanner",
                    Ok = false,
                    Status = "degraded",
                    Message = "Last scan still running."
                }
            ],
            Alerts = ["Scanner backlog is growing."],
            Metadata = new Dictionary<string, string>
            {
                ["environment"] = "release-check"
            }
        };

        var json = JsonSerializer.Serialize(summary);
        var roundTrip = JsonSerializer.Deserialize<ApiReadinessSummary>(json);

        Assert.That(roundTrip, Is.Not.Null);
        Assert.That(roundTrip!.Ready, Is.True);
        Assert.That(roundTrip.Checks, Has.Count.EqualTo(2));
        Assert.That(roundTrip.Checks[0].Name, Is.EqualTo("database"));
        Assert.That(roundTrip.Alerts[0], Is.EqualTo("Scanner backlog is growing."));
        Assert.That(roundTrip.Metadata["environment"], Is.EqualTo("release-check"));
    }
}
