//Chase Brower, 2023

namespace BrowerFileBrowser.Models;

public class AsyncProxyStream : Stream
{
    private readonly Stream _baseStream;  // The underlying IBrowserFile stream

    public AsyncProxyStream(Stream baseStream)
    {
        _baseStream = baseStream;
    }

    public override bool CanRead => _baseStream.CanRead;
    public override bool CanSeek => _baseStream.CanSeek;
    public override bool CanWrite => _baseStream.CanWrite;
    public override long Length => _baseStream.Length;

    public override long Position
    {
        get => _baseStream.Position;
        set => _baseStream.Position = value;
    }

    public override void Flush()
    {
        _baseStream.Flush();
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        // Synchronize the asynchronous read operation
        var task = _baseStream.ReadAsync(buffer, offset, count);
        task.Wait();  // Wait for the task to complete
        return task.Result;
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        // Synchronize the asynchronous write operation
        var task = _baseStream.WriteAsync(buffer, offset, count);
        task.Wait();  // Wait for the task to complete
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
        return _baseStream.Seek(offset, origin);
    }

    public override void SetLength(long value)
    {
        _baseStream.SetLength(value);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _baseStream.Dispose();
        }
        base.Dispose(disposing);
    }
}
