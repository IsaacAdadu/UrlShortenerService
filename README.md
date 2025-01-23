# UrlShortenerService
# You can find the database schema in the Migration folder: 
# Database Schema:
CREATE TABLE UrlMappings (
  Id int NOT NULL IDENTITY(1,1) PRIMARY KEY,
  AccessCount int NOT NULL,
  CreatedAt datetime2 NOT NULL,
  OriginalUrl nvarchar(max) NOT NULL,
  ShortUrl nvarchar(max) NOT NULL
);

The application uses a SQL Server database with the following table structure:

| Column Name | Data Type | Description |
|---|---|---|
| Id | int | Primary key, auto-incremented integer. |
| AccessCount | int | Number of times the shortened URL has been accessed. |
| CreatedAt | datetime2 | Date and time when the URL mapping was created. |
| OriginalUrl | nvarchar(max) | The original, long URL. |
| ShortUrl | nvarchar(max) | The shortened version of the URL. |



