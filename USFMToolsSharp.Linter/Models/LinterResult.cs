using System;
using System.Collections.Generic;
using System.Text;

namespace USFMToolsSharp.Linter.Models
{
    public enum LinterLevel
    {
        Info,
        Warning,
        Error,
    }
    public class LinterResult
    {
        public LinterLevel Level;
        public string Message;
        public int Position;

        public LinterResult()
        {

        }

        public LinterResult(LinterLevel level, string message, int position)
        {
            Level = level;
            Message = message;
            Position = position;
        }

    }

    // Custom comparer for the Product class
    class LinterResultComparer : IEqualityComparer<LinterResult>
    {
        // Products are equal if their names and product numbers are equal.
        public bool Equals(LinterResult x, LinterResult y)
        {

            //Check whether the compared objects reference the same data.
            if (Object.ReferenceEquals(x, y)) return true;

            //Check whether any of the compared objects is null.
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;

            //Check whether the products' properties are equal.
            return
                x.Level == y.Level &&
                x.Message == y.Message &&
                x.Position == y.Position;
        }

        // If Equals() returns true for a pair of objects
        // then GetHashCode() must return the same value for these objects.

        public int GetHashCode(LinterResult result)
        {
            //Check whether the object is null
            if (Object.ReferenceEquals(result, null)) return 0;

            int hashLevel = result.Level.GetHashCode();
            int hashMessage = result.Message.GetHashCode();

            //Calculate the hash code for the product.
            return hashLevel ^ hashMessage ^ result.Position;
        }
    }

}
