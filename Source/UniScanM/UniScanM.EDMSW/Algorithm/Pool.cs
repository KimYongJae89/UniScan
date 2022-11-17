using System;
using System.Linq;
using System.Collections.Concurrent;
using System.Threading;
using System.Timers;
using UniScanM.EDMSW.Data;
using System.Threading.Tasks;

namespace UniScanM.EDMSW.Algorithm
{
    class Pool
    {
    }

    public class TimeOutHandler
    {
        public static void Wait(int timeoutTime, ManualResetEvent eventHandler)
        {
            if (eventHandler.WaitOne(timeoutTime) == false)
            {
                throw new TimeoutException();
            }
        }
    }

    public class TimeOutTimer
    {
        System.Timers.Timer timer = null;
        bool timeOut = false;
        public bool TimeOut { get => timeOut; }

        public TimeOutTimer()
        {
            timer = new System.Timers.Timer();
            timer.Elapsed += timer_Elapsed;
        }

        public void Start(int timeOutMs)
        {
            timeOut = false;
            timer.Interval = timeOutMs;
            timer.Start();
        }

        public void Restart()
        {
            timer.Stop();
            timeOut = false;
            timer.Start();
        }

        public void Stop()
        {
            timer.Stop();
        }

        public void Reset()
        {
            timer.Stop();
            timeOut = false;
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            timeOut = true;
        }

        public void ThrowIfTimeOut()
        {
            if (TimeOut)
                throw new TimeoutException();
        }
    }

    public class ObjectPool<T>
    {
        private ConcurrentBag<T> _objects;
        private Func<T> _objectGenerator;
        private Func<T, bool> _objectDisposer;
        private int _maxNum = 0;

        public int Count()
        {
            return _objects.Count();
        }

        public ObjectPool()
        {
            _maxNum = -1; // Add에 의해 추가된 개수에 대해서만 동작
            _objects = new ConcurrentBag<T>();
        }

        public ObjectPool(Func<T> objectGenerator, int maxNum = 0)
        {
            if (objectGenerator == null) throw new ArgumentNullException("objectGenerator");
            _objects = new ConcurrentBag<T>();
            _objectGenerator = objectGenerator;
            _maxNum = maxNum;

            if (_maxNum > 0)
            {
                for (int i = 0; i < _maxNum; i++)
                    _objects.Add(_objectGenerator());
            }
        }

        public ObjectPool(Func<object, T> objectGenerator, Func<T, bool> objectDisposer, object obj, int maxNum = 0)
        {
            if (objectGenerator == null) throw new ArgumentNullException("objectGenerator");
            _objects = new ConcurrentBag<T>();
            _maxNum = maxNum;

            _objectDisposer = objectDisposer;

            if (_maxNum > 0)
            {
                for (int i = 0; i < _maxNum; i++)
                    _objects.Add(objectGenerator(obj));
            }
        }

        public T GetObject()
        {
            while (true)
            {
                if (_objects.TryTake(out T item))
                    return item;

                if (_maxNum == 0)
                    return _objectGenerator();
            }
        }

        public void PutObject(T item)
        {
            _objects.Add(item);
        }

        public void Release()
        {
            if (_objectDisposer == null)
                return;

            foreach (T obj in _objects)
            {
                _objectDisposer(obj);
            }
        }
    }

    public class ObjectPoolQueue<T>
    {
        private ConcurrentQueue<T> _objects;
        private Func<T> _objectGenerator;
        private int _maxNum = 0;

        public int Count()
        {
            return _objects.Count;
        }

        public ObjectPoolQueue()
        {
            _maxNum = -1; // Add에 의해 추가된 개수에 대해서만 동작
            _objects = new ConcurrentQueue<T>();
        }

        public ObjectPoolQueue(Func<T> objectGenerator, int maxNum = 0)
        {
            if (objectGenerator == null) throw new ArgumentNullException("objectGenerator");
            _objects = new ConcurrentQueue<T>();
            _objectGenerator = objectGenerator;
            _maxNum = maxNum;

            if (_maxNum > 0)
            {
                for (int i = 0; i < _maxNum; i++)
                    _objects.Enqueue(_objectGenerator());
            }
        }

        public T GetObject(int timeOut)
        {
            TimeOutTimer timeOutTimer = new TimeOutTimer();
            timeOutTimer.Start(timeOut);

            T item;
            while (true)
            {
                if (_objects.TryDequeue(out item)) return item;

                if (_maxNum == 0)
                    return _objectGenerator();

                Thread.Sleep(10);

                //timeOutTimer.ThrowIfTimeOut();
                if (timeOutTimer.TimeOut)
                    return item;
            }
        }

