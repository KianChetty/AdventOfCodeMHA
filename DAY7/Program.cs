using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;  

class DAY7
{
    static void Main()
    {
        string[] equations = File.ReadAllLines("puzzleInput.txt");

        BigInteger totalCalibrationResult = 0;

        foreach (var equation in equations)
        {
            string[] parts = equation.Split(':');
            BigInteger testValue = BigInteger.Parse(parts[0].Trim());
            BigInteger[] numbers = parts[1].Trim().Split(' ').Select(BigInteger.Parse).ToArray();

            if (CanEquationBeTrue(testValue, numbers))
            {
                totalCalibrationResult += testValue;
            }
        }

        Console.WriteLine($"Total calibration result: {totalCalibrationResult}");
    }

    // Check if the equation can be made true using +, *, or ||
    static bool CanEquationBeTrue(BigInteger testValue, BigInteger[] numbers)
    {
        int numOperators = numbers.Length - 1;
        int totalCombinations = (int)Math.Pow(3, numOperators); // 3 operators: +, *, ||

        for (int i = 0; i < totalCombinations; i++)
        {
            char[] operators = new char[numOperators];
            int temp = i;

            for (int j = 0; j < numOperators; j++)
            {
                int opType = temp % 3; // 3 possible operators
                operators[j] = opType == 0 ? '+' : (opType == 1 ? '*' : '|'); // 0 = +, 1 = *, 2 = ||
                temp /= 3;
            }

            BigInteger result = EvaluateExpression(numbers, operators);

            if (result == testValue)
            {
                return true;
            }
        }

        return false;
    }

    // Evaluate the expression left to right, considering concatenation (||)
    static BigInteger EvaluateExpression(BigInteger[] numbers, char[] operators)
    {
        BigInteger result = numbers[0];

        for (int i = 0; i < operators.Length; i++)
        {
            if (operators[i] == '+')
            {
                result += numbers[i + 1];
            }
            else if (operators[i] == '*')
            {
                result *= numbers[i + 1];
            }
            else if (operators[i] == '|') 
            {
                result = BigInteger.Parse(result.ToString() + numbers[i + 1].ToString());
            }
        }

        return result;
    }
}