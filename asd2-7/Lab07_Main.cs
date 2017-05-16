﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASD
{

    class BridgeCrossingTestCase : TestCase
    {
        private int[] times;
        private int expectedResult;
        private int result;
        private List<List<int>> strategy;

        public BridgeCrossingTestCase(double timeLimit, int[] t, int expRes) : base(timeLimit, null)
        {
            this.times = t;
            this.expectedResult = expRes;
        }

        public override void PerformTestCase()
        {
            result = BridgeCrossing.CrossBridge(times, out strategy);
        }

        public override void VerifyTestCase(out Result resultCode, out string message)
        {
            if (result != expectedResult)
            {
                resultCode = Result.BadResult;
                message = string.Format("incorrect result: {0} (expected: {1})", result, expectedResult);
                return;
            }
            if (strategy == null || strategy.Count == 0)
            {
                resultCode = Result.BadResult;
                message = "empty strategy";
                return;
            }

            int strategyResult = 0;
            List<int> leftRiverside = Enumerable.Range(0, times.Length).ToList();
            List<int> rightRiverside = new List<int>();
            bool fromLeftToRight = true;
            foreach (List<int> crossing in strategy)
            {
                if (crossing == null || crossing.Count == 0)
                {
                    resultCode = Result.BadResult;
                    message = "empty list in strategy, returned strategy: " + StrategyToString(strategy);
                    return;
                }
                if (crossing.Count > 2)
                {
                    resultCode = Result.BadResult;
                    message = "too many people in one crossing, returned strategy: " + StrategyToString(strategy);
                    return;
                }
                if (crossing.Any(x => x < 0 || x >= times.Length))
                {
                    resultCode = Result.BadResult;
                    message = "incorrect person id, returned strategy: " + StrategyToString(strategy);
                    return;
                }


                for (int i = 0; i < crossing.Count; i++)
                {
                    if ((fromLeftToRight && !leftRiverside.Contains(crossing[i])) || (!fromLeftToRight && !rightRiverside.Contains(crossing[i])))
                    {
                        resultCode = Result.BadResult;
                        message = "person " + crossing[i] + " is on the other river side, returned strategy " + StrategyToString(strategy);
                        return;
                    }
                    if (fromLeftToRight)
                    {
                        leftRiverside.Remove(crossing[i]);
                        rightRiverside.Add(crossing[i]);
                    }
                    else
                    {
                        rightRiverside.Remove(crossing[i]);
                        leftRiverside.Add(crossing[i]);
                    }
                }

                strategyResult += crossing.Max(x => times[x]);
                fromLeftToRight = !fromLeftToRight;
            }

            if (leftRiverside.Count != 0)
            {
                resultCode = Result.BadResult;
                message = "not everyone crossed bridge, returned stratedy: " + StrategyToString(strategy);
                return;
            }

            if (result != strategyResult)
            {
                resultCode = Result.BadResult;
                message = string.Format("incorrect result computed based on strategy: {0} (expected: {1}), reutned strategy: ", strategyResult, result, StrategyToString(strategy));
                return;
            }

            resultCode = Result.Success;
            message = "OK";
        }

        private string StrategyToString(List<List<int>> strategy)
        {
            StringBuilder result = new StringBuilder();
            foreach (List<int> l in strategy)
            {
                result.Append("{");
                if (l != null)
                    result.Append(String.Join(",", l));
                result.Append("} ");
            }
            return result.ToString();
        }
    }

    class Lab07
    {

        static void Main()
        {
            bool isExtraTests = true;
            bool runCustomTests = true;
            bool runLargeCustomTests = true;
            bool runExtremelyLargeTests = false;

            Console.WriteLine("Running boilerplate task");
            long boilerplateTaskTimeElapsed = PerformBoilerplateTask();
            Console.WriteLine("Boilerplate task done\n");

            TestSet set = new TestSet();
            set.TestCases.Add(new BridgeCrossingTestCase(5, new int[] { 1, 2, 5, 10 }, 17));
            set.TestCases.Add(new BridgeCrossingTestCase(5, new int[] { 15, 10, 6, 7 }, 42));
            set.TestCases.Add(new BridgeCrossingTestCase(5, new int[] { 10, 25, 20, 5 }, 60));
            set.TestCases.Add(new BridgeCrossingTestCase(5, new int[] { 100 }, 100));
            set.TestCases.Add(new BridgeCrossingTestCase(5, new int[] { 1, 20 }, 20));
            set.TestCases.Add(new BridgeCrossingTestCase(5, new int[] { 1, 10, 100 }, 111));
            set.TestCases.Add(new BridgeCrossingTestCase(5, new int[] { 2, 2, 2, 2, 2 }, 14));
            set.TestCases.Add(new BridgeCrossingTestCase(5, new int[] { 1, 2, 3, 4, 5 }, 16));
            set.TestCases.Add(new BridgeCrossingTestCase(5, new int[] { 1, 1, 5, 5, 10, 10 }, 22));
            set.TestCases.Add(new BridgeCrossingTestCase(5, GenerateTestArray(5, 555), 284));
            set.TestCases.Add(new BridgeCrossingTestCase(5, GenerateTestArray(6, 666), 346));
            set.TestCases.Add(new BridgeCrossingTestCase(5, GenerateTestArray(7, 777), 216));
            Console.WriteLine("\nBasic tests");

            long[] elapsedTime = new long[5];
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            set.PreformTests(verbose: true, checkTimeLimit: false);
            stopwatch.Stop();
            elapsedTime[0] = stopwatch.ElapsedMilliseconds;


            if (isExtraTests)
            {
                TestSet set2 = new TestSet();
                set2.TestCases.Add(new BridgeCrossingTestCase(3, GenerateTestArray(8, 888), 313));
                set2.TestCases.Add(new BridgeCrossingTestCase(30, GenerateTestArray(9, 999), 453));
                Console.WriteLine("\nExtra tests");

                stopwatch.Restart();
                set2.PreformTests(verbose: true, checkTimeLimit: true);
                stopwatch.Stop();
                elapsedTime[1] = stopwatch.ElapsedMilliseconds;
            }

            if (runCustomTests)
            {
                TestSet set3 = new TestSet();
                set3.TestCases.Add(new BridgeCrossingTestCase(3, GenerateTestArray(8, 123), 283));
                set3.TestCases.Add(new BridgeCrossingTestCase(30, GenerateTestArray(9, 1234), 641));
                set3.TestCases.Add(new BridgeCrossingTestCase(30, GenerateTestArray(9, 5678), 305));
                set3.TestCases.Add(new BridgeCrossingTestCase(30, GenerateTestArray(9, 9012), 335));
                set3.TestCases.Add(new BridgeCrossingTestCase(30, GenerateTestArray(9, 3456), 452));
                set3.TestCases.Add(new BridgeCrossingTestCase(30, GenerateTestArray(9, 7890), 364));
                Console.WriteLine("\nCustom tests");

                stopwatch.Restart();
                set3.PreformTests(verbose: true, checkTimeLimit: false);
                stopwatch.Stop();
                elapsedTime[2] = stopwatch.ElapsedMilliseconds;
            }

            if (runLargeCustomTests)
            {
                TestSet set4 = new TestSet();
                set4.TestCases.Add(new BridgeCrossingTestCase(300, GenerateTestArray(10, 2703), 249));
                set4.TestCases.Add(new BridgeCrossingTestCase(300, GenerateTestArray(10, 6969), 245));
                set4.TestCases.Add(new BridgeCrossingTestCase(300, GenerateTestArray(10, 1), 379));
                Console.WriteLine("\nCustom large tests");

                stopwatch.Restart();
                set4.PreformTests(verbose: true, checkTimeLimit: false);
                stopwatch.Stop();
                elapsedTime[3] = stopwatch.ElapsedMilliseconds;
            }

            if (runExtremelyLargeTests)
            {
                TestSet set5 = new TestSet();
                set5.TestCases.Add(new BridgeCrossingTestCase(300, GenerateTestArray(150, 1), 3678));
                set5.TestCases.Add(new BridgeCrossingTestCase(300, GenerateTestArray(250, 2), 6268));
                set5.TestCases.Add(new BridgeCrossingTestCase(300, GenerateTestArray(300, 3), 7537));
                set5.TestCases.Add(new BridgeCrossingTestCase(300, GenerateTestArray(400, 4), 10169));
                set5.TestCases.Add(new BridgeCrossingTestCase(300, GenerateTestArray(400, 5), 9862));
                set5.TestCases.Add(new BridgeCrossingTestCase(300, GenerateTestArray(500, 6), 12615));
                set5.TestCases.Add(new BridgeCrossingTestCase(300, GenerateTestArray(600, 7), 14124));
                set5.TestCases.Add(new BridgeCrossingTestCase(300, GenerateTestArray(1500, 8), 36810));
                set5.TestCases.Add(new BridgeCrossingTestCase(300, GenerateTestArray(3000, 9), 73827));
                set5.TestCases.Add(new BridgeCrossingTestCase(300, GenerateTestArray(100000, 10), 2478340));
                Console.WriteLine("\nCustom extremely large tests");

                stopwatch.Restart();
                set5.PreformTests(verbose: true, checkTimeLimit: false);
                stopwatch.Stop();
                elapsedTime[4] = stopwatch.ElapsedMilliseconds;
            }

            Console.WriteLine("Basic tests   : {0,5} ms          ({1:F3} times the boilerplate time)", elapsedTime[0], (double)elapsedTime[0] / boilerplateTaskTimeElapsed);
            Console.WriteLine("Extra tests   : {0,5} ms          ({1:F3} times the boilerplate time)", elapsedTime[1], (double)elapsedTime[1] / boilerplateTaskTimeElapsed);
            if (runCustomTests)
                Console.WriteLine("Custom tests  : {0,5} ms          ({1:F3} times the boilerplate time)", elapsedTime[2], (double)elapsedTime[2] / boilerplateTaskTimeElapsed);
            if (runLargeCustomTests)
                Console.WriteLine("Large custom  : {0,5} ms          ({1:F3} times the boilerplate time)", elapsedTime[3], (double)elapsedTime[3] / boilerplateTaskTimeElapsed);
            if (runExtremelyLargeTests)
                Console.WriteLine("Extreme custom: {0,5} ms          ({1:F3} times the boilerplate time)", elapsedTime[4], (double)elapsedTime[4] / boilerplateTaskTimeElapsed);
        }

        static int[] GenerateTestArray(int numberOfElements, int seed)
        {
            Random r = new Random(seed);
            int[] testArray = new int[numberOfElements];
            for (int i = 0; i < numberOfElements; i++) testArray[i] = r.Next(100);
            return testArray;
        }

        static long PerformBoilerplateTask()
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            for (int i = 0; i < 100000000; i++)
            {
                if (i * (1 - i) > 0)
                    continue;
            }

            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds;
        }
    }
}