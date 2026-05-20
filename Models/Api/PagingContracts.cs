using System;
using System.Collections.Generic;

namespace PolyhydraGames.Core.Models.Api;

/// <summary>
/// Sort direction used by paging helpers.
/// </summary>
public enum SortDirection
{
    Ascending = 0,
    Descending = 1
}

/// <summary>
/// Portable paging request contract.
/// </summary>
public sealed record PagedRequest
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 50;
    public string? Query { get; init; }
    public string? SortBy { get; init; }
    public SortDirection SortDirection { get; init; } = SortDirection.Ascending;
    public IReadOnlyList<SortRule> Sorts { get; init; } = [];
    public IReadOnlyList<FilterRule> Filters { get; init; } = [];
}

/// <summary>
/// Portable paging result contract.
/// </summary>
public sealed record PagedResult<T>
{
    public IReadOnlyList<T> Items { get; init; } = [];
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 50;
    public int TotalCount { get; init; }
    public int TotalPages => PageSize <= 0 ? 0 : (int)Math.Ceiling(TotalCount / (double)PageSize);
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;
}

/// <summary>
/// Portable sort rule.
/// </summary>
public sealed record SortRule
{
    public string Field { get; init; } = "";
    public SortDirection Direction { get; init; } = SortDirection.Ascending;
}

/// <summary>
/// Portable filter operator.
/// </summary>
public enum FilterOperator
{
    Equals = 0,
    NotEquals = 1,
    Contains = 2,
    StartsWith = 3,
    EndsWith = 4,
    GreaterThan = 5,
    GreaterThanOrEqual = 6,
    LessThan = 7,
    LessThanOrEqual = 8,
    In = 9,
    NotIn = 10,
    IsNull = 11,
    IsNotNull = 12
}

/// <summary>
/// Portable filter rule.
/// </summary>
public sealed record FilterRule
{
    public string Field { get; init; } = "";
    public FilterOperator Operator { get; init; } = FilterOperator.Equals;
    public string? Value { get; init; }
    public IReadOnlyList<string> Values { get; init; } = [];
}
