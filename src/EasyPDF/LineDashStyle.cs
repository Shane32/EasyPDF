using System;

namespace Shane32.EasyPDF
{
    /// <summary>
    /// Represents a line dash pattern, such as a solid line, dashed or dotted pattern.
    /// </summary>
    public class LineDashStyle
    {
        private readonly float[] _array;
        /// <summary>
        /// Returns the phase distance as measured in multiples of the line width.
        /// </summary>
        public float Phase { get; }

        /// <summary>
        /// Creates a uniform on-off pattern.
        /// </summary>
        /// <param name="unitsOnOff">As measured in multiples of the line width, the on and off distance of the pattern.</param>
        /// <param name="phase">As measured in multiples of the line width, the distance through the pattern when starting a line.</param>
        public LineDashStyle(float unitsOnOff, float phase)
            : this(new float[] { unitsOnOff, unitsOnOff }, phase)
        {
        }

        /// <summary>
        /// Creates a custom on-off pattern.
        /// </summary>
        /// <param name="unitsOn">As measured in multiples of the line width, the on distance of the pattern.</param>
        /// <param name="unitsOff">As measured in multiples of the line width, the off distance of the pattern.</param>
        /// <param name="phase">As measured in multiples of the line width, the distance through the pattern when starting a line.</param>
        public LineDashStyle(float unitsOn, float unitsOff, float phase)
            : this(new float[] { unitsOn, unitsOff }, phase)
        {
        }

        /// <summary>
        /// Creates a custom on-off pattern.
        /// </summary>
        /// <param name="array">As measured in multiples of the line width, a list of alternating on/off distances of the pattern, begnning with an 'on' distance.</param>
        /// <param name="phase">As measured in multiples of the line width, the distance through the pattern when starting a line.</param>
        public LineDashStyle(float[] array, float phase)
        {
            if (array is null)
                throw new ArgumentNullException(nameof(array));
            _array = array;
            Phase = phase;
        }

        /// <summary>
        /// Returns a copy of the distance array, measured in multiples of the line width.
        /// </summary>
        public float[] Array => _array.Duplicate();

        /// <summary>
        /// Returns a copy of <see cref="Array"/> multiplied by the specified value.
        /// </summary>
        public float[] MultipliedArray(float multiplier)
        {
            float[] x = _array.Duplicate();
            for (int i = 0; i < x.Length; i++)
                x[i] *= multiplier;
            return x;
        }

        /// <summary>
        /// Returns the <see cref="Phase"/> multiplied by the specified value.
        /// </summary>
        public float MultipliedPhase(float multiplier)
            => Phase * multiplier;

        /// <summary>
        /// Represents a solid line.
        /// </summary>
        public static readonly LineDashStyle Solid = new(System.Array.Empty<float>(), 0f);

        /// <summary>
        /// Represents a dashed line.
        /// </summary>
        public static readonly LineDashStyle Dash = new(6f, 6f, 3f);

        /// <summary>
        /// Represents a dotted line.
        /// </summary>
        public static readonly LineDashStyle Dot = new(2f, 3f, 0f);

        /// <summary>
        /// Represents a dash-dot-dot line pattern.
        /// </summary>
        public static readonly LineDashStyle DashDotDot = new(new float[] { 6f, 3f, 2f, 3f, 2f, 3f }, 0f);
    }
}
