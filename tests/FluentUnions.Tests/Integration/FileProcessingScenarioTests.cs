using System.Text;

namespace FluentUnions.Tests.Integration
{
    /// <summary>
    /// Integration tests demonstrating real-world usage of FluentUnions in file processing scenarios.
    /// </summary>
    public class FileProcessingScenarioTests
    {
        [Fact]
        public async Task ProcessCsvFile_ValidFile_Success()
        {
            // Arrange
            var fileService = new FileProcessingService();
            var csvContent = @"Name,Email,Age
John Doe,john@example.com,30
Jane Smith,jane@example.com,25
Bob Johnson,bob@example.com,35";

            // Act
            var result = await fileService.ProcessCsvFileAsync("users.csv", csvContent);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(3, result.Value.ProcessedRecords);
            Assert.Equal(0, result.Value.FailedRecords);
            Assert.Equal(3, result.Value.Users.Count);
        }

        [Fact]
        public async Task ProcessCsvFile_WithInvalidRecords_PartialSuccess()
        {
            // Arrange
            var fileService = new FileProcessingService();
            var csvContent = @"Name,Email,Age
John Doe,john@example.com,30
Invalid User,not-an-email,25
Jane Smith,jane@example.com,invalid-age
Bob Johnson,bob@example.com,35";

            // Act
            var result = await fileService.ProcessCsvFileAsync("users.csv", csvContent);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(4, result.Value.ProcessedRecords);
            Assert.Equal(2, result.Value.FailedRecords);
            Assert.Equal(2, result.Value.Users.Count);
            Assert.Equal(2, result.Value.Errors.Count);
        }

        [Fact]
        public async Task ProcessCsvFile_EmptyFile_Failure()
        {
            // Arrange
            var fileService = new FileProcessingService();
            var csvContent = "";

            // Act
            var result = await fileService.ProcessCsvFileAsync("empty.csv", csvContent);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal("FILE_EMPTY", result.Error.Code);
        }

        [Fact]
        public async Task BatchProcessFiles_MixedResults()
        {
            // Arrange
            var fileService = new FileProcessingService();
            var files = new Dictionary<string, string>
            {
                ["users1.csv"] = @"Name,Email,Age
John Doe,john@example.com,30",
                ["users2.csv"] = @"Name,Email,Age
Jane Smith,jane@example.com,25",
                ["invalid.csv"] = "Invalid CSV Content",
                ["empty.csv"] = ""
            };

            // Act
            var results = await fileService.BatchProcessFilesAsync(files);

            // Assert
            Assert.Equal(4, results.Count);
            Assert.Equal(2, results.Count(r => r.Value.IsSuccess));
            Assert.Equal(2, results.Count(r => r.Value.IsFailure));
        }

        [Fact]
        public async Task ProcessAndTransformFile_JsonOutput_Success()
        {
            // Arrange
            var fileService = new FileProcessingService();
            var csvContent = @"Name,Email,Age
John Doe,john@example.com,30
Jane Smith,jane@example.com,25";

            // Act
            var result = await fileService.ProcessAndTransformToJsonAsync("users.csv", csvContent);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Contains("john@example.com", result.Value);
            Assert.Contains("jane@example.com", result.Value);
        }

        [Fact]
        public async Task ValidateAndProcessLargeFile_WithProgressTracking()
        {
            // Arrange
            var fileService = new FileProcessingService();
            var progressUpdates = new List<int>();
            
            // Generate large CSV
            var csvBuilder = new StringBuilder("Name,Email,Age\n");
            for (int i = 1; i <= 100; i++)
            {
                csvBuilder.AppendLine($"User{i},user{i}@example.com,{20 + (i % 50)}");
            }

            // Act
            var result = await fileService.ProcessLargeFileWithProgressAsync(
                "large.csv", 
                csvBuilder.ToString(),
                progress => progressUpdates.Add(progress)
            );

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(100, result.Value.ProcessedRecords);
            Assert.True(progressUpdates.Count > 0);
            Assert.Equal(100, progressUpdates.Last());
        }

        // Domain Models
        private class CsvUser
        {
            public string Name { get; set; } = "";
            public string Email { get; set; } = "";
            public int Age { get; set; }
        }

        private class ProcessingResult
        {
            public int ProcessedRecords { get; set; }
            public int FailedRecords { get; set; }
            public List<CsvUser> Users { get; set; } = new();
            public List<ProcessingError> Errors { get; set; } = new();
        }

        private class ProcessingError
        {
            public int LineNumber { get; set; }
            public string Error { get; set; } = "";
        }

