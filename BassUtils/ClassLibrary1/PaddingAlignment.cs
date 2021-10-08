﻿namespace ClassLibrary1
{
    /// <summary>
    /// How to align the input string when padding.
    /// </summary>
    /// <remarks>
    /// I hate enums, can never remember:
    /// a.ToString() gives "Left"
    /// Enum.Parse(typeof(Alignment), "Right") gives Alignment.Right
    /// 
    /// (char)a gives '&lt;'
    /// var b = (Alignment)'&gt;' // b is "Right"
    /// </remarks>
    public enum PaddingAlignment
    {
        /// <summary>
        /// No alignment.
        /// </summary>
        None = 0,

        /// <summary>
        /// Align the input string to the left: "ab" -> "ab  ".
        /// </summary>
        Left = '<',

        /// <summary>
        /// Align the input string to the right: "ab" -> "  ab".
        /// </summary>
        Right = '>',

        /// <summary>
        /// Align the input string to the center: "ab" -> " ab ".
        /// </summary>
        Center = '^'
    }
}
