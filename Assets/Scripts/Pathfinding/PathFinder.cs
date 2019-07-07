using UnityEngine;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;
using System;

namespace Gamepackage
{
    public class PathFinder
    {
        public enum LogLevel
        {
            On,
            Off
        }


        private bool _threadsRunning;
        private List<Thread> _threads = new List<Thread>();

        private LogLevel PathLogging;
        public DiagonalOptions DiagonalSetting;
        private int _numberOfPathsToDrawInDebug = 5;
        private ConcurrentQueue<PathRequest> _incompletePaths = new ConcurrentQueue<PathRequest>();
        private ConcurrentQueue<PathRequest> _completePaths = new ConcurrentQueue<PathRequest>();
        private Queue<PathRequest> _previouslyCompletedPaths = new Queue<PathRequest>();

        public PathFinder()
        {

        }

        public void StartPath(Point start, Point end, Grid<Tile> grid, OnPathComplete handler)
        {
            Log("Pathfinding: Started path!");
            _incompletePaths.Enqueue(new PathRequest()
            {
                Start = start,
                End = end,
                Handler = handler,
                Grid = grid,
            });
        }


        public void Init(DiagonalOptions diagonalSetting, int numberOfThreads)
        {
            Log("Pathfinding: Started with " + numberOfThreads + " threads.");
            DiagonalSetting = diagonalSetting;
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
               // UnityEngine.Debug.Log(log);
            }
        }

        private void LogCompletedRequest(PathRequest request)
        {
            if (PathLogging == LogLevel.On)
            {
                var min = request.TimeToFind.Minutes;
                var sec = request.TimeToFind.Seconds;
                var milli = request.TimeToFind.Milliseconds;

                //UnityEngine.Debug.Log(string.Format("Pathfinding: Thread {0} Completed path {1} -> {2} in: {3}m:{4}s.{5}", request.ThreadId, request.Start, request.End, min, sec, milli));
                if(request.Path.HasErrors)
                {
                    foreach (var error in request.Path.Errors)
                    {
                        //UnityEngine.Debug.Log("Pathfinding: " + error);
                    }
                }
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
                    incompletePath.Path = solver.FindPath(incompletePath.Start, incompletePath.End, DiagonalSetting, incompletePath.Grid);
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
            public Point Start;
            public Point End;
            public int ThreadId;
            public Path Path = null;
            public OnPathComplete Handler;
            public TimeSpan TimeToFind;
            public Grid<Tile> Grid;
        }
    }
}



