using System;
using FluentUnions;

namespace TestAnalyzerControlFlow
{
    class TestCases
    {
        // Test case 1: Original working case - should not produce warning
        void TestIsSuccessCheck(Result<int> result)
        {
            if (result.IsSuccess)
            {
                Console.WriteLine(result.Value); // No warning expected
            }
        }

        // Test case 2: IsFailure with throw - should not produce warning
        void TestIsFailureWithThrow(Result<int> result)
        {
            if (result.IsFailure) throw new InvalidOperationException();
            
            Console.WriteLine(result.Value); // No warning expected - failure case exits
        }

        // Test case 3: IsFailure with return - should not produce warning
        int TestIsFailureWithReturn(Result<int> result)
        {
            if (result.IsFailure) return -1;
            
            return result.Value; // No warning expected - failure case exits
        }

        // Test case 4: Negated IsSuccess with throw - should not produce warning
        void TestNegatedIsSuccessWithThrow(Result<int> result)
        {
            if (!result.IsSuccess) throw new InvalidOperationException();
            
            Console.WriteLine(result.Value); // No warning expected - failure case exits
        }

        // Test case 5: IsFailure with else - should produce warning
        void TestIsFailureWithElse(Result<int> result)
        {
            if (result.IsFailure)
                throw new InvalidOperationException();
            else
                Console.WriteLine("Success");
            
            Console.WriteLine(result.Value); // Warning expected - else branch doesn't exit
        }

        // Test case 6: No check - should produce warning
        void TestNoCheck(Result<int> result)
        {
            Console.WriteLine(result.Value); // Warning expected - no check
        }

        // Test case 7: Error access after IsSuccess exit - should not produce warning
        void TestErrorAfterSuccessExit(Result<int> result)
        {
            if (result.IsSuccess) return;
            
            Console.WriteLine(result.Error); // No warning expected - success case exits
        }

        // Test case 8: Error access after negated IsFailure exit - should not produce warning
        void TestErrorAfterNegatedFailureExit(Result<int> result)
        {
            if (!result.IsFailure) throw new InvalidOperationException();
            
            Console.WriteLine(result.Error); // No warning expected - success case exits
        }

        // Test case 9: Complex case with break in loop
        void TestWithBreakInLoop(Result<int> result)
        {
            while (true)
            {
                if (result.IsFailure) break;
                
                Console.WriteLine(result.Value); // No warning expected - failure case exits loop
                return;
            }
        }

        // Test case 10: Top-level statement simulation
        void TestTopLevelStatement()
        {
            Result<int> result = Result.Success(42);
            
            if (result.IsFailure) throw new InvalidOperationException();
            
            Console.WriteLine(result.Value); // No warning expected - failure case exits
        }
    }
}