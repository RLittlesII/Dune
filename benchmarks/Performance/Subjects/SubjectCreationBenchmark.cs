using System.Reactive.Subjects;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;

namespace Performance.Subjects;

/// <summary>
/// Benchmark for a Subject.
/// </summary>
[SimpleJob(RuntimeMoniker.Net70)]
[MemoryDiagnoser]
[MarkdownExporterAttribute.GitHub]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class SubjectCreationBenchmark
{
    /// <summary>
    /// Gets or sets a parameter for how many numbers to create.
    /// </summary>
    [Params(500, 1000, 5000, 10000)]
    public int CreateNumber { get; set; }

    /// <summary>
    /// A benchmark for subject creation.
    /// </summary>
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Creation", "Memory")]
    public void Subject()
    {
        var __ =
            Enumerable.Range(0, CreateNumber)
                      .Select(_ => new Subject<int>())
                      .ToList();
    }

    /// <summary>
    /// A benchmark for async subject creation.
    /// </summary>
    [Benchmark]
    [BenchmarkCategory("Creation", "Memory")]
    public void AsyncSubject()
    {
        var __ =
            Enumerable.Range(0, CreateNumber)
                      .Select(_ => new AsyncSubject<int>())
                      .ToList();
    }

    /// <summary>
    /// A benchmark for async subject creation.
    /// </summary>
    [Benchmark]
    [BenchmarkCategory("Creation", "Memory")]
    public void BehaviorSubject()
    {
        var __ =
            Enumerable.Range(0, CreateNumber)
                      .Select(_ => new BehaviorSubject<int>(0))
                      .ToList();
    }

    /// <summary>
    /// A benchmark for async subject creation.
    /// </summary>
    [Benchmark]
    [BenchmarkCategory("Creation", "Memory")]
    public void ReplaySubject()
    {
        var __ =
            Enumerable.Range(0, CreateNumber)
                      .Select(_ => new ReplaySubject<int>())
                      .ToList();
    }
}