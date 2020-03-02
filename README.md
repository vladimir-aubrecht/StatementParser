# StatementParser ![.NET Core](https://github.com/vladimir-aubrecht/StatementParser/workflows/.NET%20Core/badge.svg)

Idea behind the StatementParser is, that it would be nice to be able to process financial data from different kind of statements in automatized way.
This is often pretty hard as brokers are giving these data only in form of xls/xlst/pdf or other format which is not directly processable and here comes StatmentParser.

StatementParser is taking statement file from your broker on the input and converting it into preffered format.

# Usages
There are two ways how you can use the project:
- As a .Net Core library you can include parsing within your project.
- As multiplatform utility, you can directly use it to convert statement into other format.

## Library
```csharp
string filePath = "<absolute path to file>";
var parser = new TransactionParser();
IList<Transaction> result = parser.Parse(filePath);

// Result can be null in case no internal parser was able to parse input file.
if (result != null)
{
  // Do something
}
```

## Utility
Plain text conversion:

``dotnet StatementParserCLI.dll <path to the file or folder containing statements>``

JSON conversion:

``dotnet StatementParserCLI.dll -j <path to the file or folder containing statements>``

XSLT conversion:

``dotnet StatementParserCLI.dll -x <path to file with output xslt file> <path to the file or folder containing statements>``

# Future plans
- Support for Revolut broker
- Add support for sale transactions in FIFO way for all brokers.
- Add support for auto-download of statements.

# Guarantees
Use on your own risk.
Official guidance is, verify all generated numbers manually. Currently there are no known bugs.

# Supported output formats
- JSON
- Plain text
- XSLX

# Supported file formats for brokers

|Type / Broker        |Fidelity|Morgan Stanley|MetaTrader|FxChoice|Degiro      |Lynx|Interactive Broker|Revolut|
|---------------------|--------|--------------|----------|--------|------------|----|------------------|-------|
|Sale transactions    | -      | -            |Htm       |Htm     | -          | -  | -                | -     |
|Dividend transactions|Pdf     |Pdf, Xls      | -        | -      |Csv in Czech|Csv |Csv               | -     |
|ESPP transactions    |Pdf     | -            | -        | -      | -          | -  | -                | -     |
|Deposit transactions |Pdf     |Pdf, Xls      | -        | -      | -          | -  | -                | -     |

## Examples of how parsable statements looks like

### Fidelity Statement
![Fidelity Statement][Fidelity]

### FXChoice Statement
![FXChoice Statement][FXChoice]

### MorganStanley Statement
![MorganStanley Statement][MorganStanley]

### Lynx Statement
![Lynx Statement][Lynx]

[Fidelity]: Docs/Images/FidelityStatement.png
[FXChoice]: Docs/Images/FXChoiceStatement.png
[MorganStanley]: Docs/Images/MorganStanleyStatement.png
[Lynx]: Docs/Images/LynxStatement.png