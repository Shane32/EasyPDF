namespace Tests
{
    public class LineStyleTests
    {
        [Fact]
        public void LineStyleComparable()
        {
            var style1 = new LineStyle(dashStyle: new LineDashStyle(new[] { 1f, 2f }, 1f));
            var style2 = new LineStyle(dashStyle: new LineDashStyle(new[] { 1f, 2f }, 1f));
            if (style1 != style2)
                throw new InvalidOperationException("Equals operator fails");
            if (!object.Equals(style1, style2))
                throw new InvalidOperationException("Equals object comparison fails");
            if (!style1.Equals(style2))
                throw new InvalidOperationException("Equals typed comparison fails");
        }

        [Fact]
        public void LineStyleComparable_different()
        {
            var style1 = new LineStyle(dashStyle: new LineDashStyle(new[] { 1f, 2f }, 1f));
            var style2 = new LineStyle(dashStyle: new LineDashStyle(new[] { 1f, 2f }, 2f));
            if (style1 == style2)
                throw new InvalidOperationException("Equals operator fails");
            if (object.Equals(style1, style2))
                throw new InvalidOperationException("Equals object comparison fails");
            if (style1.Equals(style2))
                throw new InvalidOperationException("Equals typed comparison fails");
        }
    }
}