        public T CopyObject(int timeOut)
        {
            TimeOutTimer timeOutTimer = new TimeOutTimer();
            timeOutTimer.Start(timeOut);

            T item;
            while (true)
            {
                if (_objects.TryPeek(out item)) return item;

                if (_maxNum == 0)
                    return _objectGenerator();

                Thread.Sleep(10);

                //timeOutTimer.ThrowIfTimeOut();
                if (timeOutTimer.TimeOut)
                    return item;
            }
        }

        public void PutObject(T item)
        {
            _objects.Enqueue(item);
        }

        public void ClearObject()
        {
            T item;

            while (_objects.Count() != 0)
                _objects.TryDequeue(out item);
        }
    }


    public class SyncRunner 
    {
        private bool _syncDisable = false;


        private int _delayCount;

        //private AutoResetEvent _resetEvent = new AutoResetEvent(false);
        private CancellationTokenSource _cts;

        private ConcurrentQueue<InspectionResult> _queue = new ConcurrentQueue<InspectionResult>();


        private int _count;
        public int Count
        {
            get { return _count; }
            set { _count = value; }
        }


        public SyncRunner()
        {

        }

        public void ImageGrabbed( )
        {
            if (_cts.IsCancellationRequested == true)
                return;

            _delayCount++;

            //if (_delayCount >= _syncInfo.BufferCount)
            //    _resetEvent.Set();
        }

        public void Run()
        {
            if (_cts?.IsCancellationRequested == false)
                return;

            _cts = new CancellationTokenSource();
            //_resetEvent.Reset();
            InspectionResult result=null;
            Thread thread = new Thread(() =>
            {
                //_resetEvent.WaitOne();

                while (_cts.IsCancellationRequested == false)
                {
                    try
                    {
                        //if (_delayCount < _syncInfo.BufferCount)
                        //{
                        //    Thread.Sleep(0);
                        //    continue;
                        //}

                        if (_cts.IsCancellationRequested)
                            break;

                        while (!_queue.TryDequeue(out result))
                        {
                            Thread.Sleep(1);
                        }

                        _delayCount--;
  

                        Count = _queue.Count;
                    }
                    catch (Exception e)
                    {
                        //_logService.Logging(new Datas.PlasterLog(Datas.PlasterLogType.Inspection, Microsoft.Extensions.Logging.LogLevel.Error, e.Message));
                    }
                }

                while (!_queue.IsEmpty)
                    _queue.TryDequeue(out var result2);

                Count = _queue.Count;
                _delayCount = 0;
            });

            thread.Priority = ThreadPriority.Highest;
            thread.Start();
        }

        public void Cancle()
        {
            _cts?.Cancel();
            //_resetEvent.Set();
        }

        public void Enqueue()
        {
            if (_cts.IsCancellationRequested)
                return;

           // _queue.Enqueue(data);

            Count = _queue.Count;
        }
    }
    public  class JobRunner<T> 
    {
        public bool IsRun => _cts == null ? true : _cts.IsCancellationRequested == false;

        private int _count;
        public int Count
        {
            get => _count;
            set { _count = value; }
        }

        CancellationTokenSource _cts;
        protected CancellationTokenSource Cts => _cts;

        private ConcurrentQueue<T> _queue = new ConcurrentQueue<T>();
        protected ConcurrentQueue<T> Queue => _queue;
        private ManualResetEvent _resetEvent = new ManualResetEvent(false);


        public JobRunner()
        {

        }

        protected void Job(T data)
        {

        }

        protected virtual void PrevRun()
        {

        }

        protected virtual void PostRun()
        {

        }

        protected virtual void PrevCancle()
        {

        }

        protected virtual void PostCancle()
        {

        }

        public void Run()
        {
            if (_cts?.IsCancellationRequested == false)
                return;

            _cts = new CancellationTokenSource();

            Task.Factory.StartNew(() =>
            {
                PrevRun();

                while (_cts.IsCancellationRequested == false)
                {
                    try
                    {
                        if (_queue.IsEmpty)
                        {
                            _resetEvent.Reset();
                            _resetEvent.WaitOne();
                        }

                        if (_queue.TryDequeue(out T data))
                        {
                            Job(data);
                            Count = _queue.Count;
                        }
                    }
                    catch (Exception e)
                    {
                        //_logService.Logging(new Datas.PlasterLog(Datas.PlasterLogType.Inspection, Microsoft.Extensions.Logging.LogLevel.Error, e.Message));
                        //_ioService.Cancle();
                        //_inspectService.Stop();
                    }
                }

                while (!_queue.IsEmpty)
                    _queue.TryDequeue(out var result);

                Count = _queue.Count;

                PostRun();
            });
        }

        public void Cancle()
        {
            PrevCancle();

            _cts?.Cancel();
            _resetEvent.Set();

            PostCancle();
        }

        public virtual void Enqueue(T data)
        {
            if (_cts.IsCancellationRequested)
                return;

            lock (_queue)
                _queue.Enqueue(data);

            _resetEvent.Set();
            Count = _queue.Count;
        }
    }
}
