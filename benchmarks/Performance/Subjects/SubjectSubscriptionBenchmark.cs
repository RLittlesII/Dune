using System.Reactive.Subjects;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;

namespace Performance.Subjects;

/// <summary>
/// Benchmark for the RxObject.
/// </summary>
[SimpleJob(RuntimeMoniker.Net70)]
[MemoryDiagnoser]
[MarkdownExporterAttribute.GitHub]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class SubjectSubscriptionBenchmark
{
    /// <summary>
    /// Gets or sets a parameter for how many numbers to create.
    /// </summary>
    [Params(500, 1000, 5000, 10000)]
    public int CreateNumber { get; set; }

    /// <summary>
    /// A benchmark for subject usage.
    /// </summary>
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Usage", "Memory")]
    public void Subject()
    {
        var subject = new Subject<int>();
        using var _ = subject.Subscribe();

        Array.ForEach(Enumerable.Range(0, CreateNumber)
                                .ToArray(),
                      subject.OnNext);

        subject.OnCompleted();
    }

    /// <summary>
    /// A benchmark for async subject usage.
    /// </summary>
    [Benchmark]
    [BenchmarkCategory("Usage", "Memory")]
    public void AsyncSubject()
    {
        var asyncSubject = new AsyncSubject<int>();
        using var _ = asyncSubject.Subscribe();

        Array.ForEach(Enumerable.Range(0, CreateNumber)
                                .ToArray(),
                      asyncSubject.OnNext);

        asyncSubject.OnCompleted();
    }

    /// <summary>
    /// A benchmark for behavior subject usage.
    /// </summary>
    [Benchmark]
    [BenchmarkCategory("Usage", "Memory")]
    public void BehaviorSubject()
    {
        var behaviorSubject = new BehaviorSubject<int>(0);
        using var _ = behaviorSubject.Subscribe();

        Array.ForEach(Enumerable.Range(0, CreateNumber)
                                .ToArray(),
                      behaviorSubject.OnNext);

        behaviorSubject.OnCompleted();
    }

    /// <summary>
    /// A benchmark for replay subject usage.
    /// </summary>
    [Benchmark]
    [BenchmarkCategory("Usage", "Memory")]
    public void ReplaySubject()
    {
        var replaySubject = new ReplaySubject<int>();
        using var _ = replaySubject.Subscribe();

        Array.ForEach(Enumerable.Range(0, CreateNumber)
                                .ToArray(),
                      replaySubject.OnNext);

        replaySubject.OnCompleted();
    }
}