        // Service Implementation
        private class FileProcessingService
        {
            public async Task<Result<ProcessingResult>> ProcessCsvFileAsync(string fileName, string content)
            {
                // Validate file
                var validationResult = ValidateFile(fileName, content);
                if (validationResult.IsFailure)
                    return validationResult.Error;

                // Parse CSV
                var lines = content.Split('\n', StringSplitOptions.RemoveEmptyEntries);
                if (lines.Length < 2)
                    return new ValidationError("INVALID_CSV", "CSV must contain header and at least one data row");

                var result = new ProcessingResult();
                var headers = lines[0].Split(',');

                // Process each line
                for (int i = 1; i < lines.Length; i++)
                {
                    var processLineResult = ProcessCsvLine(lines[i], headers, i);
                    
                    processLineResult.OnEither(
                        success: user =>
                        {
                            result.Users.Add(user);
                            result.ProcessedRecords++;
                        },
                        failure: error =>
                        {
                            result.Errors.Add(new ProcessingError 
                            { 
                                LineNumber = i + 1, 
                                Error = error.Message 
                            });
                            result.FailedRecords++;
                            result.ProcessedRecords++;
                        }
                    );
                }

                await Task.Delay(10); // Simulate async processing
                return Result.Success(result);
            }

            public async Task<Dictionary<string, Result<ProcessingResult>>> BatchProcessFilesAsync(
                Dictionary<string, string> files)
            {
                var tasks = files.Select(async kvp =>
                {
                    var result = await ProcessCsvFileAsync(kvp.Key, kvp.Value);
                    return (kvp.Key, result);
                });

                var results = await Task.WhenAll(tasks);
                return results.ToDictionary(r => r.Key, r => r.result);
            }

            public async Task<Result<string>> ProcessAndTransformToJsonAsync(string fileName, string content)
            {
                var processingResult = await ProcessCsvFileAsync(fileName, content);
                
                return processingResult.Map(result =>
                {
                    var jsonBuilder = new StringBuilder();
                    jsonBuilder.AppendLine("[");
                    
                    for (int i = 0; i < result.Users.Count; i++)
                    {
                        var user = result.Users[i];
                        jsonBuilder.Append($"  {{ \"name\": \"{user.Name}\", \"email\": \"{user.Email}\", \"age\": {user.Age} }}");
                        if (i < result.Users.Count - 1)
                            jsonBuilder.AppendLine(",");
                        else
                            jsonBuilder.AppendLine();
                    }
                    
                    jsonBuilder.Append("]");
                    return jsonBuilder.ToString();
                });
            }

            public async Task<Result<ProcessingResult>> ProcessLargeFileWithProgressAsync(
                string fileName, 
                string content,
                Action<int> onProgress)
            {
                var validationResult = ValidateFile(fileName, content);
                if (validationResult.IsFailure)
                    return validationResult.Error;

                var lines = content.Split('\n', StringSplitOptions.RemoveEmptyEntries);
                var result = new ProcessingResult();
                var headers = lines[0].Split(',');

                for (int i = 1; i < lines.Length; i++)
                {
                    var processLineResult = ProcessCsvLine(lines[i], headers, i);
                    
                    processLineResult.OnEither(
                        success: user => result.Users.Add(user),
                        failure: error => result.Errors.Add(new ProcessingError 
                        { 
                            LineNumber = i + 1, 
                            Error = error.Message 
                        })
                    );

                    result.ProcessedRecords++;

                    // Report progress
                    if (i % 10 == 0 || i == lines.Length - 1)
                    {
                        var progress = (int)((i / (double)(lines.Length - 1)) * 100);
                        onProgress(progress);
                        await Task.Delay(1); // Simulate processing time
                    }
                }

                return Result.Success(result);
            }

            private Result ValidateFile(string fileName, string content)
            {
                if (string.IsNullOrWhiteSpace(fileName))
                    return new ValidationError("INVALID_FILENAME", "File name is required");

                if (!fileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
                    return new ValidationError("INVALID_FILE_TYPE", "Only CSV files are supported");

                if (string.IsNullOrWhiteSpace(content))
                    return new ValidationError("FILE_EMPTY", "File content is empty");

                return Result.Success();
            }

            private Result<CsvUser> ProcessCsvLine(string line, string[] headers, int lineNumber)
            {
                var values = line.Split(',');
                
                if (values.Length != headers.Length)
                    return new ValidationError("INVALID_ROW", $"Line {lineNumber}: Expected {headers.Length} columns but found {values.Length}");

                var nameIndex = Array.IndexOf(headers, "Name");
                var emailIndex = Array.IndexOf(headers, "Email");
                var ageIndex = Array.IndexOf(headers, "Age");

                if (nameIndex < 0 || emailIndex < 0 || ageIndex < 0)
                    return new ValidationError("MISSING_HEADERS", "CSV must contain Name, Email, and Age columns");

                // Validate email
                var email = values[emailIndex].Trim();
                if (!email.Contains('@') || !email.Contains('.'))
                    return new ValidationError("INVALID_EMAIL", $"Line {lineNumber}: Invalid email format");

                // Parse age
                if (!int.TryParse(values[ageIndex].Trim(), out var age))
                    return new ValidationError("INVALID_AGE", $"Line {lineNumber}: Age must be a valid number");

                return new CsvUser
                {
                    Name = values[nameIndex].Trim(),
                    Email = email,
                    Age = age
                };
            }
        }
    }
}