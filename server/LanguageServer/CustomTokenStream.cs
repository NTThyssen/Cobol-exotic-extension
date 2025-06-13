using System;
using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;

/// <summary>
/// A CharStream that can push and pop underlying streams, so included files
/// can be seamlessly lexed as part of one continuous stream.
/// </summary>
public class StackedCharStream : ICharStream {
    private readonly Stack<ICharStream> _streams = new Stack<ICharStream>();

    public StackedCharStream(ICharStream initial) {
        _streams.Push(initial);
    }

    /// <summary>Push a new file stream onto the stack.</summary>
    public void Push(ICharStream stream) {
        _streams.Push(stream);
    }

    /// <summary>Pop the current stream when it reaches EOF.</summary>
    private void MaybePopOnEof() {
        while (_streams.Count > 1 && _streams.Peek().La(1) == IntStreamConstants.Eof) {
            _streams.Pop();
        }
    }

    public void Consume() {
        _streams.Peek().Consume();
    }

    public int Mark() => _streams.Peek().Mark();
    public void Release(int marker) => _streams.Peek().Release(marker);
    public int Index {
        get {
            MaybePopOnEof();
            return _streams.Peek().Index;
        }
    }
    public int Size {
        get {
            // Size might not be meaningful across multiple files;
            // return EOF when only one stream remains.
            return _streams.Peek().Size;
        }
    }
    public string SourceName {
        get {
            return _streams.Peek().SourceName;
        }
    }

    public string GetText(Interval interval) {
        // If interval spans files, you'd need to stitch; for simplicity,
        // only support single-stream ranges.
        return _streams.Peek().GetText(interval);
    }

    public int La(int i)
    {
        MaybePopOnEof();
        return _streams.Peek().La(i);
    }

        public void Seek(int index) {
        // Delegate seeking to the current top stream
        MaybePopOnEof();
        _streams.Peek().Seek(index);
    }
}