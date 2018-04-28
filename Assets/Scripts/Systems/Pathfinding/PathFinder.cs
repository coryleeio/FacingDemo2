using UnityEngine;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;
using System;

namespace Gamepackage
{
    public class PathFinder : IPathFinder
    {
        public enum LogLevel
        {
            On,
            Off
        }

        public ILogSystem _logSystem;
        public PathFinder(ILogSystem logSystem)
        {
            _logSystem = logSystem;
        }

        private bool _threadsRunning;
        private List<Thread> _threads = new List<Thread>();
        public LogLevel PathLogging;

        private int _numberOfPathsToDrawInDebug = 5;
        private ConcurrentQueue<PathRequest> _incompletePaths = new ConcurrentQueue<PathRequest>();
        private ConcurrentQueue<PathRequest> _completePaths = new ConcurrentQueue<PathRequest>();
        private Queue<PathRequest> _previouslyCompletedPaths = new Queue<PathRequest>();

        private GridGraph _grid;
        public GridGraph Grid
        {
            get
            {
                return _grid;
            }
            private set
            {
                _grid = value;
            }
        }

        public void StartPath(Point start, Point end, OnPathComplete handler)
        {
            StartPath(start.X, start.Y, end.X, end.Y, handler);
        }

        public void StartPath(int startX, int startY, int endX, int endY, OnPathComplete handler)
        {
            Log("Pathfinding: Started path!");
            _incompletePaths.Enqueue(new PathRequest()
            {
                StartX = startX,
                StartY = startY,
                EndX = endX,
                EndY = endY,
                Handler = handler
            });
        }

        public void Init(int gridSizeX, int gridSizeY, GridGraph.DiagonalOptions diagonalSetting, int numberOfThreads)
        {
            Grid = new GridGraph(gridSizeX, gridSizeY, diagonalSetting);
            Log("Pathfinding: Started with " + numberOfThreads + " threads.");
            for (var i = 0; i < numberOfThreads; i++)
            {
                _threads.Add(new Thread(PathingWorker));
            }

            foreach (var thread in _threads)
            {
                thread.Start();
            }
        }

        public void Process()
        {
            while (_completePaths.Count > 0)
            {
                // call completed path handlers in the main unity thread
                PathRequest completedRequest;
                _completePaths.TryDequeue(out completedRequest);
                if (completedRequest != null)
                {
                    LogCompletedRequest(completedRequest);
                    completedRequest.Handler(completedRequest.Path);
                    if (_previouslyCompletedPaths.Count > _numberOfPathsToDrawInDebug)
                    {
                        _previouslyCompletedPaths.Dequeue();
                    }
                    _previouslyCompletedPaths.Enqueue(completedRequest);
                }
            }
        }

        public void Cleanup()
        {
            // If the thread is still running, we should shut it down,
            // otherwise it can prevent the game from exiting correctly.
            if (_threadsRunning)
            {
                // This forces the while loop in the ThreadedWork function to abort.
                _threadsRunning = false;

                foreach (var thread in _threads)
                {
                    thread.Join();
                }
                // This waits until the thread exits,
                // ensuring any cleanup we do after this is safe. 
            }
            _threads.Clear();
        }

        private void Log(String log)
        {
            if (PathLogging == LogLevel.On)
            {
                _logSystem.Log(log);
            }
        }

        private void LogCompletedRequest(PathRequest request)
        {
            if (PathLogging == LogLevel.On)
            {
                var min = request.TimeToFind.Minutes;
                var sec = request.TimeToFind.Seconds;
                var milli = request.TimeToFind.Milliseconds;
                _logSystem.Log(string.Format("Pathfinding: Thread {0} Completed path {1},{2} -> {3},{4} in: {5}m:{6}s.{7}", request.ThreadId, request.StartX, request.StartY, request.EndX, request.EndY, min, sec, milli));
            }
        }

        private void PathingWorker()
        {
            PathSolver solver = new PathSolver();
            Stopwatch watch = new Stopwatch();
            _threadsRunning = true;
            bool workDone = false;

            // This pattern lets us interrupt the work at a safe point if neeeded.
            while (_threadsRunning && !workDone)
            {
                PathRequest incompletePath;
                _incompletePaths.TryDequeue(out incompletePath);
                if (incompletePath != null)
                {
                    watch.Reset(); ;
                    watch.Start();
                    incompletePath.Path = solver.FindPath(incompletePath.StartX, incompletePath.StartY, incompletePath.EndX, incompletePath.EndY, Grid);
                    watch.Stop();
                    incompletePath.TimeToFind = watch.Elapsed;
                    incompletePath.ThreadId = Thread.CurrentThread.ManagedThreadId;
                    _completePaths.Enqueue(incompletePath);
                }
            }
            _threadsRunning = false;
        }

        private Vector3 FlipForDrawing(float x, float y)
        {
            return new Vector3(x, -y, 0.0f);
        }

        private class PathRequest
        {
            public int StartX;
            public int StartY;
            public int EndX;
            public int EndY;
            public int ThreadId;
            public Path Path = null;
            public OnPathComplete Handler;
            public TimeSpan TimeToFind;
        }
    }
}



