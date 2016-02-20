using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;

public class AriaBehaviour : MonoBehaviour
{
    // Implement Python-like Range syntax
    protected EnumerableRange Range (int end) { return new EnumerableRange(0, end, 1); }
    protected EnumerableRange Range (int start, int end) { return new EnumerableRange(start, end, 1); }
    protected EnumerableRange Range (int start, int end, int step) { return new EnumerableRange(start, end, step); }

    protected class EnumerableRange : IEnumerable<int>
    {
        private int _start, _end, _step;
        private bool _debugmode = false;

        public EnumerableRange(int start, int end, int step)
        {
            _start = start;
            _end = end;
            if (step == 0) { throw new InvalidOperationException(); }
            if ((_end < _start && step > 0) || (_end > _start && step < 0))
            {
                step = 0 - step;
            }
            _step = step;
        }

        public IEnumerator<int> GetEnumerator()
        {
            return new RangeEnumerator(_start, _end, _step);
        }

        private IEnumerator GetRangeEnumerator()
        {
            return this.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetRangeEnumerator();
        }
    }

    protected class RangeEnumerator : IEnumerator<int>
    {
        private int _start, _end, _step;
        private bool _debugmode = false;

        public RangeEnumerator(int start, int end, int step)
        {
            _start = start;
            _end = end;
            _step = step;
        }

        private int? _current;

        public int Current
        {
            get
            {
                if (_current == null) { throw new InvalidOperationException(); }
                return (int)_current;
            }
        }

        private object Current1
        {
            get { return this.Current; }
        }

        object IEnumerator.Current
        {
            get { return Current1; }
        }

        public void Dispose()
        {
            
        }

        public bool MoveNext()
        {
            if (_current == null)
            {
                _current = _start - _step;
            }
            
            _current = _current + _step;

            if ((_step > 0 && _current >= _end) || (_step < 0 && _current <= _end))
            {
                _current = null;
            }

            if (_current == null)
            {
                return false;
            }

            return true;
        }

        public void Reset()
        {
            _current = null;
        }
    }
}
