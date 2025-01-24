# UrlShortenerService

# Overview

This project is a simple URL Shortener Service built with .NET Core. The service provides the following features:

- Shorten a URL: Generates a unique short URL for a given long URL.

- Retrieve a URL: Redirects users to the original long URL when the short URL is accessed.

- URL Statistics: Tracks and provides statistics such as access counts for each short URL.

- Rate Limiting: Implements limits to prevent abuse of the service.

- Caching: Improves performance for frequently accessed URLs.


# Technologies Used

.NET Core: Backend framework.

Entity Framework Core: Database ORM.

SQL Server: Persistent data store.

IMemoryCache: In-memory caching for performance.

ASP.NET Core Rate Limiting: Middleware for rate limiting.

Swagger: API documentation.


# You can find the database schema in the Migration folder: 
# Database Schema:
CREATE TABLE UrlMappings (
  Id int NOT NULL IDENTITY(1,1) PRIMARY KEY,
  AccessCount int NOT NULL,
  CreatedAt datetime2 NOT NULL,
  ExpiresAt datetime2 NULL,
  OriginalUrl nvarchar(max) NOT NULL,
  ShortUrl nvarchar(max) NOT NULL
);

The application uses a SQL Server database with the following table structure:

| Column Name | Data Type | Description |
|---|---|---|
| Id | int | Primary key, auto-incremented integer. |
| AccessCount | int | Number of times the shortened URL has been accessed. |
| CreatedAt | datetime2 | Date and time when the URL mapping was created. |
| ExpiresAt |datetime2| Date and time when the shortened URL expires (nullable)|
| OriginalUrl | nvarchar(max) | The original, long URL. |
| ShortUrl | nvarchar(max) | The shortened version of the URL. |

# Design Considerations

1. Scalability

To handle increasing traffic and data:

Distributed Caching:

Implement Redis or another distributed cache for storing frequently accessed URL mappings.

This ensures consistency across multiple application instances and improves performance by reducing database queries.

Database Partitioning/Sharding:

Partition the database to distribute data across multiple servers.

Use sharding techniques based on the short URL or user ID to balance database loads.

Horizontal Scaling:

Deploy multiple instances of the application using container orchestration tools like Kubernetes.

Use auto-scaling policies to handle sudden spikes in traffic.

2. High Availability

To ensure the service remains available:

Load Balancing:

Use a load balancer (e.g., Azure Load Balancer, AWS Elastic Load Balancer) to distribute traffic evenly across instances.

Fault Tolerance:

Deploy the application across multiple regions or availability zones to ensure resilience against regional outages.

Redundant Backups:

Schedule periodic backups of the database to ensure data recovery in case of failures.

Health Checks:

Configure health probes to monitor the application's health and remove unhealthy instances from the load balancer.

3. Security

To secure the application and protect user data:

Rate Limiting:

Implement rate limiting to prevent abuse, such as excessive requests from a single IP address.

Input Sanitization:

Validate and sanitize user inputs to prevent SQL injection, cross-site scripting (XSS), and other common attacks.

HTTPS Enforcement:

Ensure all traffic is served over HTTPS to encrypt data in transit.

API Logging:

Log all API requests and responses to monitor usage and detect malicious activity.

Authentication and Authorization:

Add API key-based or token-based authentication to restrict access to authorized users.



