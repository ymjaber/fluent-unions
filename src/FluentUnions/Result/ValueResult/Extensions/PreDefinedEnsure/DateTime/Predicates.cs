using FluentUnions.PreDefinedEnsure;

namespace FluentUnions;

/// <summary>Provides pre-defined validation predicates for DateTime values in <see cref="Result{T}"/> types.</summary>
public static partial class EnsureBuilderExtensions
{
    public static EnsureBuilder<DateTime> InFuture(
        in this EnsureBuilder<DateTime> ensure,
        Error? error = null) =>
        ensure.Satisfies(v => v > DateTime.Now, error ?? DateTimeErrors.NotInFuture("DateTime"));

    public static EnsureBuilder<DateOnly> InFuture(
        in this EnsureBuilder<DateOnly> ensure,
        Error? error = null) =>
        ensure.Satisfies(v => v > DateOnly.FromDateTime(DateTime.Now),
            error ?? DateTimeErrors.NotInFuture("Date"));

    public static EnsureBuilder<TimeOnly> InFuture(
        in this EnsureBuilder<TimeOnly> ensure,
        Error? error = null) =>
        ensure.Satisfies(v => v > TimeOnly.FromDateTime(DateTime.Now),
            error ?? DateTimeErrors.NotInFuture("Time"));

    public static EnsureBuilder<DateTime> InPast(
        in this EnsureBuilder<DateTime> ensure,
        Error? error = null) =>
        ensure.Satisfies(v => v < DateTime.Now, error ?? DateTimeErrors.NotInPast("DateTime"));

    public static EnsureBuilder<DateOnly> InPast(
        in this EnsureBuilder<DateOnly> ensure,
        Error? error = null) =>
        ensure.Satisfies(v => v < DateOnly.FromDateTime(DateTime.Now),
            error ?? DateTimeErrors.NotInPast("Date"));

    public static EnsureBuilder<TimeOnly> InPast(
        in this EnsureBuilder<TimeOnly> ensure,
        Error? error = null) =>
        ensure.Satisfies(v => v < TimeOnly.FromDateTime(DateTime.Now),
            error ?? DateTimeErrors.NotInPast("Time"));

    public static EnsureBuilder<DateOnly> InFutureOrPresent(
        in this EnsureBuilder<DateOnly> ensure,
        Error? error = null) =>
        ensure.Satisfies(v => v >= DateOnly.FromDateTime(DateTime.Now),
            error ?? DateTimeErrors.DateInPast);

    public static EnsureBuilder<DateOnly> InPastOrPresent(
        in this EnsureBuilder<DateOnly> ensure,
        Error? error = null) =>
        ensure.Satisfies(v => v <= DateOnly.FromDateTime(DateTime.Now),
            error ?? DateTimeErrors.DateInFuture);
}