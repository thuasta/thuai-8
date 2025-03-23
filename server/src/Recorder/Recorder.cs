using System.IO.Compression;
using System.Text.Json;
using Serilog;
using Thuai.Server.Utility;

namespace Thuai.Server.Recorder;

public partial class Recorder : IDisposable
{
    public const int MaxRecordsBeforeSave = 10000;

    private readonly RecordPage _recordPage = new();

    private readonly ILogger _logger = Tools.LogHandler.CreateLogger("Recorder");
    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        WriteIndented = true
    };

    private readonly string _recordsDir;
    private readonly string _targetRecordFileName;
    private readonly string _targetResultFileName;
    private readonly string _copyRecordDir;

    private readonly object _saveLock = new();

    #region Constructors and finalizers
    /// <summary>
    /// Create a new recorder.
    /// </summary>
    public Recorder(string recordsDir, string targetRecordFileName, string targetResultFileName)
    {
        _recordsDir = recordsDir;
        _targetRecordFileName = targetRecordFileName;
        _targetResultFileName = targetResultFileName;
        _copyRecordDir = Path.Combine(_recordsDir, "copy", $"{DateTime.UtcNow:yyyy-MM-dd-HH-mm-ss}-{Guid.NewGuid()}");

        // Remove record file if it exists.
        if (File.Exists(Path.Combine(_recordsDir, _targetRecordFileName)))
        {
            File.Delete(Path.Combine(_recordsDir, _targetRecordFileName));
        }
        if (File.Exists(Path.Combine(_recordsDir, _targetResultFileName)))
        {
            File.Delete(Path.Combine(_recordsDir, _targetResultFileName));
        }

        if (!Directory.Exists(_recordsDir))
        {
            Directory.CreateDirectory(_recordsDir);
        }
        if (!Directory.Exists(_copyRecordDir))
        {
            Directory.CreateDirectory(_copyRecordDir);
        }
    }
    #endregion

    #region Methods
    public void Dispose()
    {
        Save();
    }

    public void Record(params Protocol.IRecordable[] records)
    {
        // Record should not be null
        foreach (Protocol.IRecordable record in records)
        {
            if (record is null)
            {
                _logger.Error("Null record passed to Recorder.Record().");
                return;
            }
            _logger.Debug($"Adding record {record.GetType().Name}");
        }

        _recordPage.Record(records);
        if (_recordPage.Length >= MaxRecordsBeforeSave)
        {
            Save();
        }
    }

    public void Save()
    {
        if (Monitor.TryEnter(_saveLock))
        {
            try
            {
                lock (_saveLock)
                {
                    if (_recordPage.Length == 0)
                    {
                        return;
                    }

                    string recordJson = _recordPage.Export();

                    string recordFilePath = Path.Combine(_recordsDir, _targetRecordFileName);

                    long timestamp = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds * 1000;
                    string pageName = $"{timestamp}-{Guid.NewGuid()}.json";

                    _logger.Debug($"Adding new record {pageName} to {_targetRecordFileName}.");

                    // Create directory if it doesn't exist.
                    if (!Directory.Exists(_recordsDir))
                    {
                        Directory.CreateDirectory(_recordsDir);
                    }

                    // Write records to file.
                    using FileStream zipFile = new(recordFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
                    using ZipArchive archive = new(zipFile, ZipArchiveMode.Update);
                    ZipArchiveEntry entry = archive.CreateEntry(pageName, CompressionLevel.SmallestSize);
                    using StreamWriter writer = new(entry.Open());
                    writer.Write(recordJson);
                    writer.Close();

                    // Create a copy of the record file.
                    using FileStream copyZipFile
                        = new(Path.Combine(_copyRecordDir, $"{timestamp}.dat"), FileMode.Create);
                    using ZipArchive copyArchive = new(copyZipFile, ZipArchiveMode.Create);
                    ZipArchiveEntry copyEntry
                        = copyArchive.CreateEntry("record.json", CompressionLevel.SmallestSize);
                    using StreamWriter copyWriter = new(copyEntry.Open());
                    copyWriter.Write(recordJson);
                    copyWriter.Close();
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to save records: {ex.Message}");
                _logger.Debug($"{ex}");
            }
            finally
            {
                Monitor.Exit(_saveLock);
            }

        }
        else
        {
            // If the lock is not acquired, it means that another thread is saving the records.
            // In this case, we just return.
            return;
        }
    }

    public void SaveResults(Dictionary<GameLogic.Player, int> scoreboard)
    {
        Result results = new()
        {
            Scores = scoreboard.ToDictionary(
                kvp => kvp.Key.Token,
                kvp => kvp.Value
            )
        };

        string resultFilePath = Path.Combine(_recordsDir, _targetResultFileName);
        File.WriteAllText(
            resultFilePath,
            JsonSerializer.Serialize(results, _jsonSerializerOptions)
        );
    }
    #endregion
}
