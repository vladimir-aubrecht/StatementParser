namespace StatementParser.Parsers.Pdf
{
	internal interface IPdfDocumentParser<TDocumentDescriptor> where TDocumentDescriptor : new()
	{
		TDocumentDescriptor Parse(IPdfSource pdfSource);
	}
}