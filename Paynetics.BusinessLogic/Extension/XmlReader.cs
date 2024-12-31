namespace System.Xml
{
    public static class XmlReaderExtensions
    {
        public static async Task<DateTimeOffset> ReadContentAsDateTimeOffsetAsync(this XmlReader reader)
        {
            if (reader.IsStartElement() || reader.NodeType == XmlNodeType.Text)
            {
                string dateTimeString = await reader.ReadElementContentAsStringAsync();

                if (DateTimeOffset.TryParse(dateTimeString, out var dateTimeOffset))
                {
                    return dateTimeOffset;
                }
                else
                {
                    throw new FormatException($"Invalid DateTimeOffset format: {dateTimeString}");
                }
            }

            throw new InvalidOperationException("XmlReader is not at a valid start element to read DateTimeOffset.");
        }

        public static async Task<int> ReadContentAsIntAsync(this XmlReader reader)
        {
            if (reader.IsStartElement() || reader.NodeType == XmlNodeType.Text)
            {
                // Read the element content as a string asynchronously.
                string intString = await reader.ReadElementContentAsStringAsync();

                if (int.TryParse(intString, out var intValue))
                {
                    return intValue;
                }
                else
                {
                    throw new FormatException($"Invalid integer format: {intString}");
                }
            }

            throw new InvalidOperationException("XmlReader is not at a valid start element to read integer.");
        }

        public static async Task<decimal> ReadContentAsDecimalAsync(this XmlReader reader)
        {
            if (reader.IsStartElement() || reader.NodeType == XmlNodeType.Text)
            {
                string decimalString = await reader.ReadElementContentAsStringAsync();

                if (decimal.TryParse(decimalString, out var decimalValue))
                {
                    return decimalValue;
                }
                else
                {
                    throw new FormatException($"Invalid decimal format: {decimalString}");
                }
            }

            throw new InvalidOperationException("XmlReader is not at a valid start element to read decimal.");
        }
    }

}
